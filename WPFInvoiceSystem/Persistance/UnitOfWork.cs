using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain;
using WPFInvoiceSystem.Domain.Entities;
using WPFInvoiceSystem.Domain.Repositories;

namespace WPFInvoiceSystem.Persistance.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public AppDbContext Connection { get; private set; }
        public IBanksRepository BanksRepository { get; private set; }
        public ICustomersRepository CustomersRepository { get; private set; }
        public IInvoicesRepository InvoicesRepository { get; private set; }
        public IPaymentsRepository PaymentsRepository { get; private set; }
        public IPaymentMethodsRepository PaymentMethodsRepository { get; private set; }
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
                { typeof(Bank), BanksRepository},
                { typeof(Customer), CustomersRepository },
                { typeof(Invoice), InvoicesRepository },
                { typeof(Payment), PaymentsRepository },
                { typeof(PaymentMethod), PaymentMethodsRepository },
                { typeof(Service), ServicesRepository },
                { typeof(ServiceType), ServiceTypesRepository },
            };

            return (IBaseRepository<ItemsType>)repositorioesMap[typeof(ItemsType)];
        }

        private void Initialize()
        {
            Connection = new AppDbContext();
            BanksRepository = new BanksRepository(Connection);
            CustomersRepository = new CustomersRepository(Connection);
            InvoicesRepository = new InvoicesRepository(Connection);
            PaymentsRepository = new PaymentsRepository(Connection);
            PaymentMethodsRepository = new PaymentMethodsRepository(Connection);
            ServicesRepository = new ServicesRepository(Connection);
            ServiceTypesRepository = new ServiceTypesRepository(Connection);
        }
    }
}
