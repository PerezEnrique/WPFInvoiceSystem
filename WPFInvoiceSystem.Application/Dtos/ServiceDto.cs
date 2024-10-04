namespace WPFInvoiceSystem.Application.Dtos
{
    public record ServiceDto(
            int Id,
            string Name,
            decimal Price,
            bool IsExempt,
            ServiceTypeDto Type
        );
}
