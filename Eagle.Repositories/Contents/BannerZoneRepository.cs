using Eagle.Core.Settings;
using Eagle.Entities.Contents.Banners;
using Eagle.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace Eagle.Repositories.Contents
{
    public class BannerZoneRepository : RepositoryBase<BannerZone>, IBannerZoneRepository
    {
        public BannerZoneRepository(IDataContext dataContext) : base(dataContext)
        {
        }

        public IEnumerable<BannerZone> GetListByBannerId(int bannerId, BannerStatus? status=null)
        {
            return (from x in DataContext.Get<BannerZone>()
                    join y in DataContext.Get<Banner>() on x.BannerId equals y.BannerId into zones
                    from zl in zones.DefaultIfEmpty()
                    where x.BannerId == bannerId && (status == null || zl.Status == status)
                    orderby zl.ListOrder
                    select x).AsEnumerable();
        }

        public IEnumerable<BannerZone> GetListByPositionId(int positionId, BannerStatus? status = null)
        {
            return (from x in DataContext.Get<BannerZone>()
                    join y in DataContext.Get<Banner>() on x.BannerId equals y.BannerId into zones
                    from zl in zones.DefaultIfEmpty()
                    where x.PositionId == positionId && (status == null || zl.Status == status)
                    orderby zl.ListOrder
                    select x).AsEnumerable();
        }

        public BannerZone GetDetails(int bannerId, int positionId)
        {
            return (from z in DataContext.Get<BannerZone>()
                    where z.BannerId == bannerId && z.PositionId == positionId
                    select z).FirstOrDefault();
        }

        public bool HasDataExisted(int bannerId, int positionId)
        {
            var query = DataContext.Get<BannerZone>().FirstOrDefault(z => z.BannerId == bannerId && z.PositionId == positionId);
            return (query != null);
        }
    }
}
