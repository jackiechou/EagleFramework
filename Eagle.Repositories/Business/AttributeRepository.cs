using Eagle.Entities.Business.Products;
using Eagle.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Settings;
using Eagle.Core.Common;

namespace Eagle.Repositories.Business
{
    public class AttributeRepository : RepositoryBase<Attribute>, IAttributeRepository
    {
        public AttributeRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<Attribute> GetAttributes(int productId)
        {
            return (from a in DataContext.Get<Attribute>()
                       where a.CategoryId == productId
                    select a).AsEnumerable();
        }

        public bool HasDataExisted(int productId, string attributeName)
        {
            var query = (from a in DataContext.Get<Attribute>()
                         where a.CategoryId == productId
                         && a.AttributeName.ToLower() == attributeName.ToLower()
                         && a.IsActive == ProductAttributeStatus.Active
                         select a).FirstOrDefault();
            return query != null;
        }

        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<Attribute>() select u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }

        public IEnumerable<Attribute> GetAttributes(string attributeName, ProductAttributeStatus isActive, ref int? recordCount, string orderBy, int? page, int defaultPageSize)
        {
            var query = DataContext.Get<Attribute>().Where(x =>
      ( x.IsActive == isActive));
            return query.WithRecordCount(ref recordCount)
                            .WithSortingAndPaging(orderBy, page, defaultPageSize);
        }
    }
}
