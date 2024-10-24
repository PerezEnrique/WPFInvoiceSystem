using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WPFInvoiceSystem.Domain.Entities;

namespace WPFInvoiceSystem.Persistance.EntityConfigs
{
    public static class ServiceTypeEntityConfiguration
    {
        public static void Configure(EntityTypeBuilder<ServiceType> builder)
        {
            builder
                .Property(st => st.Name)
                .IsRequired()
                .HasMaxLength(25);
        }
    }
}
