using Eagle.Core.Pagination;

namespace Eagle.Core.Search
{
    public class SearchResult<TResult>
    {
        public IPagedList<TResult> PagedList { get; set; }
    }
}
