using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Entities;
using WPFInvoiceSystem.Domain.Repositories;

namespace WPFInvoiceSystem.Persistance.Repositories
{
    public class PaymentMethodsRepository : BaseRepository<PaymentMethod>, IPaymentMethodsRepository
    {
        public PaymentMethodsRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
