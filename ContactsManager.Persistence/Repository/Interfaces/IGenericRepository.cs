using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Persistence.Repository.Interfaces
{
    public interface IGenericRepository<T, TKey> where T : class
        where TKey : IEquatable<TKey>
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetbyIdAsync(TKey Id);
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        T Update(T entity);
        IEnumerable<T> UpdateRange(IEnumerable<T> entities);
        Task<T> DeleteAsync(TKey id);
        IEnumerable<T> DeleteRange(IEnumerable<T> entities);
        Task<List<T>> GetAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
    }
}
