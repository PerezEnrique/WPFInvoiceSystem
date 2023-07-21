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
            //Table restrictions
            entityBuilder
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(75);

            //Dummy data seeding
            var initialServices = new List<Service>
            {
                new Service
                {
                    Id = 1,
                    IsExempt = true,
                    Name = "Service 1",
                    Price = 20,
                    TypeId = 1
                },
                new Service
                {
                    Id = 2,
                    IsExempt = false,
                    Name = "Service 2",
                    Price = 50,
                    TypeId = 2
                }
            };

            entityBuilder.HasData(initialServices);
        }
    }
}
