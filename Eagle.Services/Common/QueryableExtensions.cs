using System.Linq;

namespace Eagle.Services.Common
{
    internal static class QueryableExtensions
    {
        public static IQueryable<TResult> WithPageCount<TResult>(this IQueryable<TResult> source, int pageSize, out int pageCount)
        {
            var recordsCount = source.Count();
            pageCount = recordsCount > 0 ? (recordsCount/pageSize) + 1 : 0;
            return source;
        }

        public static IQueryable<TResult> WithPaging<TResult>(this IQueryable<TResult> source, int page, int pageSize)
        {
            var result = source
                    .Skip((page - 1)*pageSize)
                    .Take(pageSize);

            return result;
        }
    }
}
