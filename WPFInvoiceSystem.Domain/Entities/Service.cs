using System.Collections.ObjectModel;
using WPFInvoiceSystem.Domain.Exceptions;

namespace WPFInvoiceSystem.Domain.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public virtual Collection<InvoiceService> Invoices { get; set; } = [];
        public bool IsExempt { get; set; }

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
        public decimal Price { get; set; }

        private ServiceType _type;
        public virtual required ServiceType Type
        {
            get { return _type; }
            set
            {
                EnsureTypeIsNotNull(value);
                _type = value;
            }
        }
        private static void EnsureNameIsNotEmpty(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new CoreValidationException("Name is required");
        }

        private static void EnsureTypeIsNotNull(ServiceType type)
        {
            if (type == null)
                throw new CoreValidationException("Service type is required");
        }
    }
}
