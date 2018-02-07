using Eagle.Entities.Business.Products;
using Eagle.EntityFramework.Repositories;
using System.Collections.Generic;

namespace Eagle.Repositories.Business
{
    public interface IProductAlbumRepository : IRepositoryBase<ProductAlbum>
    {
        IEnumerable<ProductAlbum> GetProductAlbum(int productId);
        ProductAlbum FindImageByProductId(int productId, int fileId);
    }
}
