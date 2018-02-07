using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Products;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Business
{
    public class ProductTaxRateRepository : RepositoryBase<ProductTaxRate>, IProductTaxRateRepository
    {
        public ProductTaxRateRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<ProductTaxRate> GetList(ProductTaxRateStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = (from m in DataContext.Get<ProductTaxRate>()
                       where (status == null || m.IsActive == status) 
                       select m);
            return lst.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public bool HasDataExisted(decimal taxRate, bool isPercent)
        {
            var entity =
                DataContext.Get<ProductTaxRate>().FirstOrDefault(x => x.TaxRate == taxRate && x.IsPercent == isPercent);
            return entity != null;
        }

        public SelectList PopulateProductTaxRateSelectList(ProductTaxRateStatus? status = null, int? selectedValue = null, bool? isShowSelectText = false)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from c in DataContext.Get<ProductTaxRate>()
                       where status == null || c.IsActive == status
                       select c).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem
                {
                    Text = $"{p.TaxRate} {(p.IsPercent ? "%" : "")}", Value = p.TaxRateId.ToString(),
                    Selected = (selectedValue != null && p.TaxRateId == selectedValue)
                }).ToList();

                if (isShowSelectText != null && isShowSelectText == true)
                {
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectProductTaxRate} ---", Value = "" });
                }
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }
    }
}
