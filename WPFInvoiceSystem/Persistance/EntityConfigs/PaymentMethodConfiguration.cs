using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Entities;

namespace WPFInvoiceSystem.Persistance.EntityConfigs
{
    public class PaymentMethodConfiguration
    {
        public PaymentMethodConfiguration(EntityTypeBuilder<PaymentMethod> entityBuilder)
        {
            entityBuilder
                .Property(pm => pm.Name)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
