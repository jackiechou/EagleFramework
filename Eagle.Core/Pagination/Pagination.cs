namespace Eagle.Core.Pagination
{
    public class Pagination
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int? PageSize { get; set; }
        public int? Page { get; set; }
    }
}
