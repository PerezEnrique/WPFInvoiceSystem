using WPFInvoiceSystem.Domain.Entities;

namespace WPFInvoiceSystem.Application.Abstractions
{
    public interface IInvoicesReportGenerator
    {
        byte[] Generate(IEnumerable<Invoice> invoices);
    }
}
