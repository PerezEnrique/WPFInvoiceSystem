using System.ComponentModel.DataAnnotations;

namespace WPFInvoiceSystem.API.ApiResources
{
    public record CustomerInputResource
    (
        [Required][StringLength(75)] string Name,
        int IdentityCard,
        [Required][StringLength(15)] string Phone,
        [Required][StringLength(150)] string Address,
        DateOnly? Birthdate
    );
}
