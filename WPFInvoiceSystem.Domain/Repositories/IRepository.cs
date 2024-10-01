using System.Linq.Expressions;

namespace WPFInvoiceSystem.Domain.Repositories
{
    public interface IRepository<T>
    {
        void Add(T item);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAsync(object filter);
        Task<T?> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        void Remove(T item);
    }
}
