using WPFInvoiceSystem.Domain.Entities;

namespace WPFInvoiceSystem.Domain.Repositories
{
    public interface ICustomersRepository : IRepository<Customer>
    {
        public Task<Customer?> GetByIdentityCard(int identityCard);
    }
}
