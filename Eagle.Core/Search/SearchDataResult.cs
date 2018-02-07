using System.Collections.Generic;
using Eagle.Core.Pagination;

namespace Eagle.Core.Search
{
    public class SearchDataResult<TData>
    {
        public IEnumerable<TData> List { get; set; }
        public PaginatedList PaginatedList { get; set; }
    }
}
