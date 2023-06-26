using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Entities;
using WPFInvoiceSystem.Domain.Repositories;

namespace WPFInvoiceSystem.Persistance.Repositories
{
    public class ServiceTypesRepository : BaseRepository<ServiceType>, IServiceTypesRepository
    {
        public ServiceTypesRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
