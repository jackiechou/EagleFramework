using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Common;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.SystemManagement
{
    public class CountryRepository : RepositoryBase<Country>, ICountryRepository
    {
        public CountryRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<Country> GetList(ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            return DataContext.Get<Country>().WithRecordCount(ref recordCount)
                            .WithSortingAndPaging(orderBy, page, pageSize);
        }
        public SelectList PopulateCountrySelectList(bool? status = null, int? selectedValue = null, bool? isShowSelectText = false)
        {
            var lst = (from x in DataContext.Get<Country>()
                        where (status==null || x.IsActive == status)
                        select new SelectListItem
                        {
                            Text = x.CountryName,
                            Value = x.CountryId.ToString(),
                            Selected = (selectedValue!=null && x.CountryId == selectedValue)
                        }).ToList();

            if (lst.Any())
            {
                if (isShowSelectText != null && isShowSelectText == true)
                    lst.Insert(0, new SelectListItem() { Value = "", Text = $"-- {LanguageResource.SelectCountry} --" });
            }
            else
            {
                lst.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }

            return new SelectList(lst, "Value", "Text", selectedValue);
        }
    }
}
