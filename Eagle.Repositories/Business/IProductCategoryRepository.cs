using System;
using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Products;
using Eagle.Entities.Common;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
    public interface IProductCategoryRepository : IRepositoryBase<ProductCategory>
    {
        IEnumerable<ProductCategory> GetList(string languageCode, int vendorId, ProductCategoryStatus? status, ref int? recordCount,
      string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<ProductCategory> GetList(string languageCode, int vendorId, Guid categoryCode, ProductCategoryStatus? status, ref int? recordCount,
      string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<TreeEntity> GetProductCategorySelectTree(ProductCategoryStatus? status, int? selectedId,
            bool? isRootShowed = false);
        ProductCategory FindByCategoryCode(string categoryCode);
        ProductCategory FindByCategoryName(string categoryName);
        int GetNewListOrder();
        bool HasDataExisted(string name, int? parentId);

    }
}
