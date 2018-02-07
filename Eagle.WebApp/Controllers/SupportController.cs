using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Business;
using Eagle.Services.SystemManagement;

namespace Eagle.WebApp.Controllers
{
    public class SupportController : BasicController
    {
        private IContactService ContactService { get; set; }
        private IDocumentService DocumentService { get; set; }
        private IVendorService VendorService { get; set; }
        private ICacheService CacheService { get; set; }

        public SupportController(IVendorService vendorService, IContactService contactService, 
            IDocumentService documentService, ICacheService cacheService)
        {
            VendorService = vendorService;
            ContactService = contactService;
            DocumentService = documentService;
            CacheService = cacheService;
        }

        // GET: Support
        public ActionResult Index()
        {
            var vendor = CacheService.Get<VendorInfoDetail>(CacheKeySetting.Vendor);
            if (vendor != null) return PartialView("../Support/Index", vendor);

            vendor = VendorService.GetDefaultVendor();
            CacheService.Set(CacheKeySetting.Vendor, vendor);
            return View("../Support/Index", vendor);
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
                    ContactService = null;
                    DocumentService = null;
                    VendorService = null;
                    CacheService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}