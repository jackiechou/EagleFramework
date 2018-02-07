using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Products;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business
{
    public class ProductAttributeOptionRepository : RepositoryBase<ProductAttributeOption>, IProductAttributeOptionRepository
    {
        public ProductAttributeOptionRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<ProductAttributeOption> GetProductAttributeOptions(int attributeId)
        {
            return (from o in DataContext.Get<ProductAttributeOption>()
                    where o.AttributeId == attributeId
                    orderby o.ListOrder ascending 
                    select o).AsEnumerable();
        }

        public bool HasDataExisted(int attributeId, string optionName)
        {
            var query = (from o in DataContext.Get<ProductAttributeOption>()
                         where o.AttributeId == attributeId 
                         && o.OptionName.ToLower() == optionName.ToLower()
                         select o).FirstOrDefault();
            return query != null;
        }

        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<ProductAttributeOption>() select u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }
    }
}
