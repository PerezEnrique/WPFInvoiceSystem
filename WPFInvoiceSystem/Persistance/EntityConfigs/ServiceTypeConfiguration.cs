using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Modals;

namespace WPFInvoiceSystem.Persistance.EntityConfigs
{
    public class ServiceTypeConfiguration
    {
        public ServiceTypeConfiguration(EntityTypeBuilder<ServiceType> entityBuilder)
        {
            //Table restrictions
            entityBuilder
                .Property(st => st.Name)
                .IsRequired()
                .HasMaxLength(25);

            //Dummy data seeding
            var initialTypes = new List<ServiceType>
            {
                new ServiceType
                {
                    Id = 1,
                    Name = "Type A"
                },

                new ServiceType
                {
                    Id = 2,
                    Name = "Type B"
                }
            };

            entityBuilder.HasData(initialTypes);
        }
    }
}
