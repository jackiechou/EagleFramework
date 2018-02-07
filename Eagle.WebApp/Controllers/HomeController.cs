using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Services.Booking;
using Eagle.Services.Dtos.Services.Event;
using Eagle.Services.Service;
using Eagle.Services.SystemManagement;

namespace Eagle.WebApp.Controllers
{
    public class HomeController : BasicController
    {
        private IApplicationService ApplicationService { get; set; }
        private IBookingService BookingService { get; set; }
        private IProductService ProductService { get; set; }
        private IContactService ContactService { get; set; }
        private IDocumentService DocumentService { get; set; }
        private IVendorService VendorService { get; set; }
        private ICacheService CacheService { get; set; }
        private IEventService EventService { get; set; }

        public HomeController(IApplicationService applicationService, IEventService eventService, IProductService productService,
            IBookingService bookingService, IVendorService vendorService, IContactService contactService, 
            IDocumentService documentService, ICacheService cacheService)
        {
            ApplicationService = applicationService;
            EventService = eventService;
            BookingService = bookingService;
            ProductService = productService;
            VendorService = vendorService;
            ContactService = contactService;
            DocumentService = documentService;
            CacheService = cacheService;
        }

        public ActionResult Index()
        {  
            return View("../Home/Index");
        }

        public ActionResult Welcome()
        {
            var vendor = CacheService.Get<VendorInfoDetail>(CacheKeySetting.Vendor);
            if (vendor != null) return PartialView("../Home/_Welcome", vendor);

            vendor = VendorService.GetDefaultVendor();
            CacheService.Set(CacheKeySetting.Vendor, vendor);
            return PartialView("../Home/_Welcome", vendor);
        }

        [HttpGet]
        public ActionResult GetDiscountedProducts(int? categoryId = null, int? page = 1, int pageSize = 5)
        {
            var vendor = CacheService.Get<VendorInfoDetail>(CacheKeySetting.Vendor);
            if (vendor == null)
            {
                vendor = VendorService.GetDefaultVendor();
                CacheService.Set(CacheKeySetting.Vendor, vendor);
            }
            
            int vendorId = vendor.VendorId;
            int? recordCount = 0;
            var sources = ProductService.GetDiscountedProducts(vendorId, categoryId, ProductStatus.Published, ref recordCount, "ProductId DESC", page, pageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, pageSize, recordCount);
            return PartialView("../Home/_DiscountedProducts", lst);
        }

        [HttpGet]
        public ActionResult GetDiscountedProductListImage(int? categoryId = null, int? page = 1, int pageSize = 4)
        {
            var vendor = CacheService.Get<VendorInfoDetail>(CacheKeySetting.Vendor);
            if (vendor == null)
            {
                vendor = VendorService.GetDefaultVendor();
                CacheService.Set(CacheKeySetting.Vendor, vendor);
            }

            int vendorId = vendor.VendorId;
            int? recordCount = 0;
            var sources = ProductService.GetDiscountedProducts(vendorId, categoryId, ProductStatus.Published, ref recordCount, "ProductId DESC", page, pageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, pageSize, recordCount);
            return PartialView("../Home/_DiscountedProductListImage", lst);
        }

        [HttpGet]
        public ActionResult GetDiscountedProductVertialSlide(int? categoryId = null, int? page = 1, int pageSize = 5)
        {
            var vendor = CacheService.Get<VendorInfoDetail>(CacheKeySetting.Vendor);
            if (vendor == null)
            {
                vendor = VendorService.GetDefaultVendor();
                CacheService.Set(CacheKeySetting.Vendor, vendor);
            }

            int vendorId = vendor.VendorId;
            int? recordCount = 0;
            var sources = ProductService.GetDiscountedProducts(vendorId, categoryId, ProductStatus.Published, ref recordCount, "ProductId DESC", page, pageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, pageSize, recordCount);
            return PartialView("../Home/_DiscountedProductVertialSlide", lst);
        }

        [HttpGet]
        public ActionResult GetDiscountedPackageTicker(int? page = 1)
        {
            int? recordCount = 0;
            var sources = BookingService.GetDiscountedServicePackages(null, ServicePackStatus.Active, ref recordCount, "ListOrder DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Home/_DiscountedServicePackageTicker", lst);
        }


        [HttpGet]
        public ActionResult GetLastestProducts(int? categoryId = null, int? page = 1, int? pageSize = 6)
        {
            var vendor = CacheService.Get<VendorInfoDetail>(CacheKeySetting.Vendor);
            if (vendor == null)
            {
                vendor = VendorService.GetDefaultVendor();
                CacheService.Set(CacheKeySetting.Vendor, vendor);
            }

            int vendorId = vendor.VendorId;
            int? recordCount = 0;
            var sources = ProductService.GetLastestProducts(vendorId, categoryId, ProductStatus.Published, ref recordCount, "ProductName DESC", page, pageSize ?? GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, pageSize ?? GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Home/_LastestProducts", lst);
        }

        [HttpGet]
        public ActionResult GetLastestServices(int? page = 1, int? pageSize = 10)
        {
            int? recordCount = 0;
            ServicePackSearchEntry filter = new ServicePackSearchEntry
            {
                ServicePackName = null,
                CategoryId = null,
                ServiceType = null,
                Status = ServicePackStatus.Active
            };
            var sources = BookingService.GetServicePacks(filter, ref recordCount, "ListOrder DESC", page, pageSize ?? GlobalSettings.DefaultPageSizeByLastest);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, pageSize ?? GlobalSettings.DefaultPageSizeByLastest, recordCount);
            return PartialView("../Home/_LastestServices", lst);
        }

        // GET: VendorInfo
        public ActionResult GetVendorInfo()
        {
            // var vendor = CacheService.Get<VendorInfoDetail>(CacheKeySetting.Vendor);
            //if (vendor != null) return PartialView("../Home/_AboutUs", vendor);

            var vendor = VendorService.GetDefaultVendor();
            CacheService.Set(CacheKeySetting.Vendor, vendor);
            return PartialView("../Home/_AboutUs", vendor);
        }

        [HttpGet]
        public ActionResult GetLatestEvents(EventSearchEntry filter, int? page = 1, int? pageSize = 10)
        {
            int recordCount;
            var sources = EventService.Search(filter, out recordCount, "EventId DESC", page, pageSize ?? GlobalSettings.DefaultPageSizeEvent);
            int currentPageIndex = page - 1 ?? 0;
            var pageLst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSizeEvent, recordCount);
            return PartialView("../Home/_LastestEvent", pageLst);
        }

        [HttpGet]
        public ActionResult GetServiceCategories()
        {
            return PartialView("../Home/_ServiceCategories");
        }

        [HttpGet]
        public ActionResult GetServiceCategoriesBySingleType()
        {
            var list = BookingService.GetServiceCategorySelectTree(ServiceType.Single, ServiceCategoryStatus.Active, null, false);
            return PartialView("../Home/_ServiceCategoriesBySingleType", list);
        }

        [HttpGet]
        public ActionResult GetServiceCategoriesByFullType()
        {
            var list = BookingService.GetServiceCategorySelectTree(ServiceType.Full, ServiceCategoryStatus.Active, null, false);
            return PartialView("../Home/_ServiceCategoriesByFullType", list);
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
                    ApplicationService = null;
                    BookingService = null;
                    ProductService = null;
                    ContactService = null;
                    DocumentService = null;
                    VendorService = null;
                    CacheService = null;
                    EventService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}