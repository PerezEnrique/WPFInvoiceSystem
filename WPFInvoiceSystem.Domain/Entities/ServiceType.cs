using WPFInvoiceSystem.Domain.Exceptions;

namespace WPFInvoiceSystem.Domain.Entities
{
    public class ServiceType
    {
        public int Id { get; set; }
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

        private static void EnsureNameIsNotEmpty(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new CoreValidationException("Name is required");
        }
    }
}
