using IoTGateway.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IoTGateway.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Gateway> Gateways { get; set; }

        public DbSet<Peripheral> Peripherals { get; set; }

        public DbSet<Vendor> Vendors { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Gateway>().HasIndex(i => i.UID).IsUnique(true);
            modelBuilder.Entity<Peripheral>().Property(e => e.Status).HasConversion(v => v.ToString(), v => Enum.Parse<Status>(v));
            modelBuilder.Entity<Peripheral>().HasIndex(i => i.UID).IsUnique(true);
            modelBuilder.Entity<Peripheral>().HasIndex(i => i.VendorId);
            modelBuilder.Entity<Peripheral>().HasIndex(i => i.GatewayId);
            modelBuilder.Entity<Vendor>().HasIndex(i => i.Name);
            
        }
    }
}