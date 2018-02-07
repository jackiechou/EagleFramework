using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Settings;
using Eagle.Services.Contents;
using Eagle.Services.Dtos.Contents.Banners;
using Eagle.Services.SystemManagement;

namespace Eagle.WebApp.Controllers
{
    public class BannerInfoController : BasicController
    {
        private IBannerService BannerService { get; set; }
        private ICacheService CacheService { get; set; }

        public BannerInfoController(IBannerService bannerService, ICacheService cacheService)
        {
            BannerService = bannerService;
            CacheService = cacheService;
        }

        // GET: BannerInfo
        [HttpGet]
        public ActionResult GetTopBanners()
        {
            var banners = CacheService.Get<IEnumerable<BannerInfoDetail>>(CacheKeySetting.TopBanner);
            if (banners != null && banners.Any()) return PartialView("../BannerInfo/_Banner", banners);
            int vendorId = GlobalSettings.DefaultVendorId;
            banners = BannerService.GetBanners(vendorId, BannerTypeSetting.Image, BannerPositionSetting.Top, null, BannerStatus.Active);
            CacheService.Set(CacheKeySetting.TopBanner, banners);
            return PartialView("../BannerInfo/_Banner", banners);
        }

        // GET: BannerInfoBooking
        [HttpGet]
        public ActionResult GetHomeBanners()
        {
            var banners = CacheService.Get<IEnumerable<BannerInfoDetail>>(CacheKeySetting.TopBanner);
            if (banners != null && banners.Any()) return PartialView("../BannerInfo/_BannerBooking", banners);

            int vendorId = GlobalSettings.DefaultVendorId;
            banners = BannerService.GetBanners(vendorId, BannerTypeSetting.Image, BannerPositionSetting.Top, null, BannerStatus.Active);
            CacheService.Set(CacheKeySetting.TopBanner, banners);
            return PartialView("../BannerInfo/_BannerBooking", banners);
        }

        // GET: Right BannerInfo
        [HttpGet]
        public ActionResult GetRightBanners()
        {
            var banners = CacheService.Get<IEnumerable<BannerInfoDetail>>(CacheKeySetting.RightBanner);
            if (banners != null && banners.Any()) return PartialView("../BannerInfo/_BannerRight", banners);

            int vendorId = GlobalSettings.DefaultVendorId;
            banners = BannerService.GetBanners(vendorId, BannerTypeSetting.Image, BannerPositionSetting.Right, null, BannerStatus.Active);
            CacheService.Set(CacheKeySetting.RightBanner, banners);
            return PartialView("../BannerInfo/_BannerRight", banners);
        }

        // GET: Bottom BannerInfo
        [HttpGet]
        public ActionResult GetFooterBanners()
        {
            var banners = CacheService.Get<IEnumerable<BannerInfoDetail>>(CacheKeySetting.FooterBanner);
            if (banners != null && banners.Any()) return PartialView("../BannerInfo/_BannerFooter", banners);

            int vendorId = GlobalSettings.DefaultVendorId;
            banners = BannerService.GetBanners(vendorId, BannerTypeSetting.Image, BannerPositionSetting.Bottom, null,
                BannerStatus.Active);
            CacheService.Set(CacheKeySetting.FooterBanner, banners);
            return PartialView("../BannerInfo/_BannerFooter", banners);
        }

        #region Dispose

        private bool _disposed;

        [NonAction]
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    BannerService = null;
                    CacheService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}