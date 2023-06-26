using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Repositories;

namespace WPFInvoiceSystem.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomersRepository CustomersRepository { get; }
        IInvoicesRepository InvoicesRepository { get; }
        IServicesRepository ServicesRepository { get; }
        IServiceTypesRepository ServiceTypesRepository { get; }
        Task CompleteAsync();
        IBaseRepository<ItemsType> GetRepository<ItemsType>();
    }
}
