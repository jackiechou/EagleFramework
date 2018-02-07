using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.Contents.Banners;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Contents
{
    public interface IBannerScopeRepository : IRepositoryBase<BannerScope>
    {
        IEnumerable<BannerScope> GetActiveList();
        IEnumerable<BannerScope> GetList(ref int? recordCount, int? page = null, int? pageSize = null);
        bool IsIdExisted(int id);
        bool HasDataExisted(string bannerScopeName);
        SelectList PopulateBannerScopeSelectList(int? selectedValue, bool? isShowSelectText);
    }
}
