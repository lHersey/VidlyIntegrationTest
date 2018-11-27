using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CleanVidly.Core.Abstract;
using CleanVidly.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanVidly.Persistance.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly CleanVidlyDbContext context;

        public UserRepository(CleanVidlyDbContext context) : base(context)
        {
            this.context = context;
        }

        public Task<User> GetWithRoles(Expression<Func<User, bool>> predicate)
        {
            return context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(predicate);
        }
    }
}