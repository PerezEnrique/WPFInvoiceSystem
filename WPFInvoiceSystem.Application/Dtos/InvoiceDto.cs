using System.Collections.ObjectModel;

namespace WPFInvoiceSystem.Application.Dtos
{
    public record InvoiceDto(
        CustomerDto Customer,
        DateTime Date,
        Collection<InvoiceServiceDto> InvoiceServices
        );
}
