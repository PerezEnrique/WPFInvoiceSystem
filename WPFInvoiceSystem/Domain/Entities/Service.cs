using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFInvoiceSystem.Domain.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public virtual ObservableCollection<InvoiceService> Invoices { get; set; }
        public bool IsExempt { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public virtual ServiceType Type { get; set; }
        public int TypeId { get; set; } //This was added here only to ease the process of injecting the dummy services
    }
}
