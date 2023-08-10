using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WPFInvoiceSystem.Domain.Repositories
{
    public interface IBaseRepository<T>
    {
        void Add(T item);
        void AddRange(IEnumerable<T> items);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
        Task<T?> Get(int id);
        Task<IEnumerable<T>> GetAll();
        void Remove(T item);
    }
}
