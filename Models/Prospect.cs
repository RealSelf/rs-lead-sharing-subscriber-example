using System;
using Newtonsoft.Json;

namespace rs_lead_sharing_subscriber_example.Models
{
    public class Prospect
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
    }
}
