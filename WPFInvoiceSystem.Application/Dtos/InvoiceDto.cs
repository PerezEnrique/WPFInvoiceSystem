using System.Collections.ObjectModel;

namespace WPFInvoiceSystem.Application.Dtos
{
    public record InvoiceDto(
        int Id,
        int InvoiceNumber,
        decimal Exempt,
        decimal Tax,
        decimal TaxBase,
        bool IsPaid,
        CustomerDto Customer,
        DateTime Date,
        Collection<InvoiceServiceDto> InvoiceServices
        );
}
