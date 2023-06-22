using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Entities;

namespace WPFInvoiceSystem.Persistance.EntityConfigs
{
    public class BankConfiguration
    {
        public BankConfiguration(EntityTypeBuilder<Bank> entityBuilder)
        {
            entityBuilder
                 .Property(b => b.Name)
                 .IsRequired()
                 .HasMaxLength(50);
        }
    }
}
