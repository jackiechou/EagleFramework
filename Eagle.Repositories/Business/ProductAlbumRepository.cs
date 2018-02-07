using System.Collections.Generic;
using System.Linq;
using Eagle.Entities.Business.Products;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business
{
    public class ProductAlbumRepository : RepositoryBase<ProductAlbum>, IProductAlbumRepository
    {
        public ProductAlbumRepository(IDataContext dataContext) : base(dataContext) { }


        public ProductAlbum FindImageByProductId(int productId, int fileId)
        {
            return (from o in DataContext.Get<ProductAlbum>()
                    where o.ProductId == productId
                    where o.FileId == fileId
                    select o).FirstOrDefault();
        }

        public IEnumerable<ProductAlbum> GetProductAlbum(int productId)
        {
            return (from o in DataContext.Get<ProductAlbum>()
                    where o.ProductId == productId
                    select o).AsEnumerable();
        }
    }
}
