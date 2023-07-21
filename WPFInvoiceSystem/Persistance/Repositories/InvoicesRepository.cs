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
    public class InvoicesRepository : BaseRepository<Invoice>, IInvoicesRepository
    {
        public InvoicesRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Invoice?> GetByInvoiceNumber(int invoiceNumber)
        {
            return await _connection.Invoices.SingleOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber);
        }

        public async Task<Invoice?> GetByInvoiceNumberWithRelatedData(int invoiceNumber)
        {
            return await _connection.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Services)
                    .ThenInclude(s => s.Service)
                        .ThenInclude(s => s.Type)
                .SingleOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber);
        }

        public async Task<IEnumerable<Invoice>> GetAllWithCustomerData()
        {
            return await _connection.Invoices
                .Include(i => i.Customer)
                .ToListAsync();
        }
        public async Task<Invoice?> GetWithRelatedData(int id)
        {
            return await _connection.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Services)
                    .ThenInclude(s => s.Service)
                        .ThenInclude(s => s.Type)
                .SingleOrDefaultAsync(i => i.Id == id);
        }
    }
}
