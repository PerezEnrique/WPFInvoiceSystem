using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WPFInvoiceSystem.Domain.Entities;

namespace WPFInvoiceSystem.Persistance.EntityConfigs
{
    public static class ServiceEntityConfiguration
    {
        public static void Configure(EntityTypeBuilder<Service> builder)
        {
            builder
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
