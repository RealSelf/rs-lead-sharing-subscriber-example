using System;
namespace rs_lead_sharing_subscriber_example.Models
{
    public class Lead
    {
        public int id { get; set; }
        public Patient patient { get; set; }
        public Treatment treatment { get; set; }
    }
}
