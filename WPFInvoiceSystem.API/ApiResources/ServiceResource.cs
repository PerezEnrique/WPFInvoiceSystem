namespace WPFInvoiceSystem.API.ApiResources
{
    public record ServiceResource(
            int Id,
            string Name,
            decimal Price,
            bool IsExempt,
            ServiceTypeResource Type
        );
}
