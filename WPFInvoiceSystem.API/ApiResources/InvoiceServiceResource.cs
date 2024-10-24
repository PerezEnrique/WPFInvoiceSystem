namespace WPFInvoiceSystem.API.ApiResources
{
    public record InvoiceServiceResource(
        int Id,
        ServiceResource Service,
        int Quantity
        );
}
