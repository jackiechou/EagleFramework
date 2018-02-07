using System;
using Eagle.Core.Configuration;

namespace Eagle.Core.Pagination
{
    public class PaginatedList
    {
        public PaginatedList()
        {
            PageIndex = 1;
            PageNumber = PageIndex ?? 1 + 1;
            ItemStart = PageIndex ?? 1 * PageSize + 1;
            ItemEnd = Math.Min(PageIndex ?? 1 * PageSize + PageSize, TotalItemCount);
        }
        public int PageCount { get { return TotalItemCount > 0 ? (int)Math.Ceiling(TotalItemCount / (double)PageSize) : 0; } }
        public int? PageIndex { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = GlobalSettings.DefaultPageSize;

        public bool HasPreviousPage
        {
            get { return (PageIndex > 0); }
        }

        public bool HasNextPage
        {
            get { return (PageIndex < (PageCount - 1)); }
        }

        public bool IsFirstPage { get { return (PageIndex <= 0); } }

        public bool IsLastPage
        {
            get { return (PageIndex >= (PageCount - 1)); }
        }

        public int ItemStart { get; set; }
        public int ItemEnd { get; set; }
        public int TotalItemCount { get; set; }
    }
}
