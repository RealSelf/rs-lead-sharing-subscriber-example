using System;
using Microsoft.EntityFrameworkCore;
using rs_lead_sharing_subscriber_example.Models;

namespace rs_lead_sharing_subscriber_example.Repository
{
    public class InMemoryLead
    {
        public int id { get; set; }
        public string subscriptionPayload { get; set; }
    }

    public class InMemoryDB: DbContext
    {
       public InMemoryDB(DbContextOptions<InMemoryDB> options)
       : base(options) { }

        public DbSet<InMemoryLead> Leads => Set<InMemoryLead>();
    }
}
