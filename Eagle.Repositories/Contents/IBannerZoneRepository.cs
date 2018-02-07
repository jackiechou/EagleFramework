using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Banners;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Contents
{
    public interface IBannerZoneRepository : IRepositoryBase<BannerZone>
    {
        IEnumerable<BannerZone> GetListByBannerId(int bannerId, BannerStatus? status=null);
        IEnumerable<BannerZone> GetListByPositionId(int positionId, BannerStatus? status=null);
        BannerZone GetDetails(int bannerId, int positionId);
        bool HasDataExisted(int bannerId, int positionId);
    }
}
