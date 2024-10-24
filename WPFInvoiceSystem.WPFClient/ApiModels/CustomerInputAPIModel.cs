using System;

namespace WPFInvoiceSystem.WPFClient.ApiModels
{
    public record CustomerInputAPIModel
    (
        string Name,
        int IdentityCard,
        string Phone,
        string Address,
        DateOnly? Birthdate
    );
}
