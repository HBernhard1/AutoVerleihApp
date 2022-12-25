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

        public DbSet<Autos> Autos { get; set; }

        public DbSet<Verleih> Verleih { get; set; }

        public DbSet<Accounts> Accounts { get; set; }
    }
}
