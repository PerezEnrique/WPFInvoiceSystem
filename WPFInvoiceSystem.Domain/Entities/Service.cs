using System.Collections.ObjectModel;

namespace WPFInvoiceSystem.Domain.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public virtual Collection<InvoiceService> Invoices { get; set; } = [];
        public bool IsExempt { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public required virtual ServiceType Type { get; set; }
        public int TypeId { get; set; } //This was added here only to ease the process of injecting the dummy services
    }
}
