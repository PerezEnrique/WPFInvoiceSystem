using WPFInvoiceSystem.Application.Abstractions;
using WPFInvoiceSystem.Application.Dtos;
using WPFInvoiceSystem.Domain;
using WPFInvoiceSystem.Domain.Entities;

namespace WPFInvoiceSystem.Application.Services
{
    public class ReportsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInvoicesReportGenerator _invoicesReportGenerator;

        public ReportsService(
            IUnitOfWork unitOfWork,
            IInvoicesReportGenerator invoicesReportGenerator
            )
        {
            _unitOfWork = unitOfWork;
            _invoicesReportGenerator = invoicesReportGenerator;
        }

        public async Task<byte[]> GenerateInvoicesReport(DateRangeFilterDto filter)
        {
            filter.Validate();

            var invoiceFilterDto = new InvoicesFilterDto(
                filter.FromDate,
                filter.ToDate,
                CustomerId: null
                );

            IEnumerable<Invoice> invoices = await _unitOfWork.InvoicesRepository
                .FindAsync(invoiceFilterDto);

            return _invoicesReportGenerator.Generate(invoices);
        }
    }
}
