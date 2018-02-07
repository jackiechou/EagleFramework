using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Eagle.Entities;

namespace Eagle.EntityFramework.Repositories
{
    public interface IRepositoryBase<TEntity> where TEntity : class, IObjectState
    {
        TEntity FindById(object id);
        TEntity Find(params object[] keyValues);
        IQueryable<TEntity> Get();
        IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", int? page = null,
            int? pageSize = null);
        IQueryable<TEntity> Get(
          out int recordCount,
          Expression<Func<TEntity, bool>> filter = null,
          string orderBy = null,
          List<Expression<Func<TEntity, object>>> includeProperties = null,
          int? page = null,
          int? pageSize = null);
        IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            string orderBy = null,
            List<Expression<Func<TEntity, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null);

        IQueryable<TEntity> GetList(string strsql);
        IQueryable<TEntity> SelectQuery(string query, params object[] parameters);
        IQueryable<TEntity> StoredProc(string storedProcedureName, params object[] args);
        TEntity Insert(TEntity entity);
        void InsertRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Delete(object id);
        //IQueryFluent<TEntity> Query();
        //IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject);
        //IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query);
        //IQueryable Queryable(ODataQueryOptions<TEntity> oDataQueryOptions);
        //IQueryable<TEntity> Queryable();
        //IRepositoryBase<T> GetRepository<T>() where T : class, IObjectState;

        //Task<TEntity> FindAsync(params object[] keyValues);
        //Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues);
        //Task<bool> DeleteAsync(params object[] keyValues);
        //Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues);
    }
}