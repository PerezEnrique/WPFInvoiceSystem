using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Entities;
using WPFInvoiceSystem.Persistance.EntityConfigs;

namespace WPFInvoiceSystem.Persistance
{
    public class AppDbContext : DbContext
    {
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceService> InvoicesServices { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceType> ServicesTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                connectionString: "Filename=" + Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "AppDb.db"));

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new BankConfiguration(modelBuilder.Entity<Bank>());
            new CustomerConfiguration(modelBuilder.Entity<Customer>());
            new InvoiceConfiguration(modelBuilder.Entity<Invoice>());
            new InvoiceServiceConfiguration(modelBuilder.Entity<InvoiceService>());
            new PaymentConfiguration(modelBuilder.Entity<Payment>());
            new PaymentMethodConfiguration(modelBuilder.Entity<PaymentMethod>());
            new ServiceConfiguration(modelBuilder.Entity<Service>());
            new ServiceTypeConfiguration(modelBuilder.Entity<ServiceType>());
        }
    }
}
