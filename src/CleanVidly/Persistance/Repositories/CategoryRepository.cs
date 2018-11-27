using CleanVidly.Core.Abstract;
using CleanVidly.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanVidly.Persistance.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly CleanVidlyDbContext context;

        public CategoryRepository(CleanVidlyDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}