using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Banners;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Contents
{
    public interface IBannerPageRepository : IRepositoryBase<BannerPage>
    {
        IEnumerable<BannerPage> GetListByBannerId(int bannerId);
        IEnumerable<BannerPage> GetListByPageId(int pageId);
        BannerPage GetDetails(int bannerId, int pageId);
        bool HasDataExisted(int bannerId, int pageId);
    }
}
