using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Products;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
   public interface IProductTaxRateRepository : IRepositoryBase<ProductTaxRate>
   {
       IEnumerable<ProductTaxRate> GetList(ProductTaxRateStatus? status, ref int? recordCount, string orderBy = null,
           int? page = null, int? pageSize = null);
       bool HasDataExisted(decimal taxRate, bool isPercent);

       SelectList PopulateProductTaxRateSelectList(ProductTaxRateStatus? status = null, int? selectedValue = null,
           bool? isShowSelectText = false);
   }
}
