using System.Collections.Generic;
using System.Linq;
using Eagle.Entities.Business.Products;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business
{
    public class ProductRatingRepository : RepositoryBase<ProductRating>, IProductRatingRepository
    {
        public ProductRatingRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<ProductRating> GetProductRatings(int productId)
        {
            return DataContext.Get<ProductRating>().Where(x => x.ProductId == productId);
        }
    }
}
