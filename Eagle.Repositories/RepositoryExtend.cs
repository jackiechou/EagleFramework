using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using Eagle.Entities;
using Eagle.EntityFramework;

namespace Eagle.Repositories
{
   public abstract class RepositoryExtend<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
    {
        public DateTime NullDateTime = DateTime.Parse("1/1/1900");

        protected RepositoryExtend(IDataContext context)
        {
            DataContext = context;
            InstanceId = Guid.NewGuid();

            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            DateTimeFormatInfo dateformat = new DateTimeFormatInfo
            {
                ShortDatePattern = "MM/dd/yyyy"
            };
            culture.DateTimeFormat = dateformat;
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return DataContext.Get<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = DataContext.Get<TEntity>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query;
        }
        public virtual IEnumerable<TEntity> FindByIds(IEnumerable<TKey> ids, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = DataContext.Get<TEntity>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.Where(i => ids.Contains(i.Id));
        }
        public virtual IEnumerable<TEntity> GetByIds(IEnumerable<TKey> ids, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = DataContext.Get<TEntity>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.Where(i => ids.Contains(i.Id));
        }

        public TEntity GetById(TKey id, params Expression<Func<TEntity, object>>[] includes)
        {
            return GetByIds(new[] { id }, includes).FirstOrDefault();
        }
        public TEntity FindById(TKey id, params Expression<Func<TEntity, object>>[] includes)
        {
            return FindByIds(new[] { id }, includes).FirstOrDefault();
        }
        public IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = DataContext.Get<TEntity>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.Where(predicate).AsEnumerable();
        }

        public void Insert(TEntity entity)
        {
            DataContext.Insert(entity);
        }

        public void Insert(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                DataContext.Insert(entity);
            }
        }

        public void Update(TEntity entity)
        {
            DataContext.Update(entity);
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                DataContext.Update(entity);
            }
        }

        public void Update(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            DataContext.Set<TEntity>().Attach(entity);
            DbEntityEntry<TEntity> entry = DataContext.Entry(entity);
            foreach (var selector in properties)
            {
                entry.Property(selector).IsModified = true;
            }
        }
        public virtual void Delete(TKey id)
        {
            DataContext.Delete<TEntity>(id);
        }

        public void Delete(IEnumerable<TKey> ids)
        {
            foreach (var id in ids)
            {
                Delete(id);
            }
        }

        public virtual void Delete(TEntity entity)
        {
            DataContext.Delete<TEntity>(entity);
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        protected ClaimsPrincipal CurrentClaimsIdentity { get; private set; }
        public void SetIdentity(ClaimsPrincipal identity)
        {
            CurrentClaimsIdentity = identity;
        }

        /// <summary>
        /// Gets or sets the platform context.
        /// </summary>
        /// <value>
        /// The platform context.
        /// </value>
        /// <remarks>
        /// This property should be visible to UnitOfWork to enable cross PlatformContexts transaction management.
        /// </remarks>
        protected internal IDataContext DataContext { get; set; }

        private Guid InstanceId { get; set; }

    }
}
