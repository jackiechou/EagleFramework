using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Settings;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Services.Booking;
using Eagle.Services.Service;
using Eagle.Services.SystemManagement;

namespace Eagle.WebApp.Controllers
{
    public class AboutUsController : BasicController
    {
        private ICompanyService CompanyService { get; set; }
        private IContactService ContactService { get; set; }
        private IDocumentService DocumentService { get; set; }
        private IVendorService VendorService { get; set; }
        private IBookingService BookingService { get; set; }
        private ICacheService CacheService { get; set; }

        public AboutUsController(IVendorService vendorService, IBookingService bookingService, 
            ICompanyService companyService, IContactService contactService, IDocumentService documentService,
            ICacheService cacheService)
        {
            CompanyService = companyService;
            ContactService = contactService;
            DocumentService = documentService;
            VendorService = vendorService;
            BookingService = bookingService;
            CacheService = cacheService;
        }
    
        // GET: AboutUs
        public ActionResult Index()
        {
            return View("../AboutUs/Index");
        }

        // GET: VendorInfo
        public ActionResult GetVendorInfo()
        {
            var vendor = CacheService.Get<VendorInfoDetail>(CacheKeySetting.Vendor);
            if (vendor != null) return PartialView("../AboutUs/_AboutUs", vendor);

            vendor = VendorService.GetDefaultVendor();
            CacheService.Set(CacheKeySetting.Vendor, vendor);
            return PartialView("../AboutUs/_AboutUs", vendor);
        }

        // GET: Service Packages
        public ActionResult GetServicePackages()
        {
            var filter = new ServicePackSearchEntry
            {
                ServicePackName = null,
                ServiceType = null,
                Status = ServicePackStatus.Active
            };

            int? recordCount = 0, page = 1;
            var lst = BookingService.GetServicePacks(filter, ref recordCount, "ListOrder DESC", page, GlobalSettings.DefaultPageSize);
            return PartialView("../AboutUs/_ServicePackage", lst);
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
                    CompanyService = null;
                    ContactService = null;
                    DocumentService = null;
                    VendorService = null;
                    BookingService = null;
                    CacheService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}