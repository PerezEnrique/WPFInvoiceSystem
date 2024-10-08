using System;

namespace WPFInvoiceSystem.WPFClient.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public string Address { get; set; } = string.Empty;
        public DateOnly? Birthdate { get; set; }
        public int IdentityCard { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
