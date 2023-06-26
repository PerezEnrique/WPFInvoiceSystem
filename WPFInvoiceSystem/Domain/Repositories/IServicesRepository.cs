using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Entities;

namespace WPFInvoiceSystem.Domain.Repositories
{
    public interface IServicesRepository : IBaseRepository<Service>
    {
        public Task<Service?> FindSingleOrDefault(Expression<Func<Service, bool>> predicate);
    }
}
