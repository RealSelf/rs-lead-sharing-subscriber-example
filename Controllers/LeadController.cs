using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Amazon.SimpleNotificationService.Util;

using rs_lead_sharing_subscriber_example.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using System;
using rs_lead_sharing_subscriber_example.Repository;
using System.Linq;

namespace rs_lead_sharing_subscriber_example
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly InMemoryDB _inMemoryDB;

        public LeadController(IHttpClientFactory httpClientFactory, InMemoryDB inMemoryDB) {
            _httpClientFactory = httpClientFactory;
            _inMemoryDB = inMemoryDB;

}
        // GET: api/Lead
        [HttpGet]
        public IActionResult Get()
        {
            InMemoryLead[] leads = _inMemoryDB.Leads.ToArray();
            return Ok(leads);
        }

        // GET: api/Lead/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(string id)
        {
            InMemoryLead lead = _inMemoryDB.Leads.Find(id);
            if (lead == null)
            {
                return NotFound();
            }
            return Ok(lead);
        }

        // POST: api/Lead
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                string rawInput = await reader.ReadToEndAsync();
                Message SNSParsedMessage = Message.ParseMessage(rawInput);
                Console.WriteLine(rawInput);

                // Is Valid SNS Signature
                if (!SNSParsedMessage.IsMessageSignatureValid())
                {
                    return Unauthorized("Signature not valid");
                }

                // Should we confirm the subscriber
                if (SNSParsedMessage.IsSubscriptionType)
                {
                    string SNSSubscribeUrl = SNSParsedMessage.SubscribeURL;
                    var httpClient = _httpClientFactory.CreateClient();
                    await httpClient.GetAsync(SNSSubscribeUrl);
                    return NoContent();
                }

                // Process Lead Message
                SubscriptionPayload message = JsonConvert.DeserializeObject<SubscriptionPayload>(SNSParsedMessage.MessageText);
                Console.WriteLine(message.Callback);

                var leadToSave = new InMemoryLead
                {
                    id = message.Lead.Id,
                    subscriptionPayload = SNSParsedMessage.MessageText,
                };

                if (_inMemoryDB.Leads.Any(n => n.id == message.Lead.Id))
                {
                    _inMemoryDB.Update(leadToSave);
                } else
                {
                    _inMemoryDB.Leads.Add(leadToSave);
                }
                
                
                _inMemoryDB.SaveChanges();
                return Ok(message);
            }           
        }
    }
}
