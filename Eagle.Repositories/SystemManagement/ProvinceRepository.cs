using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Common;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.SystemManagement
{
    public class ProvinceRepository: RepositoryBase<Province>, IProvinceRepository
    {
        public ProvinceRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<Province> GetList(ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            return DataContext.Get<Province>().WithRecordCount(ref recordCount)
                            .WithSortingAndPaging(orderBy, page, pageSize);
        }
        public SelectList PopulateProvinceSelectList(int countryId, bool? status=null, int? selectedValue=null, bool? isShowSelectText = true)
        {
            var list = (from x in DataContext.Get<Province>()
                        where x.CountryId == countryId && (status == null || x.IsActive == status)
                        select new SelectListItem
                        {
                            Text = x.ProvinceName,
                            Value = x.ProvinceId.ToString(),
                            Selected = (selectedValue != null && x.ProvinceId == selectedValue)
                        }).ToList();

            if (list.Any())
            {
                if (isShowSelectText != null && isShowSelectText == true)
                {
                    list.Insert(0, new SelectListItem() { Value = "", Text = $"-- {LanguageResource.SelectProvince} --" });
                }
            }
            else
            {
                list.Insert(0, new SelectListItem() { Value = "", Text = $"-- {LanguageResource.None} --" });
            }

            return new SelectList(list, "Value", "Text", selectedValue);
        }
        public bool HasDataExisted(string provinceName)
        {
            var query = DataContext.Get<Province>().Where(c =>
                    c.ProvinceName.ToLower() == provinceName.ToLower());
            return (query.Any());
        }
    }
}
