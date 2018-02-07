using System.Collections.Generic;
using System.Linq;

namespace Eagle.Core.Pagination
{
    public static class PagingExtensions
    {
        #region IQueryable<T> extensions
     
    
        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> source, int pageIndex, int pageSize)
        {
            return new PagedList<T>(source, pageIndex, pageSize);
        }

        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> source, int pageIndex, int pageSize, int? totalCount)
        {
            return new PagedList<T>(source, pageIndex, pageSize, totalCount);
        }
        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> source, int pageIndex, int pageSize, int? totalCount, string sort)
        {
            return new PagedList<T>(source, pageIndex, pageSize, totalCount, sort);
        }
        #endregion

        #region IEnumerable<T> extensions
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
        {
            return new PagedList<T>(source, pageIndex, pageSize);
        }
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int pageIndex, int pageSize, int? totalCount)
        {
            return new PagedList<T>(source, pageIndex, pageSize, totalCount);
        }

        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int pageIndex, int pageSize, int? totalCount, string sortColumnName, string sort)
        {
            return new PagedList<T>(source, pageIndex, pageSize, totalCount, sortColumnName, sort);
        }
      
        #endregion
    }
}