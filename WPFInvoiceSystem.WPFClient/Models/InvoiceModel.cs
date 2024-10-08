using System;
using System.Collections.ObjectModel;

namespace WPFInvoiceSystem.WPFClient.Models
{
    public class InvoiceModel
    {
        public int Id { get; set; }
        public required CustomerModel Customer { get; set; }
        public DateTime Date { get; set; }
        public decimal Exempt { get; set; }
        public int InvoiceNumber { get; set; }
        public ObservableCollection<InvoiceServiceModel> Services { get; set; } = [];
        public bool IsPaid { get; set; }
        public decimal Tax { get; set; }
        public decimal TaxBase { get; set; }
        public decimal Total { get; set; }

    }
}
