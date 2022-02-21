using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Util;

using rs_lead_sharing_subscriber_example.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace rs_lead_sharing_subscriber_example
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadController : ControllerBase
    {
        private List<Lead> leads = new List<Lead>();

        // GET: api/Lead
        [HttpGet]
        public IEnumerable<Lead> Get()
        {
            return leads;
        }

        // GET: api/Lead/5
        [HttpGet("{id}", Name = "Get")]
        public Lead Get(string id)
        {
            return leads.Find(lead => lead.id == id);
        }

        // POST: api/Lead
        [HttpPost]
        public async Task<Lead> Post([FromBody] object jsonInput)
        {
            string rawInput = jsonInput.ToString();
            Message SNSParsedMessage = Message.ParseMessage(rawInput);
            if (SNSParsedMessage.IsSubscriptionType)
            {
                AmazonSimpleNotificationServiceClient SNSService = new AmazonSimpleNotificationServiceClient();
                await SNSService.ConfirmSubscriptionAsync(SNSParsedMessage.TopicArn, SNSParsedMessage.Token);
                return null;
            }

            dynamic message = JsonConvert.DeserializeObject(SNSParsedMessage.MessageText);
            leads.Add((Lead)message);
            return (Lead)message;
        }
    }
}
