using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Products;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
    public interface IProductDiscountRepository : IRepositoryBase<ProductDiscount>
    {
        IEnumerable<ProductDiscount> GetProductDiscounts(int vendorId, DateTime? startDate, DateTime? endDate, ProductDiscountStatus? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        ProductDiscount GetDetails(int vendorId, int discountId);
        bool HasDataExisted(int? quantity, decimal rate, bool? isPercent);

        SelectList PopulateProductDiscountSelectList(DiscountType type, ProductDiscountStatus? status = null,
            int? selectedValue = null, bool? isShowSelectText = false);
    }
}
