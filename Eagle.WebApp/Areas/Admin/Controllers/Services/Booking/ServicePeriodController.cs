using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Booking;
using Eagle.Services.Service;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Services.Booking
{
    public class ServicePeriodController : BaseController
    {
        private IBookingService BookingService { get; set; }

        public ServicePeriodController(IBookingService bookingService) : base(new IBaseService[] { bookingService })
        {
            BookingService = bookingService;
        }
        #region GET METHODS =========================================================================

        // GET: Admin/ServicePeriod
        public ActionResult Index()
        {
            return View("../Services/Booking/ServicePeriod/Index");
        }

        // GET: /Admin/ServicePeriod/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Status = BookingService.PopulateServicePeriodStatus();
            return PartialView("../Services/Booking/ServicePeriod/_Create");
        }

        //GET: /Admin/ServicePeriod/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = BookingService.GetServicePeriodDetail(id);

            var editModel = new ServicePeriodEditEntry
            {
                PeriodId = entity.PeriodId,
                PeriodName = entity.PeriodName,
                PeriodValue = entity.PeriodValue,
                Status = entity.Status
            };
            ViewBag.Status = BookingService.PopulateServicePeriodStatus(entity.Status);
            return PartialView("../Services/Booking/ServicePeriod/_Edit", editModel);
        }

        // GET: /Admin/ServicePeriod/Search       
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            ViewBag.Status = BookingService.PopulateServicePeriodStatus(null, true);
            return PartialView("../Services/Booking/ServicePeriod/_SearchForm");
        }

        //GET: /Admin/ServicePeriod/Search
        //[HttpGet]
        //public ActionResult Search(int? page = 1)
        //{
        //    int? recordCount = 0;
        //    var sources = BookingService.GetServicePeriods(null, ref recordCount, null, page, GlobalSettings.DefaultPageSize);
        //    int currentPageIndex = page - 1 ?? 0;
        //    var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
        //    return PartialView("../Services/Booking/ServicePeriod/_SearchResult", lst);
        //}

        [HttpGet]
        public ActionResult Search(ServicePeriodSearchEntry entry, string sourceEvent, int? page)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["ServicePeriodSearchRequest"] = entry;
            }
            else
            {
                if (TempData["ServicePeriodSearchRequest"] != null)
                {
                    entry = (ServicePeriodSearchEntry)TempData["ServicePeriodSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = BookingService.GetServicePeriods(entry.Status, ref recordCount, null, page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Services/Booking/ServicePeriod/_SearchResult", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/ServicePeriod/Create
        [HttpPost]
        public ActionResult Create(ServicePeriodEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BookingService.InsertServicePeriod(entry);
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


        // PUT - /Admin/ServicePeriod/Edit
        [HttpPut]
        public ActionResult Edit(ServicePeriodEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BookingService.UpdateServicePeriod(entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.PeriodId
                        }
                    }, JsonRequestBehavior.AllowGet);
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


        // POST: /Admin/ServicePeriod/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                BookingService.UpdateServicePeriodStatus(id, status);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion =====================================================================================
        
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
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}