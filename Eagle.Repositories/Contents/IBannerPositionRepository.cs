using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Banners;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Contents
{
    public interface IBannerPositionRepository : IRepositoryBase<BannerPosition>
    {
        IEnumerable<BannerPosition> GetList(BannerPositionStatus? status, ref int? recordCount, int? page = null, int? pageSize = null);
        IEnumerable<BannerPosition> GetList(BannerPositionStatus? status);
        IEnumerable<BannerPosition> GetListByRoleIdUserIdStatus(int node);

        MultiSelectList PopulateBannerPositionMultiSelectList(bool? isShowSelectText = null, BannerPositionStatus? status = null, int[] selectedValues = null);
        int? GetLastListOrder();
        bool HasDataExisted(string bannerPositionName);
    }
}
