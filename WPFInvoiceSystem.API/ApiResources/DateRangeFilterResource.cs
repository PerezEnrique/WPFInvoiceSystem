using System.ComponentModel.DataAnnotations;

namespace WPFInvoiceSystem.API.ApiResources
{
    public record DateRangeFilterResource(
        DateTime FromDate,
        DateTime ToDate
    ) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            bool fromDataGreateThanToDate = FromDate > ToDate;

            if (fromDataGreateThanToDate)
                yield return new ValidationResult(
                    "FromDate cannot be greater than ToDate"
                );
        }
    }
}
