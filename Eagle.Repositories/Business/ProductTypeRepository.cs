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
    public class ProductTypeRepository : RepositoryBase<ProductType>, IProductTypeRepository
    {
        public ProductTypeRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<ProductType> GetProductTypes(int vendorId, int? categoryId, string searchText, ProductTypeStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = (from t in DataContext.Get<ProductType>()
                       where t.VendorId == vendorId && (status == null || t.IsActive == status) 
                       select t);

            if (categoryId !=null && categoryId > 0)
            {
                query = query.Where(x => x.CategoryId == categoryId);
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(x => x.TypeName.ToLower().Contains(searchText));
            }
            return query.AsEnumerable().WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public SelectList PoplulateProductTypeSelectList(int categoryId, ProductTypeStatus? status = null, int? selectedValue = null, bool? isShowSelectText = false)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from c in DataContext.Get<ProductType>()
                       where c.CategoryId == categoryId && (status == null || c.IsActive == status)
                       select c).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text = p.TypeName, Value = p.TypeId.ToString(), Selected = (selectedValue != null && p.TypeId == selectedValue) }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                {
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectProductType} ---", Value = "" });
                }
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }
        public ProductType GetDetails(int vendorId, int productTypeId)
        {
            return DataContext.Get<ProductType>().FirstOrDefault(c =>
                c.VendorId == vendorId && c.TypeId == productTypeId);
        }
        public ProductType FindByProductTypeName(string productTypeName)
        {
            return DataContext.Get<ProductType>().FirstOrDefault(x => x.TypeName.ToLower() == productTypeName.ToLower());
        }
        public ProductType GetNextItem(int currentItemId)
        {
            return (from x in DataContext.Get<ProductType>()
                    where x.TypeId > currentItemId
                    orderby x.ListOrder descending
                    select x).FirstOrDefault();
        }
        public ProductType GetPreviousItem(int currentItemId)
        {
            return (from x in DataContext.Get<ProductType>()
                    where x.TypeId < currentItemId
                    orderby x.ListOrder descending
                    select x).FirstOrDefault();
        }
        public bool HasDataExisted(int vendorId, int productTypeId)
        {
            var query = DataContext.Get<ProductType>().FirstOrDefault(c =>
                c.VendorId == vendorId && c.TypeId == productTypeId);
            return (query != null);
        }
        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<ProductType>() select (int)u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }
    }
}
