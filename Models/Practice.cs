using Newtonsoft.Json;

namespace rs_lead_sharing_subscriber_example.Models
{
    public class Practice
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
    }
}