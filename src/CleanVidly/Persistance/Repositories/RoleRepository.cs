using CleanVidly.Core.Abstract;
using CleanVidly.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanVidly.Persistance.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly CleanVidlyDbContext context;

        public RoleRepository(CleanVidlyDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}