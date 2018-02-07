using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Products;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
    public interface IAttributeRepository : IRepositoryBase<Attribute>
    {
        IEnumerable<Attribute> GetAttributes(int productId);
        bool HasDataExisted(int productId, string attributeName);
        int GetNewListOrder();
        IEnumerable<Attribute> GetAttributes(string attributeName, ProductAttributeStatus isActive, ref int? recordCount, string orderBy, int? page, int defaultPageSize);
    }
}
