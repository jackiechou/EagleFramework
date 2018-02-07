using System.Collections.Generic;
using System.Linq;
using Eagle.Entities.Contents.Articles;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Contents
{
    public class NewsRatingRepository : RepositoryBase<NewsRating>, INewsRatingRepository
    {
        public NewsRatingRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<NewsRating> GetNewsRatings(int newsId)
        {
            return DataContext.Get<NewsRating>().Where(x => x.NewsId == newsId);
        }
    }
}
