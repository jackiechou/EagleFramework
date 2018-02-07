using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Common;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.SystemManagement
{
    public class RegionRepository: RepositoryBase<Region>, IRegionRepository
    {
        public RegionRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<Region> GetList(ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            return DataContext.Get<Region>().WithRecordCount(ref recordCount)
                            .WithSortingAndPaging(orderBy, page, pageSize);
        }
        public SelectList PopulateRegionSelectList(int? provinceId, bool? status=null, int? selectedValue=null, bool? isShowSelectText = true)
        {
            var list = new List<SelectListItem>();
            if (provinceId == null)
            {
                if (isShowSelectText != null && isShowSelectText == true)
                    list.Insert(0, new SelectListItem() { Value = "", Text = $"-- {LanguageResource.SelectRegion} --" });
                return new SelectList(list, "Value", "Text", selectedValue);
            }
            else
            {
                list = (from x in DataContext.Get<Region>()
                        where x.ProvinceId == provinceId && (status == null || x.IsActive == status)
                        select new SelectListItem
                        {
                            Text = x.RegionName,
                            Value = x.RegionId.ToString(),
                            Selected = (selectedValue != null && x.RegionId == selectedValue)
                        }).ToList();

                if (list.Any())
                {
                    if (isShowSelectText != null && isShowSelectText == true)
                        list.Insert(0, new SelectListItem() { Value = "", Text = $"-- {LanguageResource.SelectRegion} --" });
                }
                else
                {
                    list.Insert(0, new SelectListItem() { Value = "", Text = $"-- {LanguageResource.None} --" });
                }
                return new SelectList(list, "Value", "Text", selectedValue);
            }
        }
        public bool HasDataExisted(string regionName)
        {
            var query = DataContext.Get<Region>().Where(c => c.RegionName == regionName);
            return (query.Any());
        }
    }
}
