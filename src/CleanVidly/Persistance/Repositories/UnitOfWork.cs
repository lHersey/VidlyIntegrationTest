using System.Threading.Tasks;
using CleanVidly.Core.Abstract;

namespace CleanVidly.Persistance.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CleanVidlyDbContext context;

        public UnitOfWork(CleanVidlyDbContext context)
        {
            this.context = context;
        }
        public Task<int> SaveAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}