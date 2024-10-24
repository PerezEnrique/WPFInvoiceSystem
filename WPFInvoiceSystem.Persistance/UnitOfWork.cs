using WPFInvoiceSystem.Domain;
using WPFInvoiceSystem.Domain.Repositories;
using WPFInvoiceSystem.Persistance.Repositories;

namespace WPFInvoiceSystem.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        public ICustomersRepository CustomersRepository { get; }

        public IInvoicesRepository InvoicesRepository { get; }

        public IServicesRepository ServicesRepository { get; }

        public IServiceTypesRepository ServiceTypesRepository { get; }

        public UnitOfWork(AppDbContext appDbContext
            )
        {
            _dbContext = appDbContext;
            CustomersRepository = new CustomersRepository(_dbContext);
            InvoicesRepository = new InvoicesRepository(_dbContext);
            ServicesRepository = new ServicesRepository(_dbContext);
            ServiceTypesRepository = new ServiceTypesRepository(_dbContext);
        }

        public async Task CompleteAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
