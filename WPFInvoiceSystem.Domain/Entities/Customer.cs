using WPFInvoiceSystem.Domain.Exceptions;

namespace WPFInvoiceSystem.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Address { get; set; } = string.Empty;
        public DateTime? Birthdate { get; set; }
        public int IdentityCard { get; set; }
        private string _name = string.Empty;
        public required string Name
        {
            get { return _name; }
            set
            {
                EnsureNameIsNotEmpty(name: value);
                _name = value;
            }
        }
        public string Phone { get; set; } = string.Empty;

        private static void EnsureNameIsNotEmpty(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new CoreValidationException("Name is required");
        }
    }
}
