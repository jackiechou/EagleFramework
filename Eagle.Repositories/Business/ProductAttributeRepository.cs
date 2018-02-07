using Eagle.Entities.Business.Products;
using Eagle.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Settings;

namespace Eagle.Repositories.Business
{
    public class ProductAttributeRepository : RepositoryBase<ProductAttribute>, IProductAttributeRepository
    {
        public ProductAttributeRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<ProductAttribute> GetProductAttributes(int productId)
        {
            return (from a in DataContext.Get<ProductAttribute>()
                       where a.ProductId == productId
                    select a).AsEnumerable();
        }

        public bool HasDataExisted(int productId, string attributeName)
        {
            var query = (from a in DataContext.Get<ProductAttribute>()
                         where a.ProductId == productId
                         && a.AttributeName.ToLower() == attributeName.ToLower()
                         && a.IsActive == ProductAttributeStatus.Active
                         select a).FirstOrDefault();
            return query != null;
        }

        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<ProductAttribute>() select u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }
    }
}
