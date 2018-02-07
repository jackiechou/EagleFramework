using Eagle.Core.Settings;
using Eagle.Entities.Contents.Banners;
using Eagle.EntityFramework.Repositories;
using System.Collections.Generic;

namespace Eagle.Repositories.Contents
{
    public interface IBannerRepository : IRepositoryBase<Banner>
    {
        IEnumerable<Banner> GetList(int vendorId, string languageCode, int? bannerTypeId, BannerStatus? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<Banner> GetBanners(int vendorId, BannerTypeSetting type, BannerPositionSetting position, int? quantity, BannerStatus? status);
        IEnumerable<Banner> Search(out int recordCount, int vendorId, string languageCode, string bannerName, string advertiser,
            int? type = default(int?), int? position = default(int?), BannerStatus? status = default(BannerStatus?),
            string orderBy = null, int? page = default(int?), int? pageSize = default(int?));
        Banner GetDetails(int id);
        int GetNewListOrder();
        bool HasDataExisted(int bannerTypeId, string bannerTitle);
    }
}
