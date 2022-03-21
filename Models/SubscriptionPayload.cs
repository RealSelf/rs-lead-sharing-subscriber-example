using System;
using Newtonsoft.Json;

namespace rs_lead_sharing_subscriber_example.Models
{
    public class SubscriptionPayload
    {
        [JsonProperty("lead")]
        public Lead Lead { get; set; }
        [JsonProperty("callback")]
        public string Callback { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
