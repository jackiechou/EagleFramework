using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Manufacturers;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Business
{
    public class ManufacturerCategoryRepository: RepositoryBase<ManufacturerCategory>,IManufacturerCategoryRepository
    {
        public ManufacturerCategoryRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<ManufacturerCategory> GetManufacturerCategories(int vendorId, string searchText, ManufacturerCategoryStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = (from c in DataContext.Get<ManufacturerCategory>()
                       where c.VendorId == vendorId && (status == null || c.IsActive == status) || c.CategoryName.Contains(searchText)
                       select c);
            return lst.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }

        public ManufacturerCategory GetDetails(int manufacturerCategoryId)
        {
            return (from c in DataContext.Get<ManufacturerCategory>()
                    where c.CategoryId == manufacturerCategoryId
                    select c).FirstOrDefault();
        }

        public SelectList PopulateManufacturerCategorySelectList(ManufacturerCategoryStatus? status = null, int? selectedValue = null, bool? isShowSelectText = false)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from c in DataContext.Get<ManufacturerCategory>()
                       where status == null || c.IsActive == status
                       select c).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text = p.CategoryName, Value = p.CategoryId.ToString(), Selected = (selectedValue != null && p.CategoryId == selectedValue) }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectManufacturerCategory} ---", Value = "" });
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }
    }
}
