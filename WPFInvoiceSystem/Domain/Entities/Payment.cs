using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace WPFInvoiceSystem.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public virtual Bank? Bank { get; set; }
        public bool IsChange { get; set; }
        public virtual Invoice Invoice { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public string? ReferenceNumber { get; set; }
        public decimal Value { get; set; }
    }
}
