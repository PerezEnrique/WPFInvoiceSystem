namespace WPFInvoiceSystem.WPFClient.Models
{
    public class InvoiceServiceModel
    {
        public int Id { get; set; }
        public required ServiceModel Service { get; set; }
        public int Quantity { get; set; }
    }
}
