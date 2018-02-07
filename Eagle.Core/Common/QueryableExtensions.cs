using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;

namespace Eagle.Core.Common
{
    // ToDo: ptn, this class can be improved
    //      Paging and record count can be simplified
    //      Also this class should be moved inside the Repository Implementation Project as an internal set of extensions.

    /// <summary>
    /// http://stackoverflow.com/questions/41244/dynamic-linq-orderby-on-ienumerablet
    /// http://aonnull.blogspot.com.au/2010/08/dynamic-sql-like-linq-orderby-extension.html
    /// </summary>
    public static class QueryableExtensions
    {
        public static IEnumerable<T> SortBy<T>(this IEnumerable<T> enumerable, string orderBy)
        {
            if (!string.IsNullOrEmpty(orderBy))
                return enumerable.AsQueryable().SortBy(orderBy).AsEnumerable();
            return null;
        }

        public static IOrderedQueryable<T> SortBy<T>(this IQueryable<T> collection, string orderBy)
        {
            //if (!string.IsNullOrEmpty(orderBy))
            //{
                foreach (OrderByInfo orderByInfo in ParseOrderBy(orderBy))
                    collection = ApplyOrderBy(collection, orderByInfo);

                return (IOrderedQueryable<T>)collection;
            //}
            //return null;
        }

        private static IOrderedQueryable<T> ApplyOrderBy<T>(IQueryable<T> collection, OrderByInfo orderByInfo)
        {
            string[] props = orderByInfo.PropertyName.Split('.');
            Type type = typeof(T);

            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);
            string methodName = String.Empty;

            if (!orderByInfo.Initial && collection is IOrderedQueryable<T>)
            {
                if (orderByInfo.Direction == SortDirection.Ascending)
                    methodName = "ThenBy";
                else
                    methodName = "ThenByDescending";
            }
            else
            {
                if (orderByInfo.Direction == SortDirection.Ascending)
                    methodName = "OrderBy";
                else
                    methodName = "OrderByDescending";
            }

            object result = typeof(Queryable).GetMethods().Single(
                method => method.Name == methodName
                        && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 2
                        && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), type)
                .Invoke(null, new object[] { collection, lambda });
            return (IOrderedQueryable<T>)result;

        }

        private static IEnumerable<OrderByInfo> ParseOrderBy(string orderBy)
        {
            if (String.IsNullOrEmpty(orderBy))
                yield break;

            string[] items = orderBy.Split(',');
            bool initial = true;
            foreach (string item in items)
            {
                string[] pair = item.Trim().Split(' ');

                if (pair.Length > 2)
                    throw new ArgumentException(String.Format("Invalid OrderBy string '{0}'. Order By Format: Property, Property2 ASC, Property2 DESC", item));

                string prop = pair[0].Trim();

                if (String.IsNullOrEmpty(prop))
                    throw new ArgumentException("Invalid Property. Order By Format: Property,Property2 ASC, Property2 DESC");

                var dir = SortDirection.Ascending;

                if (pair.Length == 2)
                    dir = ("desc".Equals(pair[1].Trim(), StringComparison.OrdinalIgnoreCase) ? SortDirection.Descending : SortDirection.Ascending);

                yield return new OrderByInfo { PropertyName = prop, Direction = dir, Initial = initial };

                initial = false;
            }

        }

        private class OrderByInfo
        {
            public string PropertyName { get; set; }
            public SortDirection Direction { get; set; }
            public bool Initial { get; set; }
        }

        private enum SortDirection
        {
            Ascending = 0,
            Descending = 1
        }
        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> collection, int page, int pageSize, out int recordCount)
        {
            recordCount = collection.Count();
            collection = collection.Skip((page > 0 ? page - 1 : 0) * pageSize).Take(pageSize);
            return collection;
        }
        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> collection, int page, int pageSize)
        {
            collection = collection.Skip((page > 0 ? page - 1 : 0) * pageSize).Take(pageSize);
            return collection;
        }

        public static IEnumerable<T> ApplyPaging<T>(this IEnumerable<T> collection, int page, int pageSize, out int recordCount)
        {
            recordCount = collection.Count();
            collection = collection.Skip((page > 0 ? page - 1 : 0) * pageSize).Take(pageSize);
            return collection;
        }
        public static IEnumerable<T> ApplyPaging<T>(this IEnumerable<T> collection, int page, int pageSize)
        {
            collection = collection.Skip((page > 0 ? page - 1 : 0) * pageSize).Take(pageSize);
            return collection;
        }

        public static IEnumerable<TResult> WithSortingAndPaging<TResult>(this IEnumerable<TResult> source, string ordering, int? page, int? pageSize)
        {
            var result = source;

            if (!string.IsNullOrEmpty(ordering))
            {
                result = result.WithSorting(ordering);
            }

            if (page != null && pageSize != null)
            {
                result = result.WithPaging(page.Value, pageSize.Value);
            }

            return result;
        }

        #region Order

        /// <summary>
        /// Applies sorting on the query.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="ordering">The ordering. Examples: "Title DESC" or "Title,Description DESC,Date ASC". Defaults to ASC if not order defined.</param>
        /// <returns></returns>
        public static IEnumerable<TResult> WithSorting<TResult>(this IEnumerable<TResult> source, string ordering)
        {
            return source.SortBy(ordering);
        }
        #endregion
        #region RecordCount

        public static IEnumerable<TResult> WithRecordCount<TResult>(this IEnumerable<TResult> source, ref int? recordCount)
        {
            recordCount = source.Count();
            return source;
        }
        public static IEnumerable<TResult> WithRecordCount<TResult>(this IEnumerable<TResult> source, out int recordCount)
        {
            recordCount = source.Count();
            return source;
        }
        #endregion
        #region Paging
       
        /// <summary>
        /// Applies paging on the query.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public static IQueryable<TResult> WithPaging<TResult>(this IQueryable<TResult> source, int page, int pageSize)
        {
            var result = source
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);

            return result;
        }
        public static IQueryable<TResult> WithPaging<TResult>(this IQueryable<TResult> source, int? page, int? pageSize)
        {
            var result = source;

            if (page.HasValue && pageSize.HasValue)
                result = source.WithPaging(page.Value, pageSize.Value);

            return result;
        }

        /// <summary>
        /// Applies paging on the query.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public static IEnumerable<TResult> WithPaging<TResult>(this IEnumerable<TResult> source, int page, int pageSize)
        {
            var result = source
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);

            return result;
        }
        #endregion

        #region Claim
        //claim values = "abc"
        public static IQueryable<TData> ApplySingleValueClaim<TData>(this IQueryable<TData> entites, string propertyName, Claim claim)
        {
            if (claim != null)
            {
                ParameterExpression pe = Expression.Parameter(typeof(TData), "item");
                // ***** Build Where clause  ***** 
                Expression predicateBody = null;
                Expression left = Expression.Property(pe, typeof(TData).GetProperty(propertyName));
                Type type = typeof(TData).GetProperty(propertyName).PropertyType;
                Expression right = Expression.Constant(Convert.ChangeType(claim.Value, type));
                Expression e1 = Expression.Equal(left, right);
                predicateBody = e1;


                MethodCallExpression whereCallExpression = Expression.Call(typeof(Queryable), "Where",
                    new Type[] { typeof(TData) },
                    entites.Expression,
                    Expression.Lambda<Func<TData, bool>>(predicateBody, new ParameterExpression[] { pe }));
                entites = entites.Provider.CreateQuery<TData>(whereCallExpression);
            }
            return entites;
        }

        //claim values = "3,4,5,7,7"
        public static IQueryable<TData> ApplyMultiValueClaim<TData>(this IQueryable<TData> entites, string propertyName, Claim claim)
        {
            if (claim != null)
            {
                var arrayItems = claim.Value.Split(',');
                ParameterExpression pe = Expression.Parameter(typeof(TData), "item");
                // ***** Build Where clause  ***** 
                Expression expressionBody = null;
                Expression left = Expression.Property(pe, typeof(TData).GetProperty(propertyName));

                foreach (var item in arrayItems)
                {
                    Type type = typeof(TData).GetProperty(propertyName).PropertyType;
                    Expression right = Expression.Constant(Convert.ChangeType(item, type));
                    Expression e = Expression.Equal(left, right);
                    expressionBody = expressionBody != null ? Expression.Or(expressionBody, e) : e;
                }

                if (expressionBody != null)
                {
                    MethodCallExpression whereCallExpression = Expression.Call(typeof(Queryable), "Where",
                        new Type[] { typeof(TData) },
                        entites.Expression,
                        Expression.Lambda<Func<TData, bool>>(expressionBody, new ParameterExpression[] { pe }));
                    entites = entites.Provider.CreateQuery<TData>(whereCallExpression);
                }
            }
            return entites;
        }

        #endregion
    }
}
