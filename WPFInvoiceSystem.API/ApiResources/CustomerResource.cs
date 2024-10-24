namespace WPFInvoiceSystem.API.ApiResources
{
    public record CustomerResource(
        int Id,
        string Name,
        int IdentityCard,
        string Phone,
        string Address,
        DateOnly? Birthdate
        );
}
