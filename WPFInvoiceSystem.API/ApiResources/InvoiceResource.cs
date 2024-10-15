using System.Collections.ObjectModel;

namespace WPFInvoiceSystem.API.ApiResources
{
    public record InvoiceResource(
        int Id,
        int InvoiceNumber,
        decimal Exempt,
        decimal Tax,
        decimal TaxBase,
        bool IsPaid,
        CustomerResource Customer,
        DateTime Date,
        Collection<InvoiceServiceResource> InvoiceServices
        );
}
