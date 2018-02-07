using Eagle.Core.Configuration;

namespace Eagle.Core.Search
{
    public class SearchDataRequest<T>
    {
        public SearchDataRequest()
        {
            PageIndex = 1;
            PageNumber = PageIndex ?? 1 + 1;
        }

        public T Filter { get; set; }
        public int? PageIndex { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = GlobalSettings.DefaultPageSize;
    }
}
