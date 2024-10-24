using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WPFInvoiceSystem.Domain.Entities;

namespace WPFInvoiceSystem.Persistance.EntityConfigs
{
    public static class InvoiceEntityConfiguration
    {
        public static void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder
                .HasIndex(i => i.InvoiceNumber)
                .IsUnique();
        }
    }
}
