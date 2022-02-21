using System;
namespace rs_lead_sharing_subscriber_example.Models
{
    public class SubscriptionPayload
    {
        public Lead lead { get; set; }
        public string callback { get; set; }
        public string token { get; set; }
    }
}
