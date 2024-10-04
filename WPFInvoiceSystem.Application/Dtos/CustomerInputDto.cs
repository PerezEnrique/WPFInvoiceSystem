namespace WPFInvoiceSystem.Application.Dtos
{
    public record CustomerInputDto
    (
        int Id,
        string Name,
        int IdentityCard,
        string Phone,
        string Address,
        DateOnly? Birthdate
    );
}
