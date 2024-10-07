using WPFInvoiceSystem.Domain.Entities;

namespace WPFInvoiceSystem.Domain.Repositories
{
    public interface IInvoicesRepository : IRepository<Invoice>
    {
        Task<IEnumerable<Invoice>> FindAsync(object filter);
        public Task<IEnumerable<Invoice>> GetLastTenInvoices();
    }
}
