using System;
using Newtonsoft.Json;

namespace rs_lead_sharing_subscriber_example.Models
{
    public class Lead
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("consultation_type")]
        public string ConsultationType { get; set; }
        [JsonProperty("channel")]
        public string Channel { get; set; }
        [JsonProperty("comments")]
        public string Comments { get; set; }
        [JsonProperty("practice")]
        public Practice Practice { get; set; }
        [JsonProperty("provider")]
        public Provider Provider { get; set; }
        [JsonProperty("prospect")]
        public Prospect Prospect { get; set; }
        [JsonProperty("treatment")]
        public Treatment Treatment { get; set; }
    }
}
