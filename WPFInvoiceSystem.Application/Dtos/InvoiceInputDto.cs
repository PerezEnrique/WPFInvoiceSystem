namespace WPFInvoiceSystem.Application.Dtos
{
    public record InvoiceInputDto(
        DateTime Date,
        int InvoiceNumber,
        int CustomerId,
        IEnumerable<InvoiceServiceInputDto> InvoiceServices
        );
}
