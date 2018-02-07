using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Eagle.Entities;
using Eagle.EntityFramework;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories
{
    public sealed class QueryFluent<TEntity> : IQueryFluent<TEntity> where TEntity :class ,IObjectState
    {
        #region Private Fields
        private readonly Expression<Func<TEntity, bool>> _expression;
        private readonly List<Expression<Func<TEntity, object>>> _includes;
        private readonly RepositoryAsync<TEntity> _repositoryAsync;
        private Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> _orderBy;
        #endregion Private Fields

        #region Constructors
        public QueryFluent(RepositoryAsync<TEntity> repositoryAsync)
        {
            _repositoryAsync = repositoryAsync;
            _includes = new List<Expression<Func<TEntity, object>>>();
        }

        public QueryFluent(RepositoryAsync<TEntity> repositoryAsync, IQueryObject<TEntity> queryObject) : this(repositoryAsync) { _expression = queryObject.Query(); }

        public QueryFluent(RepositoryAsync<TEntity> repositoryAsync, Expression<Func<TEntity, bool>> expression) : this(repositoryAsync) { _expression = expression; }
        #endregion Constructors

        public IQueryFluent<TEntity> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            _orderBy = orderBy;
            return this;
        }

        public IQueryFluent<TEntity> Include(Expression<Func<TEntity, object>> expression)
        {
            _includes.Add(expression);
            return this;
        }

        public IEnumerable<TEntity> SelectPage(int page, int pageSize, out int totalCount)
        {
            totalCount = _repositoryAsync.Select(_expression).Count();
            return _repositoryAsync.Select(_expression, _orderBy, _includes, page, pageSize);
        }

        public IEnumerable<TEntity> Select() { return _repositoryAsync.Select(_expression, _orderBy, _includes); }

        public IEnumerable<TResult> Select<TResult>(Expression<Func<TEntity, TResult>> selector) { return _repositoryAsync.Select(_expression, _orderBy, _includes).Select(selector); }

        public async Task<IEnumerable<TEntity>> SelectAsync() { return await _repositoryAsync.SelectAsync(_expression, _orderBy, _includes); }

        public IQueryable<TEntity> SqlQuery(string query, params object[] parameters) { return _repositoryAsync.SelectQuery(query, parameters).AsQueryable(); }
    }
}