using System.Threading;
using System.Threading.Tasks;
using Eagle.Entities;

namespace Eagle.EntityFramework.Repositories
{
    public interface IUnitOfWorkAsync 
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : class, IObjectState;
    }
}