namespace Eagle.Core.Pagination
{
    public static class DefaultPagerOptions
    {
        public const int MaxNrOfPages = 10;
        public const string DisplayTemplate = null;
        public const bool AlwaysAddFirstPageNumber = false;
        public const string DefaultPageRouteValueKey = "page";
        public const string PreviousPageText = "«";
        public const string PreviousPageTitle = "Previous page";
        public const string NextPageText = "»";
        public const string NextPageTitle = "Next page";
        public const string FirstPageText = "<";
        public const string FirstPageTitle = "First page";
        public const string LastPageText = ">";
        public const string LastPageTitle = "Last page";
        public const bool DisplayFirstPage = false;
        public const bool DisplayLastPage = false;
        public const bool UseItemCountAsPageCount = false;
        public static bool HidePreviousAndNextPage = false;
    }
}
