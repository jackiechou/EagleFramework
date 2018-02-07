using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using Eagle.Entities;

namespace Eagle.EntityFramework
{
    public class DataContextAsync : DbContext, IDataContextAsync
    {
        #region Private Fields

        #endregion Private Fields

        public DataContextAsync(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            InstanceId = Guid.NewGuid();
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public Guid InstanceId { get; }

        public override int SaveChanges()
        {
            SyncObjectsStatePreCommit();
            var changes = base.SaveChanges();
            SyncObjectsStatePostCommit();
            return changes;
        }

        public override async Task<int> SaveChangesAsync()
        {
            SyncObjectsStatePreCommit();
            var changesAsync = await base.SaveChangesAsync();
            SyncObjectsStatePostCommit();
            return changesAsync;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            SyncObjectsStatePreCommit();
            var changesAsync = await base.SaveChangesAsync(cancellationToken);
            SyncObjectsStatePostCommit();
            return changesAsync;
        }

        public void SyncObjectState(object entity) { Entry(entity).State = StateHelper.ConvertState(((IObjectState)entity).State); }
        public new DbSet<T> Set<T>() where T : class { return base.Set<T>(); }

        private void SyncObjectsStatePreCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
            {
                dbEntityEntry.State = StateHelper.ConvertState(((IObjectState)dbEntityEntry.Entity).State);
            }
        }

        public void SyncObjectsStatePostCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
            {
                ((IObjectState)dbEntityEntry.Entity).State = StateHelper.ConvertState(dbEntityEntry.State);
            }
        }
    }
}
