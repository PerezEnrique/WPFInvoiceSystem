using WPFInvoiceSystem.Domain.Entities;
using WPFInvoiceSystem.Domain.Repositories;

namespace WPFInvoiceSystem.Persistance.Repositories
{
    public class ServiceTypesRepository : RepositoryBase<ServiceType>, IServiceTypesRepository
    {
        public ServiceTypesRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
