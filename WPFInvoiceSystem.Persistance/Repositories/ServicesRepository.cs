using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WPFInvoiceSystem.Domain.Entities;
using WPFInvoiceSystem.Domain.Repositories;

namespace WPFInvoiceSystem.Persistance.Repositories
{
    public class ServicesRepository : RepositoryBase<Service>, IServicesRepository
    {
        public ServicesRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<IEnumerable<Service>> FindAsync(Expression<Func<Service, bool>> predicate)
        {
            return await _dbContext.Services
                .Include(s => s.Type)
                .Where(predicate)
                .ToListAsync();
        }

        public override async Task<Service?> GetAsync(int id)
        {
            return await _dbContext.Services
                .Include(s => s.Type)
                .SingleOrDefaultAsync(s =>  s.Id == id);
        }

        public override async Task<IEnumerable<Service>> GetAllAsync()
        {
            return await _dbContext.Services
                .Include(s => s.Type)
                .ToListAsync();
        }
    }
}
