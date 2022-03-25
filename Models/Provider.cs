using Newtonsoft.Json;

namespace rs_lead_sharing_subscriber_example.Models
{
    public class Provider
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}