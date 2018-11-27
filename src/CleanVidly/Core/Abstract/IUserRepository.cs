using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CleanVidly.Core.Entities;

namespace CleanVidly.Core.Abstract
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetWithRoles(Expression<Func<User, bool>> predicate);
    }
}