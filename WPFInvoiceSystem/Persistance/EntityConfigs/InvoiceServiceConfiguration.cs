using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Modals;
    
namespace WPFInvoiceSystem.Persistance.EntityConfigs
{
    public class InvoiceServiceConfiguration
    {
        public InvoiceServiceConfiguration(EntityTypeBuilder<InvoiceService> entityBuilder)
        {
            entityBuilder
                .HasKey(iS => new { iS.InvoiceId, iS.ServiceId });
        }
    }
}
