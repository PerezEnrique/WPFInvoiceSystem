namespace WPFInvoiceSystem.WPFClient.ApiModels
{
    public record ServiceInputAPIModel(
        string Name,
        decimal Price,
        int TypeId,
        bool IsExempt
        );
}
