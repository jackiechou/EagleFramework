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
    public class BannerPositionRepository : RepositoryBase<BannerPosition>, IBannerPositionRepository
    {
        public BannerPositionRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<BannerPosition> GetList(BannerPositionStatus? status, ref int? recordCount, int? page = null, int? pageSize = null)
        {
            var queryable = DataContext.Get<BannerPosition>().Where(x=> (status==null && x.Status==status));
            if (recordCount != null)
            {
                recordCount = queryable.Count();
            }

            queryable = queryable.OrderBy(m => m.PositionName);

            if (page != null && pageSize != null)
            {
                queryable = queryable.ApplyPaging(page.Value, pageSize.Value);
            }

            return queryable.AsEnumerable();
        }
        public IEnumerable<BannerPosition> GetList(BannerPositionStatus? status)
        {
            return DataContext.Get<BannerPosition>().Where(x => status == null || x.Status == status).AsEnumerable();
        }
        public IEnumerable<BannerPosition> GetListByRoleIdUserIdStatus(int node)
        {
            string strCommand = @"EXEC dbo.BannerPosition_GetTreeNodes @node={0}";
            return DataContext.Get<BannerPosition>(strCommand, node).AsEnumerable();
        }

        public MultiSelectList PopulateBannerPositionMultiSelectList(bool? isShowSelectText = null, BannerPositionStatus? status = null, int[] selectedValues = null)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from p in DataContext.Get<BannerPosition>()
                         where status == null || p.Status == status
                         select p).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem
                {
                    Text = p.PositionName,
                    Value = p.PositionId.ToString(),
                    Selected = (selectedValues !=null && selectedValues.Contains(p.PositionId))
                }).OrderByDescending(m => m.Text).ToList();

                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectBannerPosition} ---", Value = "" });
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            
            return new MultiSelectList(listItems, "Value", "Text", selectedValues);
        }

        public int? GetLastListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<BannerPosition>() select u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max();
            }
            return listOrder;
        }

        public bool HasDataExisted(string bannerPositionName)
        {
            var query = DataContext.Get<BannerPosition>().FirstOrDefault(p => p.PositionName.Equals(bannerPositionName));
            return (query != null);
        }

    }
}
