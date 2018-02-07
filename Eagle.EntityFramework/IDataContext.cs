using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eagle.EntityFramework
{
    public interface IDataContext : IDisposable
    {
        TEntity FindById<TEntity>(params object[] ids) where TEntity : class;
        IQueryable<TEntity> Get<TEntity>() where TEntity : class;
        IQueryable<TEntity> Get<TEntity>(string storedProcedureName, params object[] args) where TEntity : class;
        IQueryable<TEntity> SelectQuery<TEntity>(string query, params object[] args) where TEntity : class;
        TEntity Insert<TEntity>(TEntity entity) where TEntity : class;
        //void InsertGraph<TEntity>(TEntity entity) where TEntity : class;
        void Update<TEntity>(TEntity entity) where TEntity : class;

        void Delete<TEntity>(TEntity entity) where TEntity : class;
        void Delete<TEntity>(params object[] ids) where TEntity : class;
        /// <summary>
        /// Execute custom SQL command or stord procedure returning number of rows returned.
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <returns></returns>
        int Execute(string sqlCommand);

        /// <summary>
        /// Execute custom SQL command or stord procedure returning number of rows returned.
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <returns></returns>
        int Execute(string sqlCommand, params object[] args);
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        void SyncObjectsStatePostCommit();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        void CommitTransaction();
        void AbortTransaction();
    }
}
