using System.Collections.ObjectModel;

namespace WPFInvoiceSystem.Application.Dtos
{
    public record InvoiceDto(
        int Id,
        CustomerDto Customer,
        DateTime Date,
        Collection<InvoiceServiceDto> InvoiceServices
        );
}
