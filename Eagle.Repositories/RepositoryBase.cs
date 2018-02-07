using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using Eagle.Common.Extensions.DataGrid;
using Eagle.Core.Common;
using Eagle.Entities;
using Eagle.EntityFramework;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories
{
    public abstract class RepositoryBase<TEntity> : DbContext, IRepositoryBase<TEntity> where TEntity : class, IObjectState
    {
        public DateTime NullDateTime = DateTime.Parse("1/1/1900");
        public Guid InstanceId { get; }
        protected IDataContext DataContext;
        protected RepositoryBase(IDataContext dataContext)
        {
            InstanceId = Guid.NewGuid();
            DataContext = dataContext;

            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            DateTimeFormatInfo dateformat = new DateTimeFormatInfo
            {
                ShortDatePattern = "MM/dd/yyyy"
            };
            culture.DateTimeFormat = dateformat;
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
        }
       
        public virtual TEntity FindById(object id)
        {
            return DataContext.FindById<TEntity>(id);
        }
        public virtual TEntity Find(params object[] keyValues)
        {
            TEntity result = null;
            try
            {
                result = DataContext.FindById<TEntity>(keyValues);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return result;
        }
        public virtual IQueryable<TEntity> Get()
        {
            return DataContext.Get<TEntity>();
        }
        public virtual IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = DataContext.Get<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query;
        }
        public virtual IQueryable<TEntity> Get(
          Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
          string includeProperties = "", int? page = null,
        int? pageSize = null)
        {
            IQueryable<TEntity> query = DataContext.Get<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (page != null && pageSize != null)
                query = query.ApplyPaging(page.Value, pageSize.Value);
            return query;
        }
        public virtual IQueryable<TEntity> Get(out int recordCount,
          Expression<Func<TEntity, bool>> filter = null,
          string orderBy = null,
          List<Expression<Func<TEntity, object>>> includeProperties = null,
          int? page = null,
          int? pageSize = null)
        {
            IQueryable<TEntity> query = DataContext.Get<TEntity>();

            if (includeProperties != null)
                includeProperties.ForEach(i => query = query.Include(i));

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = query.OrderBy(orderBy);

            recordCount = query.Count();
            if (page != null && pageSize != null)
                query = query.ApplyPaging(page.Value, pageSize.Value);

            return query;
        }

        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            string orderBy = null,
            List<Expression<Func<TEntity, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = DataContext.Get<TEntity>();

            if (includeProperties != null)
                includeProperties.ForEach(i => query = query.Include(i));

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
            {
                query = query.OrderBy(orderBy);
            }
            if (page != null && pageSize != null)
                query = query.ApplyPaging(page.Value, pageSize.Value);

            return query;
        }
        public virtual IQueryable<TEntity> GetList(string strsql)
        {
            return DataContext.SelectQuery<TEntity>(strsql);
        }

        public virtual IQueryable<TEntity> SelectQuery(string query, params object[] args)
        {
            return DataContext.SelectQuery<TEntity>(query, args);
        }
        public IQueryable<TEntity> StoredProc(string storedProcedureName, params object[] args)
        {
            return DataContext.Get<TEntity>(storedProcedureName, args);
        }
        public virtual TEntity Insert(TEntity entity)
        {
            return DataContext.Insert(entity);
        }
        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Insert(entity);
            }
        }

        public virtual void Update(TEntity entity)
        {
            DataContext.Update(entity);
        }

        public virtual void Delete(object id)
        {
            DataContext.Delete<TEntity>(id);
        }

        public virtual void Delete(TEntity entity)
        {
            DataContext.Delete(entity);
        }
    }
}
