using WPFInvoiceSystem.Domain.Exceptions;

namespace WPFInvoiceSystem.Application.Dtos
{
    public record DateRangeFilterDto
    (
        DateTime? FromDate,
        DateTime? ToDate
    )
    {
        public void Validate()
        {
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
