using System.Collections.Generic;
using Eagle.Entities.Business.Products;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
    public interface IAttributeOptionRepository : IRepositoryBase<AttributeOption>
    {
        IEnumerable<AttributeOption> GetAttributeOptions(int attributeId);
        bool HasDataExisted(int attributeId, string optionName);
        int GetNewListOrder();
    }
}
