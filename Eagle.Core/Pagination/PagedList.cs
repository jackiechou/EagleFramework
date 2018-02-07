using System;
using System.Collections.Generic;
using System.Linq;

namespace Eagle.Core.Pagination
{
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize) : this(source, pageIndex, pageSize, null){}
        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int? totalCount = null)
            : this(source.AsQueryable(), pageIndex, pageSize, totalCount)
        {
        }
        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int? totalCount, string sortColumnName, string sortExpression)
            : this(source.AsQueryable(), pageIndex, pageSize, totalCount, sortColumnName, sortExpression)
        {
        }
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize) : this(source, pageIndex, pageSize, null)
        {
        }
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize, int? totalCount) : this(source, pageIndex, pageSize, totalCount,null)
        {
        }
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize, int? totalCount = null, string sortColumnName=null, string sortExpression = null)
        {
            if (pageIndex < 0)
                throw new ArgumentOutOfRangeException("pageIndex", "Value can not be below 0.");
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException("pageSize", "Value can not be less than 1.");

            if (source == null)
                source = new List<T>().AsQueryable();

            var realTotalCount = source.Count();

            PageSize = pageSize;
            PageIndex = pageIndex;
            TotalItemCount = totalCount.HasValue ? totalCount.Value : realTotalCount;
            TotalPageCount = TotalItemCount > 0 ? (int)Math.Ceiling(TotalItemCount / (double)PageSize) : 0;

            HasPreviousPage = (PageIndex > 0);
            HasNextPage = (PageIndex < (TotalPageCount - 1));
            IsFirstPage = (PageIndex <= 0);
            IsLastPage = (PageIndex >= (TotalPageCount - 1));

            ItemStart = PageIndex * PageSize + 1;
            ItemEnd = Math.Min(PageIndex * PageSize + PageSize, TotalItemCount);

            FirstItemOnPage = (PageNumber - 1) * PageSize + 1;
            var numberOfLastItemOnPage = FirstItemOnPage + PageSize - 1;
            LastItemOnPage = numberOfLastItemOnPage > TotalItemCount ? TotalItemCount : numberOfLastItemOnPage;

            SortColumnName = sortColumnName;
            SortExpression = sortExpression;

            if (TotalItemCount <= 0)
                return;

            AddRange(realTotalCount != TotalItemCount ? source.Take(PageSize) : source.Skip(PageIndex*PageSize).Take(PageSize));
        }
      
        #region IPagedList Members

        public int TotalPageCount { get; private set; }
        public int TotalItemCount { get; private set; }
        public int PageIndex { get; private set; }
        public int PageNumber { get { return PageIndex + 1; } }
        public int PageSize { get; private set; }
        public bool HasPreviousPage { get; private set; }
        public bool HasNextPage { get; private set; }
        public bool IsFirstPage { get; private set; }
        public bool IsLastPage { get; private set; }
        public int ItemStart { get; private set; }
        public int ItemEnd { get; private set; }

        public int FirstItemOnPage { get; private set; }
        public int LastItemOnPage { get; private set; }

        public string SortColumnName { get; set; }
        public string SortExpression { get; set; }
        #endregion
    }
}