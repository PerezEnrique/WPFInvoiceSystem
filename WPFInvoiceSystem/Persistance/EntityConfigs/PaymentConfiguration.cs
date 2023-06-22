using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Entities;

namespace WPFInvoiceSystem.Persistance.EntityConfigs
{
    public class PaymentConfiguration
    {
        public PaymentConfiguration(EntityTypeBuilder<Payment> entityType)
        {
            entityType
                .Property(p => p.ReferenceNumber)
                .HasMaxLength(25);
        }
    }
}
