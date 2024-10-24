using System.ComponentModel.DataAnnotations;

namespace WPFInvoiceSystem.API.ApiResources
{
    public record ServiceTypeInputResource(
        [Required][StringLength(25)] string Name
        );
}
