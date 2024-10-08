using System.Collections.ObjectModel;

namespace WPFInvoiceSystem.WPFClient.Models
{
    public class ServiceModel
    {
        public int Id { get; set; }
        public ObservableCollection<InvoiceServiceModel> Invoices { get; set; } = [];
        public bool IsExempt { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public required ServiceTypeModel Type { get; set; }
    }
}
