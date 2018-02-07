using System.Web.Http;
using Eagle.Core.Configuration;

namespace Eagle.Core.Search
{
    public class SearchRequest<T>
    {
        public T Filter { get; set; }
        public int? Page { get; set; } = 1;
        public int? PageSize { get; set; } = GlobalSettings.DefaultPageSize;
    }
}
