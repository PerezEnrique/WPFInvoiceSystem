using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Entities;
using WPFInvoiceSystem.Domain.Repositories;

namespace WPFInvoiceSystem.Persistance.Repositories
{
    public class PaymentsRepository : BaseRepository<Payment>, IPaymentsRepository
    {
        public PaymentsRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Payment?> GetWithBankAndPaymentMethod(int id)
        {
            return await _connection.Payments
                .Include(p => p.Bank)
                .Include(p => p.PaymentMethod)
                .SingleOrDefaultAsync(p => p.Id == id);
        }
    }
}
