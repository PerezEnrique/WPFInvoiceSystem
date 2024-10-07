using System.ComponentModel.DataAnnotations;

namespace WPFInvoiceSystem.API.ApiResources
{
    public record InvoicesFilterResource(
        DateTime? FromDate,
        DateTime? ToDate,
        int? CustomerId
        ) : IValidatableObject
    {

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            bool noQueryCriterionProvided = FromDate == null &&
                ToDate == null &&
                CustomerId == null;

            bool receiveRangeValueButNotTheOther = (FromDate == null && ToDate != null) ||
                (FromDate != null && ToDate == null);

            bool fromDataGreateThanToDate = FromDate != null && ToDate != null && FromDate > ToDate;

            if (noQueryCriterionProvided)
                yield return new ValidationResult(
                   "No filtering criteria was provided"
                   );

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
