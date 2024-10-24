using System.ComponentModel.DataAnnotations;

namespace WPFInvoiceSystem.API.ApiResources
{
    public record ServiceInputResource(
        [Required][StringLength(50)] string Name,
        decimal Price,
        int TypeId,
        bool IsExempt
        );
}
