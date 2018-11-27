using System.Threading.Tasks;

namespace CleanVidly.Core.Abstract
{
    public interface IUnitOfWork
    {
        Task<int> SaveAsync();

    }
}