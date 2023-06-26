using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Entities;
using WPFInvoiceSystem.Domain.Repositories;

namespace WPFInvoiceSystem.Persistance.Repositories
{
    public class ServicesRepository : BaseRepository<Service>, IServicesRepository
    {
        public ServicesRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Service?> FindSingleOrDefault(Expression<Func<Service, bool>> predicate)
        {
            return await _connection.Services.SingleOrDefaultAsync(predicate);
        }
    }
}
