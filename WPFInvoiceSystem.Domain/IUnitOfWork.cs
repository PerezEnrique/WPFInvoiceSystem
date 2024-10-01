using WPFInvoiceSystem.Domain.Repositories;

namespace WPFInvoiceSystem.Domain
{
    public interface IUnitOfWork
    {
        ICustomersRepository CustomersRepository { get; }
        IInvoicesRepository InvoicesRepository { get; }
        IServicesRepository ServicesRepository { get; }
        IServiceTypesRepository ServiceTypesRepository { get; }
        Task CompleteAsync();
    }
}
