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
    public class BannerScopeRepository : RepositoryBase<BannerScope>, IBannerScopeRepository
    {
        public BannerScopeRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<BannerScope> GetActiveList()
        {
            return DataContext.Get<BannerScope>().Where(x => x.Status == BannerScopeStatus.Active).AsEnumerable();
        }

        public IEnumerable<BannerScope> GetList(ref int? recordCount, int? page = null, int? pageSize = null)
        {
            var queryable = DataContext.Get<BannerScope>();
            if (recordCount != null)
            {
                recordCount = queryable.Count();
            }

            queryable = queryable.OrderBy(m => m.ScopeName);

            if (page != null && pageSize != null)
            {
                queryable = queryable.ApplyPaging(page.Value, pageSize.Value);
            }

            return queryable.AsEnumerable();
        }
        public bool IsIdExisted(int id)
        {
            var query = DataContext.Get<BannerScope>().FirstOrDefault(c => c.ScopeId == id);
            return (query != null);
        }

        public bool HasDataExisted(string bannerScopeName)
        {
            var query = DataContext.Get<BannerScope>().FirstOrDefault(p => p.ScopeName.Equals(bannerScopeName));
            return (query != null);
        }

        public SelectList PopulateBannerScopeSelectList(int? selectedValue, bool? isShowSelectText)
        {
            var listItems = new List<SelectListItem>();
            var lst = GetActiveList().ToList();
            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text = p.ScopeName, Value = p.ScopeId.ToString() }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                {
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectBannerScope} ---", Value = "" });
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
