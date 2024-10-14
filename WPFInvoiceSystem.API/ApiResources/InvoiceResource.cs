using System.Collections.ObjectModel;

namespace WPFInvoiceSystem.API.ApiResources
{
    public record InvoiceResource(
        int Id,
        int InvoiceNumber,
        decimal Exempt,
        decimal Tax,
        decimal TaxBase,
        CustomerResource Customer,
        DateTime Date,
        Collection<InvoiceServiceResource> InvoiceServices
        );
}
