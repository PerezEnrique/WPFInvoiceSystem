using WPFInvoiceSystem.Domain.Exceptions;

namespace WPFInvoiceSystem.Application.Dtos
{
    public record DateRangeFilterDto
    (
        DateTime FromDate,
        DateTime ToDate
    )
    {
        public void Validate()
        {

            if (FromDate > ToDate)
            {
                throw new CoreValidationException(
                    "FromDate cannot be greater than ToDate"
                );
            }
        }
    }
}
