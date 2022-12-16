using AutoVerleih.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoVerleih.Data
{
    public class DBProjectContext : DbContext
    {
        public DBProjectContext (DbContextOptions<DBProjectContext> options):base(options)
        { 
        }

        public DbSet<Kunden> Kunde { get; set; }

        public DbSet<AutoVerleih.Models.Autos> Autos { get; set; }

        public DbSet<AutoVerleih.Models.Verleih> Verleih { get; set; }
//        public DbSet<Rechnung> Rechnung { get; set; }

    }
}
