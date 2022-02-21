using System;
namespace rs_lead_sharing_subscriber_example.Models
{
    public class Patient
    {
        public string id { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string preferred_contact_method { get; set; }
    }
}
