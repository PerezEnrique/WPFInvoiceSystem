using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Entities;

namespace WPFInvoiceSystem.Persistance.EntityConfigs
{
    public class ServiceConfiguration
    {
        public ServiceConfiguration(EntityTypeBuilder<Service> entityBuilder)
        {
            entityBuilder
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(75);
        }
    }
}
