using System.Collections.Generic;
using Eagle.Entities.Business.Products;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
    public interface IProductRatingRepository: IRepositoryBase<ProductRating>
    {
        IEnumerable<ProductRating> GetProductRatings(int servicePackId);
    }
}
