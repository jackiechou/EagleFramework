using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Products;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
    public interface IProductTypeRepository : IRepositoryBase<ProductType>
    {
        IEnumerable<ProductType> GetProductTypes(int vendorId, int? categoryId, string searchText, ProductTypeStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);

        SelectList PoplulateProductTypeSelectList(int categoryId, ProductTypeStatus? status = null,
            int? selectedValue = null, bool? isShowSelectText = false);
        ProductType GetDetails(int vendorId, int productTypeId);
        ProductType FindByProductTypeName(string productTypeName);
        ProductType GetNextItem(int currentItemId);
        ProductType GetPreviousItem(int currentItemId);
        bool HasDataExisted(int vendorId, int productTypeId);
        int GetNewListOrder();
    }
}
