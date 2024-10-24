using WPFInvoiceSystem.Domain.Exceptions;

namespace WPFInvoiceSystem.Application.Dtos
{
    public record InvoicesFilterDto(
        DateTime? FromDate,
        DateTime? ToDate,
        int? CustomerId
        )
    {
        public void Validate()
        {
            bool noQueryCriterionProvided = FromDate == null &&
                ToDate == null &&
                CustomerId == null;

            if (noQueryCriterionProvided)
                throw new CoreValidationException("No filtering criteria was provided");

            if ((FromDate == null && ToDate != null) ||
                (FromDate != null && ToDate == null))
            {
                throw new CoreValidationException("Both date range values should be provided");
            }

            if (FromDate != null && ToDate != null && FromDate > ToDate)
            {
                throw new CoreValidationException(
                    "FromDate cannot be greater than ToDate"
                );
            }
        }
    }
}
