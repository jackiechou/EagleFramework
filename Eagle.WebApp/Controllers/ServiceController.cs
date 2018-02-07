using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Service;
using Eagle.Services.Dtos.Services.Booking;
using Eagle.Services.Validations;

namespace Eagle.WebApp.Controllers
{
    public class ServiceController : Controller
    {
        private IBookingService BookingService { get; set; }
        private ICompanyService CompanyService { get; set; }

        public ServiceController(IBookingService bookingService, ICompanyService companyService)
        {
            CompanyService = companyService;
            BookingService = bookingService;
        }
        
        // GET: Service
        public ActionResult Index()
        {
            return View("../Services/Index");
        }

        [HttpGet]
        public ActionResult GetServicePacks([System.Web.Http.FromUri]int categoryId, int? page = 1, int? pageSize = 10, bool? noPaging = false)
        {
            ViewBag.CategoryId = categoryId;
            ViewBag.NoPaging = noPaging;
            int? recordCount = 0;
            var sources = BookingService.GetServicePacks(categoryId, ServicePackStatus.Active, ref recordCount, "ListOrder DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, pageSize ?? GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Services/_ServicePacks", lst);
        }

        [HttpGet]
        public ActionResult GetServicesByCategoryId(int categoryId, int? page = 1, int? pageSize = 10)
        {
            ViewBag.CategoryId = categoryId;

            int? recordCount = 0;
            var sources = BookingService.GetServicePacks(categoryId, ServicePackStatus.Active, ref recordCount, "PackageId DESC", page, pageSize);
            int currentPageIndex = page - 1 ?? 0;
            var pageLst = sources.ToPagedList(currentPageIndex, pageSize ?? GlobalSettings.DefaultPageSize, recordCount);
            return View("../Services/List", pageLst);
        }
        
        [HttpGet]
        public ActionResult GetDiscountedPackages(int? page = 1)
        {
            int? recordCount = 0;
            var sources = BookingService.GetDiscountedServicePackages(null, ServicePackStatus.Active, ref recordCount, "ListOrder DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Home/_DiscountedServicePacks", lst);
        }

        //
        // GET: /ServicePackInfo/Detail/5        
        [HttpGet]
        public ActionResult Details(int id)
        {
            var item = BookingService.GetServicePackDetail(id);
            return View("../Services/Detail", item);
        }

        //
        // GET: /ServicePackInfo/GetDetails/5        
        [HttpGet]
        public ActionResult GetDetails(int id)
        {
            var item = BookingService.GetServicePackDetail(id);
            return PartialView("../Services/_ServicePackDetail", item);
        }
        [HttpGet]
        public ActionResult ShowAddCartDialog(int id)
        {
            var item = BookingService.GetServicePackDetail(id);
            return PartialView("../Services/_ServicePackDetail", item);
        }
        [HttpGet]
        public ActionResult GetServicePacksByCategory(int categoryId, int? page = 1, int? pageSize = 10, bool? noPaging = false)
        {
            ViewBag.CategoryId = categoryId;
            ViewBag.NoPaging = noPaging;
            int? recordCount = 0;
            var sources = BookingService.GetServicePacks(categoryId, ServicePackStatus.Active, ref recordCount, "ListOrder DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, pageSize ?? GlobalSettings.DefaultPageSize, recordCount);
            return View("../Services/ServicePacksByCategory", lst);
        }      

        [HttpGet]
        public ActionResult GetServiceCategoryTree(ServiceType typeId = ServiceType.Single, int? selectedId = null, bool? isRootShowed = false)
        {
            var list = BookingService.GetServiceCategorySelectTree(typeId, ServiceCategoryStatus.Active, selectedId, isRootShowed);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetLastestServices(int? categoryId = null, int? page = 1, int? pageSize = 10)
        {
            ViewBag.CategoryId = categoryId;

            int? recordCount = 0;
            ServicePackSearchEntry filter = new ServicePackSearchEntry
            {
                ServicePackName = null,
                CategoryId = categoryId,
                ServiceType = null,
                Status = ServicePackStatus.Active
            };

            var sources = BookingService.GetServicePacks(filter, ref recordCount, "ListOrder DESC", page, pageSize ?? GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, pageSize ?? GlobalSettings.DefaultPageSize, recordCount);

            return PartialView("../Services/_ServiceLastest", lst);
        }

        [HttpGet]
        public ActionResult GetLastestServicesRight(int? page = 1, int? pageSize = 10)
        {
            int? recordCount = 0;
            ServicePackSearchEntry filter = new ServicePackSearchEntry
            {
                ServicePackName = null,
                CategoryId = null,
                ServiceType = null,
                Status = ServicePackStatus.Active
            };
            var sources = BookingService.GetServicePacks(filter, ref recordCount, "ListOrder DESC", page, pageSize ?? GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, pageSize ?? GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Services/_LastestPackagesRight", lst);
        }

        [HttpGet]
        public ActionResult GetLatestServicesOnFooter(int? page = 1, int? pageSize = 2)
        {
            int? recordCount = 0;
            ServicePackSearchEntry filter = new ServicePackSearchEntry
            {
                ServicePackName = null,
                CategoryId = null,
                ServiceType = null,
                Status = ServicePackStatus.Active
            };
            var sources = BookingService.GetServicePacks(filter, ref recordCount, "ListOrder DESC", page, pageSize ?? GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, pageSize ?? GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Services/_LatestServicesOnFooter", lst);
        }

        [HttpGet]
        public ActionResult GetRelatedServices(int servicePackageId, int? page = 1, int? pageSize = 10)
        {
            ViewBag.SevicePackageId = servicePackageId;
            var item = BookingService.GetServicePackDetail(servicePackageId);
            if (item == null)
            {
                return PartialView("../Services/_RelatedServices");
            }
            else
            {
                int? recordCount = 0;
                ServicePackSearchEntry filter = new ServicePackSearchEntry
                {
                    ServicePackName = null,
                    CategoryId = item.CategoryId,
                    ServiceType = null,
                    Status = ServicePackStatus.Active
                };

                var sources = BookingService.GetServicePacks(filter, ref recordCount, "ListOrder DESC", page,
                    pageSize ?? GlobalSettings.DefaultPageSize).ToList();
                int currentPageIndex = page - 1 ?? 0;
                if (sources.Any())
                {
                    sources = sources.Where(x => x.PackageId != servicePackageId).ToList();
                }
                var lst = sources.ToPagedList(currentPageIndex, pageSize ?? GlobalSettings.DefaultPageSize,
                    recordCount);
                return PartialView("../Services/_RelatedServices", lst);
            }
        }

        
        [HttpGet]
        public ActionResult LoadCartInfo()
        {
            var cart = ShoppingCart.Instance;
           // BookingService.ValidateCart(ShoppingCart.Instance);
            return PartialView("../Services/_CartInfo", cart);
        }


        #region TABS 

        ////[HttpGet]
        ////public ActionResult GeServicePackage(ServiceType typeId = ServiceType.Single, int? selectedId = null, bool? isRootShowed = false)
        ////{
        ////    var list = BookingService.GetServiceCategorySelectTree(typeId, ServiceCategoryStatus.Active, selectedId, isRootShowed);
        ////    return PartialView("../Services/Tabs/_ServicePackageTabs", list);
        ////}

        //[HttpGet]
        //public ActionResult GeServicePackageTabsByCategoryId(int categoryId, int? page = 1, int? pageSize = 10)
        //{
        //    int? recordCount=0;
        //    var sources = BookingService.GetServicePacks(categoryId, ServicePackStatus.Active, ref recordCount, "PackageId DESC", page, pageSize);
        //    int currentPageIndex = page - 1 ?? 0;
        //    var pageLst = sources.ToPagedList(currentPageIndex, pageSize ?? 10, recordCount);
        //    return PartialView("../Services/Tabs/_ServicesByCategory", pageLst);
        //}

        //[HttpGet]
        //public ActionResult GeServiceCategories(ServiceType typeId = ServiceType.Single, int? selectedId=null, bool? isRootShowed = false)
        //{
        //    var list = BookingService.GetServiceCategorySelectTree(typeId, ServiceCategoryStatus.Active, selectedId, isRootShowed);
        //    return PartialView("../Services/Tabs/_CategoryTabs", list);
        //}

        [HttpGet]
        public ActionResult GetServicePackagesByType(ServiceType typeId = ServiceType.Single, int? selectedId = null, bool? isRootShowed = false)
        {
            var list = BookingService.GetServiceCategorySelectTree(typeId, ServiceCategoryStatus.Active, selectedId, isRootShowed);
            return PartialView("../Services/_ServicePackagesByType", list);
        }

        [HttpGet]
        public ActionResult GeServicePackageTabContentsByCategoryId(int categoryId, int? page = 1, int? pageSize = 10)
        {
            int? recordCount = 0;
            var sources = BookingService.GetServicePacks(categoryId, ServicePackStatus.Active, ref recordCount, "PackageId DESC", page, pageSize);
            int currentPageIndex = page - 1 ?? 0;
            var pageLst = sources.ToPagedList(currentPageIndex, pageSize ?? 10, recordCount);
            return PartialView("../Services/Tabs/_TabContents", pageLst);
        }
        
        #endregion
        
        #region RATINGS

        [HttpPost]
        public ActionResult CreateServicePackRating(int? customerId, int id, int rate)
        {
            try
            {
                var rateEntry = new ServicePackRatingEntry
                {
                    CustomerId = null,
                    PackageId = id,
                    Rate = rate,
                    TotalRates = BookingService.GetDefaultServicePackRating(GlobalSettings.DefaultApplicationId)
                };
                var averageRates = BookingService.InsertServicePackRating(rateEntry);
                return Json(new SuccessResult
                {
                    Status = ResultStatusType.Success,
                    Data = new
                    {
                        AverageRates = averageRates,
                        Message = LanguageResource.CreateSuccess
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        [HttpPut]
        public ActionResult UpdateTotalViews(int id)
        {
            try
            {
                BookingService.UpdateServicePackTotalViews(id);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateNewTotalViewSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
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
                    BookingService = null;
                    CompanyService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}