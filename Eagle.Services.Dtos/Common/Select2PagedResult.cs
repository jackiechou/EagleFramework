using System.Collections.Generic;

namespace Eagle.Services.Dtos.Common
{
    public class Select2PagedResult
    {
        public bool MorePage { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public IEnumerable<Select2Result> Results { get; set; }
    }
}
