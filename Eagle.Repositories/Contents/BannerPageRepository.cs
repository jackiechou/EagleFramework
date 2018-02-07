using System.Collections.Generic;
using System.Linq;
using Eagle.Entities.Contents.Banners;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Contents
{
    public class BannerPageRepository : RepositoryBase<BannerPage>, IBannerPageRepository
    {
        public BannerPageRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<BannerPage> GetListByBannerId(int bannerId)
        {
            return (from x in DataContext.Get<BannerPage>()
                    join y in DataContext.Get<Banner>() on x.BannerId equals y.BannerId into zones
                    from zl in zones.DefaultIfEmpty()
                    where x.BannerId == bannerId
                    orderby zl.ListOrder
                    select x).AsEnumerable();
        }

        public IEnumerable<BannerPage> GetListByPageId(int pageId)
        {
            return (from x in DataContext.Get<BannerPage>()
                    join y in DataContext.Get<Banner>() on x.BannerId equals y.BannerId into zones
                    from zl in zones.DefaultIfEmpty()
                    where x.PageId == pageId 
                    orderby zl.ListOrder
                    select x).AsEnumerable();
        }

        public BannerPage GetDetails(int bannerId, int pageId)
        {
            return (from z in DataContext.Get<BannerPage>()
                    where z.BannerId == bannerId && z.PageId == pageId
                    select z).FirstOrDefault();
        }

        public bool HasDataExisted(int bannerId, int pageId)
        {
            var query = DataContext.Get<BannerPage>().FirstOrDefault(z => z.BannerId == bannerId && z.PageId == pageId);
            return (query != null);
        }
    }
}
