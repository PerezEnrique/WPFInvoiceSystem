namespace WPFInvoiceSystem.Application.Dtos
{
    public record InvoiceServiceDto(
        int Id,
        ServiceDto Service,
        int Quantity
        );
}
