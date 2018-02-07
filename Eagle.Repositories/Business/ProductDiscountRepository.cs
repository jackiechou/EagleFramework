using System;
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
    public class ProductDiscountRepository : RepositoryBase<ProductDiscount>, IProductDiscountRepository
    {
        public ProductDiscountRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<ProductDiscount> GetProductDiscounts(int vendorId, DateTime? startDate, DateTime? endDate, ProductDiscountStatus? status, ref int? recordCount,
           string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = from d in DataContext.Get<ProductDiscount>()
                where d.VendorId == vendorId && (status == null || d.IsActive == status)
                select d;

            if (startDate != null && endDate == null)
            {
                query = query.Where(x => x.StartDate >= startDate);
            }
            if (startDate == null && endDate != null)
            {
                query = query.Where(x => x.EndDate <= endDate);
            }
            if (startDate != null && endDate != null)
            {
                query = query.Where(x => x.StartDate >= startDate && x.EndDate <= endDate);
            }
            return query.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public ProductDiscount GetDetails(int vendorId, int discountId)
        {
            return DataContext.Get<ProductDiscount>().FirstOrDefault(c =>
                c.VendorId == vendorId && c.DiscountId == discountId);
        }
        public bool HasDataExisted(int? quantity, decimal rate, bool? isPercent)
        {
            var query = (from d in DataContext.Get<ProductDiscount>()
                         where (quantity==null || d.Quantity == quantity)
                         && (isPercent == null || d.IsPercent == isPercent)
                         && d.DiscountRate== rate && d.IsActive==ProductDiscountStatus.Active
                         select d).FirstOrDefault();
            return query != null;
        }

        public SelectList PopulateProductDiscountSelectList(DiscountType type, ProductDiscountStatus? status = null, int? selectedValue = null, bool? isShowSelectText = false)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from c in DataContext.Get<ProductDiscount>()
                       where c.DiscountType == type &&  (status == null || c.IsActive == status)
                       select c).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text =
                    $@"{p.DiscountRate} {(p.IsPercent ? "%" : string.Empty)}", Value = p.DiscountId.ToString(), Selected = (selectedValue != null && p.DiscountId == selectedValue) }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                {
                    listItems.Insert(0, new SelectListItem { Text = $@"--- {LanguageResource.SelectProductDiscount} ---", Value = "" });
                }
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $@"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }

    }
}
