using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace WPFInvoiceSystem.Domain.Entities
{
    public class InvoiceService
    {
        public int InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }
        public virtual Service Service { get; set; }
        public int ServiceId { get; set; }
        public decimal FinalPrice { get; set; }
        public int Quantity { get; set; }
    }
}
