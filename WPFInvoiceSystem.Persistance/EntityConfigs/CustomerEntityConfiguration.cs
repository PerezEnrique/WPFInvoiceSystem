using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WPFInvoiceSystem.Domain.Entities;

namespace WPFInvoiceSystem.Persistance.EntityConfigs
{
    public static class CustomerEntityConfiguration
    {
        public static void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder
                .Property(c => c.Address)
                .IsRequired()
                .HasMaxLength(150);

            builder
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(75);

            builder
                .Property(c => c.Phone)
                .IsRequired()
                .HasMaxLength(15);
        }
    }
}
