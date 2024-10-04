namespace WPFInvoiceSystem.Application.Dtos
{
    public record CustomerDto(
        int Id,
        string Name,
        int IdentityCard,
        string Phone,
        string Address,
        DateOnly? Birthday
        );
}
