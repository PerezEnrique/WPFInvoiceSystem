using System.Collections.ObjectModel;

namespace WPFInvoiceSystem.Application.Dtos
{
    public record InvoiceDto(
        int Id,
        int InvoiceNumber,
        decimal Exempt,
        decimal Tax,
        decimal TaxBase,
        CustomerDto Customer,
        DateTime Date,
        Collection<InvoiceServiceDto> InvoiceServices
        );
}
