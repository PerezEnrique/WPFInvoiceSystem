namespace WPFInvoiceSystem.Application.Dtos
{
    public record CustomerInputDto
    (
        string Name,
        int IdentityCard,
        string Phone,
        string Address,
        DateOnly? Birthdate
    );
}
