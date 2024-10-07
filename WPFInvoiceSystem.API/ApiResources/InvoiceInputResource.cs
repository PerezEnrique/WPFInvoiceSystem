namespace WPFInvoiceSystem.API.ApiResources
{
    public record InvoiceInputResource(
        DateTime Date,
        int InvoiceNumber,
        int CustomerId,
        IEnumerable<InvoiceServiceInputResource> InvoiceServices
        );
}
