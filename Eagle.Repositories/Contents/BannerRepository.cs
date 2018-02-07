using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Banners;
using Eagle.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace Eagle.Repositories.Contents
{
    public class BannerRepository : RepositoryBase<Banner>, IBannerRepository
    {
        public BannerRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<Banner> GetList(int vendorId, string languageCode, int? bannerTypeId, BannerStatus? status,
            out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var queryable = (from b in DataContext.Get<Banner>()
                             join t in DataContext.Get<BannerType>() on b.TypeId equals t.TypeId into typeLst
                             from type in typeLst.DefaultIfEmpty()
                             where b.VendorId == vendorId && b.LanguageCode == languageCode
                                  && (status == null || b.Status == status)
                                  && (bannerTypeId == null || b.TypeId == bannerTypeId)
                             select b);

            return queryable.AsEnumerable().WithRecordCount(out recordCount)
                                        .WithSortingAndPaging(orderBy, page, pageSize);
        }

        public IEnumerable<Banner> GetBanners(int vendorId, BannerTypeSetting type, BannerPositionSetting position, int? quantity, BannerStatus? status)
        {
            var queryable = (from b in DataContext.Get<Banner>()
                             join z in DataContext.Get<BannerZone>() on b.BannerId equals z.BannerId into zoneJoin
                             from bz in zoneJoin.DefaultIfEmpty()
                             where b.VendorId == vendorId && b.TypeId == (int)type && bz.PositionId == (int)position
                             && (status == null || b.Status == status)
                             select b).ToList();
            if (quantity != null && quantity > 0)
            {
                var total = queryable.Count();
                queryable = total > quantity ? queryable.Take((int)quantity).ToList() : queryable.Take(total).ToList();
            }
            return queryable;
        }

        public IEnumerable<Banner> Search(out int recordCount, int vendorId, string languageCode, string bannerName, string advertiser, int? type = default(int?), int? position = default(int?), BannerStatus? status = default(BannerStatus?), string orderBy = null, int? page = default(int?), int? pageSize = default(int?))
        {
            IQueryable<Banner> queryable = null;
            if (position.HasValue)
            {
                queryable = (from b in DataContext.Get<Banner>()
                             join z in DataContext.Get<BannerZone>() on b.BannerId equals z.BannerId into zoneJoin
                             from bz in zoneJoin.DefaultIfEmpty()
                             where b.VendorId == vendorId && b.LanguageCode == languageCode
                             && bz.PositionId == position
                             && (status == null || b.Status == status)
                             select b);
            }
            else
            {
                queryable = (from b in DataContext.Get<Banner>()
                             where b.LanguageCode == languageCode
                             && (status == null || b.Status == status)
                             select b);
            }

            if (!string.IsNullOrEmpty(bannerName))
            {
                var bannerLowerName = bannerName.ToLower();
                queryable = queryable.Where(x => x.BannerTitle.ToLower().Contains(bannerLowerName));
            }

            if (!string.IsNullOrEmpty(advertiser))
            {
                var advertiserLowerName = advertiser.ToLower();
                queryable = queryable.Where(x => x.Advertiser.ToLower().Contains(advertiserLowerName));
            }

            if (type.HasValue)
            {
                queryable = queryable.Where(x => x.TypeId == type.Value);
            }

            return queryable.WithRecordCount(out recordCount)
                            .WithSortingAndPaging(orderBy, page, pageSize);
        }

        public Banner GetDetails(int id)
        {
            return (from c in DataContext.Get<Banner>()
                    join t in DataContext.Get<BannerType>() on c.TypeId equals t.TypeId into typeLst
                    from type in typeLst.DefaultIfEmpty()
                    where c.BannerId == id
                    select c).FirstOrDefault();
        }

        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<Banner>() select (int)u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }
        public bool HasDataExisted(int bannerTypeId, string bannerTitle)
        {
            var query = (from b in DataContext.Get<Banner>()
                         select b).FirstOrDefault(b => b.TypeId == bannerTypeId && b.BannerTitle.ToLower() == bannerTitle.ToLower());
            return query != null;
        }
    }
}
