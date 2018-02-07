using System.Collections.Generic;
using System.Web.Mvc.Ajax;

namespace Eagle.Core.Pagination
{
    public class PaginationModel
    {
        public int PageSize { get; internal set; }

        public int CurrentPage { get; internal set; }

        public int PageCount { get; internal set; }

        public int TotalItemCount { get; internal set; }

        public IList<PaginationLink> PaginationLinks { get; private set; }

        public AjaxOptions AjaxOptions { get; internal set; }

        public PagerOptions Options { get; internal set; }

        public PaginationModel()
        {
            PaginationLinks = new List<PaginationLink>();
            AjaxOptions = null;
            Options = null;
        }
    }
}