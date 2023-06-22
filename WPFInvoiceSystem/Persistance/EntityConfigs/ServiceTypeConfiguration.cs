using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Entities;

namespace WPFInvoiceSystem.Persistance.EntityConfigs
{
    public class ServiceTypeConfiguration
    {
        public ServiceTypeConfiguration(EntityTypeBuilder<ServiceType> entityBuilder)
        {
            //Table restrictions//Table restrictions
            entityBuilder
                .Property(st => st.Name)
                .IsRequired()
                .HasMaxLength(25);
        }
    }
}
