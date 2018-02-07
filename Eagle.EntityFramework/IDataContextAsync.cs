using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eagle.EntityFramework
{
    public interface IDataContextAsync : IDisposable
    {
        int SaveChanges();
        void SyncObjectState(object entity);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync();
    }
}
