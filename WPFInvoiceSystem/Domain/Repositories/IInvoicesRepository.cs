using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Modals;

namespace WPFInvoiceSystem.Domain.Repositories
{
    public interface IInvoicesRepository : IBaseRepository<Invoice>
    {
        Task<Invoice?> GetByInvoiceNumber(int invoiceNumber);
        Task<Invoice?> GetByInvoiceNumberWithRelatedData(int invoiceNumber);
        Task<Invoice?> GetWithRelatedData(int id);
        Task<IEnumerable<Invoice>> GetAllWithCustomerData();
    }
}
