using WPFInvoiceSystem.Domain.Entities;
using WPFInvoiceSystem.Domain.Repositories;

namespace WPFInvoiceSystem.Persistance.Repositories
{
    public class ServicesRepository : RepositoryBase<Service>, IServicesRepository
    {
        public ServicesRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
