using System.Collections.Generic;
using Eagle.Entities.Business.Products;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
    public interface IProductAttributeRepository : IRepositoryBase<ProductAttribute>
    {
        IEnumerable<ProductAttribute> GetProductAttributes(int productId);
        bool HasDataExisted(int productId, string attributeName);
        int GetNewListOrder();
    }
}
