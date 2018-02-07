using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using Eagle.Entities;
using Eagle.EntityFramework;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories
{
    public class UnitOfWorkAsync : IUnitOfWorkAsync
    {
        #region Private Fields
        private readonly IDataContextAsync _dataContext;
        private bool _disposed;
        private ObjectContext _objectContext;
        private Dictionary<string, object> _repositories;
        private DbTransaction _transaction;
        #endregion Private Fields

        #region Constuctor/Dispose
        public UnitOfWorkAsync(IDataContextAsync dataContext) { _dataContext = dataContext; }

        public void Dispose()
        {
            if (_objectContext != null && _objectContext.Connection.State == ConnectionState.Open)
            {
                _objectContext.Connection.Close();
            }

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _dataContext.Dispose();
            }
            _disposed = true;
        }
        #endregion Constuctor/Dispose

        public int SaveChanges() { return _dataContext.SaveChanges(); }

        public Task<int> SaveChangesAsync() { return _dataContext.SaveChangesAsync(); }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken) { return _dataContext.SaveChangesAsync(cancellationToken); }

        public IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : class, IObjectState
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<string, object>();
            }

            var type = typeof(TEntity).Name;

            if (_repositories.ContainsKey(type))
            {
                return (IRepositoryAsync<TEntity>)_repositories[type];
            }

            var repositoryType = typeof(IRepositoryAsync<>);

            _repositories.Add(type, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _dataContext, this));
            return (IRepositoryAsync<TEntity>)_repositories[type];
        }

        #region Unit of Work Transactions
        //IF 04/09/2014 Add IsolationLevel
        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            _objectContext = ((IObjectContextAdapter)_dataContext).ObjectContext;
            if (_objectContext.Connection.State != ConnectionState.Open)
            {
                _objectContext.Connection.Open();
            }

            _transaction = _objectContext.Connection.BeginTransaction();
        }

        public bool Commit()
        {
            _transaction.Commit();
            return true;
        }

        public void Rollback()
        {
            _transaction.Rollback();
            ((DataContextAsync)_dataContext).SyncObjectsStatePostCommit();
        }
        #endregion

        // Uncomment, if rather have IRepositoryAsync<TEntity> IoC vs. Reflection Activation
        //public IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : EntityBase
        //{
        //    return ServiceLocator.Current.GetInstance<IRepositoryAsync<TEntity>>();
        //}
    }
}
