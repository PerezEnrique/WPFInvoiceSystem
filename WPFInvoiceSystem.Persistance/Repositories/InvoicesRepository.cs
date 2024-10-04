using WPFInvoiceSystem.Domain.Entities;
using WPFInvoiceSystem.Domain.Repositories;

namespace WPFInvoiceSystem.Persistance.Repositories
{
    public class InvoicesRepository : RepositoryBase<Invoice>, IInvoicesRepository
    {
        public InvoicesRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
