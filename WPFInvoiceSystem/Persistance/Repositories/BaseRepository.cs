using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Repositories;

namespace WPFInvoiceSystem.Persistance.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext _connection;

        public BaseRepository(AppDbContext dbContext)
        {
            _connection = dbContext;
        }
        public void Add(T item)
        {
            _connection.Set<T>().Add(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            _connection.Set<T>().AddRange(items);
        }

        public virtual async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await _connection.Set<T>().Where(predicate).ToListAsync();
        }

        public virtual async Task<T?> Get(int id)
        {
            return await _connection.Set<T>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _connection.Set<T>().ToListAsync();
        }

        public void Remove(T item)
        {
            _connection.Set<T>().Remove(item);
        }
    }
}
