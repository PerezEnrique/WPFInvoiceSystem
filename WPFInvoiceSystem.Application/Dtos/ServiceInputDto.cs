namespace WPFInvoiceSystem.Application.Dtos
{
    public record ServiceInputDto(
        string Name,
        decimal Price,
        int TypeId,
        bool IsExempt
        );
}
