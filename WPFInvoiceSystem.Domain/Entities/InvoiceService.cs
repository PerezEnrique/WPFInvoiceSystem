namespace WPFInvoiceSystem.Domain.Entities
{
    public class InvoiceService
    {
        public int Id { get; set; }
        public virtual required Invoice Invoice { get; set; }
        public virtual required Service Service { get; set; }
        public int Quantity { get; set; }
    }
}
