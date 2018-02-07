using System.Web.Mvc;
using Eagle.Core.Pagination;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Business;

namespace Eagle.WebApp.Controllers
{
    public class PartnerController : BasicController
    {
        private IVendorService VendorService { get; set; }

        public PartnerController(IVendorService vendorService)
        {
            VendorService = vendorService;
        }

        // GET: BannerInfo
        public ActionResult GetPartners()
        {
            var filter = new VendorPartnerSearchEntry
            {
                PartnerName = null,
                Status = true
            };

            int? page = 1;
            int pageSize = 5;
            int? recordCount = 0;
            var sources = VendorService.GetPartners(filter, ref recordCount, "PartnerId DESC", page, pageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, pageSize, recordCount);
            return PartialView("../Partner/_PartnerList", lst);
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
                    VendorService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}