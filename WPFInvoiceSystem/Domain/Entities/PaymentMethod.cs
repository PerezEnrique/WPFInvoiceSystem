using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFInvoiceSystem.Domain.Entities
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public bool IsCash { get; set; }
        public string Name { get; set; }
    }
}
