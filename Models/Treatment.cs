﻿using System;
using Newtonsoft.Json;

namespace rs_lead_sharing_subscriber_example.Models
{
    public class Treatment
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
