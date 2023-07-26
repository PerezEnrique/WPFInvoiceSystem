using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Modals;

namespace WPFInvoiceSystem.Persistance.EntityConfigs
{
    public class CustomerConfiguration
    {
        public CustomerConfiguration(EntityTypeBuilder<Customer> entityBuilder)
        {
            entityBuilder
                .Property(c => c.Address)
                .IsRequired()
                .HasMaxLength(150);

            entityBuilder
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(75);

            entityBuilder
                .Property(c => c.Phone)
                .IsRequired()
                .HasMaxLength(15);
        }
    }
}
