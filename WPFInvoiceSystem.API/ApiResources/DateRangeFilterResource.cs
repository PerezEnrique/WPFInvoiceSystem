using System.ComponentModel.DataAnnotations;

namespace WPFInvoiceSystem.API.ApiResources
{
    public record DateRangeFilterResource(
        DateTime? FromDate,
        DateTime? ToDate
    ) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            bool receiveRangeValueButNotTheOther = (FromDate == null && ToDate != null) ||
                (FromDate != null && ToDate == null);

            bool fromDataGreateThanToDate = FromDate != null && ToDate != null && FromDate > ToDate;

            if (receiveRangeValueButNotTheOther)
                yield return new ValidationResult(
                    "Both date range values should be provided"
                    );

            if (fromDataGreateThanToDate)
                yield return new ValidationResult(
                    "FromDate cannot be greater than ToDate"
                );
        }
    }
}
