using System;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Booking;
using Eagle.Services.Service;
using Eagle.Services.Validations;
using Eagle.Services.Business;

namespace Eagle.WebApp.Controllers
{
    public class BookingController : BasicController
    {
        private IBookingService BookingService { get; set; }
        private ICurrencyService CurrencyService { get; set; }
        private ICustomerService CustomerService { get; set; }
        private IEmployeeService EmployeeService { get; set; }
        private ITransactionService TransactionService { get; set; }
        private IShippingService ShippingService { get; set; }
        public IOrderService OrderService { get; set; }

        public BookingController(IBookingService bookingService, IOrderService orderService, ICurrencyService currencyService, ICustomerService customerService,
            IEmployeeService employeeService, ITransactionService transactionService, IShippingService shippingService)
        {
            BookingService = bookingService;
            OrderService = orderService;
            CurrencyService = currencyService;
            CustomerService = customerService;
            EmployeeService = employeeService;
            ShippingService = shippingService;
            TransactionService = transactionService;
        }
        // GET: Booking
        public ActionResult Index()
        {
            return View("../Booking/Index");
        }
        
        // GET: /Booking/GetPromotion
        [HttpGet]
        public ActionResult GetPromotion(string code)
        {
            int vendorId = GlobalSettings.DefaultVendorId;
            var promotion = TransactionService.GetPromotionDetailByCode(vendorId, code) ?? new PromotionDetail
            {
                PromotionCode = code
            };
            return PartialView("../Booking/_Promotion", promotion);
        }


        /// <summary>
        ///  Step 1a :  Choose Service Type - Single Kind
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        public ActionResult LoadSingleService()
        {
            var singlePackage = new BookingSingleKindEntry
            {
                Capacity = 1,
                StartDate = DateTime.UtcNow,
                PeriodGroup = BookingPeriodGroup.Anytime,
                CurrencyCode = CurrencyService.GetSelectedCurrency().CurrencyCode
            };
            ViewBag.FromPeriod = BookingService.PopulatePeriod();
            return PartialView("../Booking/_SingleService", singlePackage);
        }


        /// <summary>
        ///  Choose Service Type - Full Package Kind
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LoadFullService()
        {
            var package = new BookingPackageKindEntry
            {
                CurrencyCode = CurrencyService.GetSelectedCurrency().CurrencyCode
            };
            ViewBag.FromPeriod = BookingService.PopulatePeriod();
            return PartialView("../Booking/_FullService", package);
        }


        /// <summary>
        /// Get Packages by Category and Package Type such as single or full
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PopulatePackagesByCategory(ServiceType typeId, int categoryId)
        {
            var lst = BookingService.GetServicePacks((int)typeId, categoryId, ServicePackStatus.Active);
            if (typeId == ServiceType.Single)
            {
                var jsonResult = Json(lst, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            else
            {
                //When typeId == ServicePackType.Full
                return PartialView("../Booking/_FullServicePackages", lst);
            }
        }

        [HttpGet]
        public ActionResult GetServicPackageDetail(int packageId)
        {
            var packages = BookingService.GetServicePackDetail(packageId);
            return Json(packages, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult PopulateEmployeesByPackage(int packageId)
        {
            var employees = BookingService.PopulateProviderSelectList(packageId, EmployeeStatus.Published);
            return Json(employees, JsonRequestBehavior.AllowGet);
        }


        // GET: Hierachical List 
        [HttpGet]
        public ActionResult GetServiceCategorySelectTree(ServiceType typeId = ServiceType.Single, int? selectedId = null, bool? isRootShowed = true)
        {
            var list = BookingService.GetServiceCategorySelectTree(typeId, null, selectedId, isRootShowed);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult BookSinglePackage(BookingSingleKindEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var applicationId = GlobalSettings.DefaultApplicationId;
                    BookingService.BookingSinglePackagesToCart(applicationId, entry);
                    return RedirectToAction("CreateBill", "CustOrder");
                }
                else
                {
                    var modeStateErrors = ModelState.GetModelStateErrors();
                    return View("../Booking/Index", modeStateErrors);
                }
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return View("../Booking/Index", errors);
            }
        }

        [HttpPost]
        public ActionResult BookingFullPackage(BookingPackageKindEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var applicationId = GlobalSettings.DefaultApplicationId;
                    BookingService.BookingFullPackagesToCart(applicationId, entry);
                    return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.CreateSuccess }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var modeStateErrors = ModelState.GetModelStateErrors();
                    return Json(new FailResult { Status = ResultStatusType.Fail, Errors = modeStateErrors }, JsonRequestBehavior.AllowGet);
                }
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
                    CurrencyService = null;
                    CustomerService = null;
                    EmployeeService = null;
                    TransactionService = null;
                    ShippingService = null;
                    OrderService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}