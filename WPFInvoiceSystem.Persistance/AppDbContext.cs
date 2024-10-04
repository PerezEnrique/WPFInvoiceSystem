using Microsoft.EntityFrameworkCore;
using WPFInvoiceSystem.Domain.Entities;
using WPFInvoiceSystem.Persistance.EntityConfigs;

namespace WPFInvoiceSystem.Persistance
{
    public class AppDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceService> InvoicesServices { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceType> ServicesTypes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CustomerEntityConfiguration.Configure(modelBuilder.Entity<Customer>());
            InvoiceEntityConfiguration.Configure(modelBuilder.Entity<Invoice>());
            ServiceEntityConfiguration.Configure(modelBuilder.Entity<Service>());
            ServiceTypeEntityConfiguration.Configure(modelBuilder.Entity<ServiceType>());

            base.OnModelCreating(modelBuilder);
        }
    }
}
