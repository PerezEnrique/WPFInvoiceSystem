using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Modals;
using WPFInvoiceSystem.Domain.Repositories;

namespace WPFInvoiceSystem.Persistance.Repositories
{
    public class CustomersRepository : BaseRepository<Customer>, ICustomersRepository
    {

        public CustomersRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Customer?> GetByIdentityCard(int identityCard)
        {
            return await _connection.Customers
                .SingleOrDefaultAsync(c => c.IdentityCard == identityCard);
        }
    }
}
