using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace Eagle.Core.Pagination
{
    public class PagerOptions
    {
        public PagerOptions()
        {
            RouteValues = new RouteValueDictionary();
            DisplayTemplate = DefaultPagerOptions.DisplayTemplate;
            MaxNrOfPages = DefaultPagerOptions.MaxNrOfPages;
            AlwaysAddFirstPageNumber = DefaultPagerOptions.AlwaysAddFirstPageNumber;
            PageRouteValueKey = DefaultPagerOptions.DefaultPageRouteValueKey;
            PreviousPageText = DefaultPagerOptions.PreviousPageText;
            PreviousPageTitle = DefaultPagerOptions.PreviousPageTitle;
            NextPageText = DefaultPagerOptions.NextPageText;
            NextPageTitle = DefaultPagerOptions.NextPageTitle;
            FirstPageText = DefaultPagerOptions.FirstPageText;
            FirstPageTitle = DefaultPagerOptions.FirstPageTitle;
            LastPageText = DefaultPagerOptions.LastPageText;
            LastPageTitle = DefaultPagerOptions.LastPageTitle;
            DisplayFirstPage = DefaultPagerOptions.DisplayFirstPage;
            DisplayLastPage = DefaultPagerOptions.DisplayLastPage;
            UseItemCountAsPageCount = DefaultPagerOptions.UseItemCountAsPageCount;
            HidePreviousAndNextPage = DefaultPagerOptions.HidePreviousAndNextPage;
        }

        public RouteValueDictionary RouteValues { get; internal set; }

        public string DisplayTemplate { get; internal set; }

        public int MaxNrOfPages { get; internal set; }

        public AjaxOptions AjaxOptions { get; internal set; }

        public bool AlwaysAddFirstPageNumber { get; internal set; }

        public string Action { get; internal set; }

        public string PageRouteValueKey { get; set; }

        public string PreviousPageText { get; set; }

        public string PreviousPageTitle { get; set; }

        public string NextPageText { get; set; }

        public string NextPageTitle { get; set; }

        public string FirstPageText { get; set; }

        public string FirstPageTitle { get; set; }

        public string LastPageText { get; set; }

        public string LastPageTitle { get; set; }

        public bool DisplayFirstAndLastPage { get { return DisplayFirstPage && DisplayLastPage; } }
        public bool DisplayFirstPage { get; set; }
        public bool DisplayLastPage { get; set; }

        public bool HidePreviousAndNextPage { get; internal set; }

        public bool UseItemCountAsPageCount { get; internal set; }

    }
}
