using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CleanVidly.Core.Abstract
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetFindAllAsync();
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task<List<T>> FindByAsync(Expression<Func<T, bool>> predicate);
        Task<T> FindUniqueAsync(Expression<Func<T, bool>> predicate);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        Task<bool> ExistAsync(Expression<Func<T, bool>> predicate);
    }
}