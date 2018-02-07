using System.Collections.Generic;
using Eagle.Entities.Contents.Articles;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Contents
{
    public interface INewsRatingRepository : IRepositoryBase<NewsRating>
    {
        IEnumerable<NewsRating> GetNewsRatings(int newsId);
    }
}
