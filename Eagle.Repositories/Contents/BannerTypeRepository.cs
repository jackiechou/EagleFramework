using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Banners;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Contents
{
    public class BannerTypeRepository : RepositoryBase<BannerType>, IBannerTypeRepository
    {
        public BannerTypeRepository(IDataContext dataContext) : base(dataContext) { }
     
        public IEnumerable<BannerType> GetActiveList()
        {
            return DataContext.Get<BannerType>().Where(x => x.Status == BannerTypeStatus.Active).AsEnumerable();
        }

        public IEnumerable<BannerType> GetList(ref int? recordCount, int? page = null, int? pageSize = null)
        {
            var queryable = DataContext.Get<BannerType>();
            if (recordCount != null)
            {
                recordCount = queryable.Count();
            }

            queryable = queryable.OrderBy(m => m.TypeName);

            if (page != null && pageSize != null)
            {
                queryable = queryable.ApplyPaging(page.Value, pageSize.Value);
            }

            return queryable.AsEnumerable();
        }
        public bool IsIdExisted(int id)
        {
            var query = DataContext.Get<BannerType>().FirstOrDefault(c => c.TypeId == id);
            return (query != null);
        }
        
        public bool HasDataExisted(string bannerTypeName)
        {
            var query = DataContext.Get<BannerType>().FirstOrDefault(p => p.TypeName.Equals(bannerTypeName));
            return (query != null);
        }
      
        public SelectList PopulateBannerTypeSelectList(int? selectedValue, bool? isShowSelectText)
        {
            var listItems = new List<SelectListItem>();
            var lst = GetActiveList().ToList();
            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text = p.TypeName, Value = p.TypeId.ToString() }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectBannerType} ---", Value = "" });
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }
    }
}
