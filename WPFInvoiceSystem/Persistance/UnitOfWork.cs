using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain;
using WPFInvoiceSystem.Domain.Modals;
using WPFInvoiceSystem.Domain.Repositories;
using WPFInvoiceSystem.Persistance.Repositories;

namespace WPFInvoiceSystem.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        public AppDbContext Connection { get; private set; }
        public ICustomersRepository CustomersRepository { get; private set; }
        public IInvoicesRepository InvoicesRepository { get; private set; }
        public IServicesRepository ServicesRepository { get; private set; }
        public IServiceTypesRepository ServiceTypesRepository { get; private set; }

        public UnitOfWork()
        {
            Initialize();
        }

        public async Task CompleteAsync()
        {
            await Connection.SaveChangesAsync();
        }

        public void Dispose()
        {
            Connection.Dispose();
            Initialize();
        }

        public IBaseRepository<ItemsType> GetRepository<ItemsType>()
        {
            var repositorioesMap = new Dictionary<Type, object>
            {
                { typeof(Customer), CustomersRepository },
                { typeof(Invoice), InvoicesRepository },
                { typeof(Service), ServicesRepository },
                { typeof(ServiceType), ServiceTypesRepository },
            };

            return (IBaseRepository<ItemsType>)repositorioesMap[typeof(ItemsType)];
        }

        private void Initialize()
        {
            Connection = new AppDbContext();
            CustomersRepository = new CustomersRepository(Connection);
            InvoicesRepository = new InvoicesRepository(Connection);
            ServicesRepository = new ServicesRepository(Connection);
            ServiceTypesRepository = new ServiceTypesRepository(Connection);
        }
    }
}
