using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFInvoiceSystem.Domain.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public virtual Customer Customer { get; set; }
        public DateTime Date { get; set; }
        public decimal Exempt { get; set; }
        public int InvoiceNumber { get; set; }
        public virtual ObservableCollection<InvoiceService> Services { get; set; }
        public bool IsPaid { get; set; }
        public decimal Tax { get; set; }
        public decimal TaxBase { get; set; }
        public decimal Total { get; set; }

        public Invoice()
        {
            Services = new ObservableCollection<InvoiceService>();
        }
    }
}
