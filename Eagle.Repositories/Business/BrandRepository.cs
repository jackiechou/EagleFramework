using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Common.Extensions;
using Eagle.Core.Common;
using Eagle.Entities.Business.Brand;
using Eagle.EntityFramework;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Repositories.Business
{
    public class BrandRepository : RepositoryBase<Brand>, IBrandRepository
    {
        public BrandRepository(IDataContext dataContext) : base(dataContext)
        {   
        }

        public IEnumerable<Brand> GetBrandList(string searchText, bool? isOnline, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = from p in DataContext.Get<Brand>()
                        where (isOnline == null || isOnline == p.IsOnline)
                        select p;
            if (!searchText.IsNullOrEmpty())
            {
                query = query.Where(t => t.BrandName.Contains(searchText) || t.BrandAlias.Contains(searchText));
            }
            return query.AsEnumerable().WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);    
        }

        public bool CheckExistedName(string brandName)
        {
            var query = (from x in DataContext.Get<Brand>()
                         where x.BrandName.ToLower().Contains(brandName.ToLower())
                         select x).FirstOrDefault();
            return query != null;
        }

        public SelectList PopulateProductBrandSelectList(BrandStatus? status = null,
            int? selectedValue = null, bool? isShowSelectText = false)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from x in DataContext.Get<Brand>()
                         where status == null || x.IsOnline == (status == BrandStatus.Active)
                         select x).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem
                {
                    Text = $"{p.BrandName}",
                    Value = p.BrandId.ToString(),
                    Selected = (selectedValue != null && p.BrandId == selectedValue)
                }).ToList();

                if (isShowSelectText != null && isShowSelectText == true)
                {
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectProductBrand} ---", Value = "" });
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
