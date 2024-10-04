using System.Collections.ObjectModel;
using WPFInvoiceSystem.Application.Dtos;
using WPFInvoiceSystem.Domain.Entities;

namespace WPFInvoiceSystem.Application.Utils
{
    public static class ExtensionMethods
    {
        public static CustomerDto AsDto(this Customer customer)
        {
            return new CustomerDto(
                customer.Id,
                customer.Name,
                customer.IdentityCard,
                customer.Phone,
                customer.Address,
                customer.Birthdate
                );
        }

        public static InvoiceDto AsDto(this Invoice invoice)
        {
            return new InvoiceDto(
                invoice.Customer.AsDto(),
                invoice.Date,
                new Collection<InvoiceServiceDto>(invoice.Services.Select(s => s.AsDto()).ToList())
                );
        }

        public static InvoiceServiceDto AsDto(this InvoiceService invoiceService)
        {
            return new InvoiceServiceDto(
                invoiceService.Id,
                invoiceService.Service.AsDto(),
                invoiceService.Quantity
                );
        }

        public static ServiceDto AsDto(this Service service)
        {
            return new ServiceDto(
                service.Id,
                service.Name,
                service.Price,
                service.IsExempt,
                service.Type.AsDto()
                );
        }

        public static ServiceTypeDto AsDto(this ServiceType serviceType)
        {
            return new ServiceTypeDto(serviceType.Id, serviceType.Name);
        }
    }
}
