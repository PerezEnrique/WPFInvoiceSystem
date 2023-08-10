using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Modals;

namespace WPFInvoiceSystem.Persistance.EntityConfigs
{
    public class InvoiceConfiguration
    {
        public InvoiceConfiguration(EntityTypeBuilder<Invoice> entityBuilder)
        {

            entityBuilder
                .HasIndex(i => i.InvoiceNumber)
                .IsUnique();
        }
    }
}
