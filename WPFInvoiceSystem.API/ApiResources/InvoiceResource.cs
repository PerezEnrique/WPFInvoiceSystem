using System.Collections.ObjectModel;

namespace WPFInvoiceSystem.API.ApiResources
{
    public record InvoiceResource(
        int Id,
        CustomerResource Customer,
        DateTime Date,
        Collection<InvoiceServiceResource> InvoiceServices
        );
}
