namespace WPFInvoiceSystem.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Address { get; set; } = string.Empty;
        public DateTime? Birthdate { get; set; }
        public int IdentityCard { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
