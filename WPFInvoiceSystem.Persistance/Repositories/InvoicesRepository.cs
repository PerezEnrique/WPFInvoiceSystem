using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WPFInvoiceSystem.Application.Dtos;
using WPFInvoiceSystem.Domain.Entities;
using WPFInvoiceSystem.Domain.Repositories;

namespace WPFInvoiceSystem.Persistance.Repositories
{
    public class InvoicesRepository : RepositoryBase<Invoice>, IInvoicesRepository
    {
        public InvoicesRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<IEnumerable<Invoice>> FindAsync(Expression<Func<Invoice, bool>> predicate)
        {
            return await _dbContext.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Services)
                    .ThenInclude(s => s.Service)
                        .ThenInclude(s => s.Type)
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Invoice>> FindAsync(object filter)
        {
            IQueryable<Invoice> query = _dbContext.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Services)
                    .ThenInclude(s => s.Service)
                        .ThenInclude(s => s.Type)
                .AsQueryable();

            InvoicesFilterDto invoiceFilter = (InvoicesFilterDto)filter;

            if (invoiceFilter.FromDate != null && invoiceFilter.ToDate != null)
            {
                query = query.Where((invoice) => invoice.Date >= invoiceFilter.FromDate &&
                invoice.Date <= invoiceFilter.ToDate);
            }

            if (invoiceFilter.CustomerId != null)
            {
                query = query.Where((invoice) => invoice.Customer.Id == invoiceFilter.CustomerId);
            }

            return await query.ToListAsync();
        }

        public override async Task<Invoice?> GetAsync(int id)
        {
            return await _dbContext.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Services)
                    .ThenInclude(s => s.Service)
                        .ThenInclude(s => s.Type)
                .SingleOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Invoice>> GetLastTenInvoices()
        {
            return await _dbContext.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Services)
                    .ThenInclude(s => s.Service)
                        .ThenInclude(s => s.Type)
                .OrderBy(i => i.Date)
                .Take(10)
                .ToListAsync();
        }
    }
}
