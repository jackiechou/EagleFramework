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
    public class ServicePackDurationController : BaseController
    {
        private IBookingService BookingService { get; set; }
        public ServicePackDurationController(IBookingService bookingService) : base(new IBaseService[] { bookingService })
        {
            BookingService = bookingService;
        }

        #region GET METHODS =========================================================================

        // GET: Admin/ServicePackDuration
        public ActionResult Index()
        {
            return View("../Services/Booking/ServicePackDuration/Index");
        }

        // GET: /Admin/ServicePackDuration/Create
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("../Services/Booking/ServicePackDuration/_Create");
        }

        //GET: /Admin/ServicePackDuration/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = BookingService.GetServicePackDurationDetail(id);

            var editModel = new ServicePackDurationEditEntry
            {
                DurationId = entity.DurationId,
                DurationName = entity.DurationName,
                AllotedTime = entity.AllotedTime,
                Unit = entity.Unit,
                Description = entity.Description,
                IsActive = entity.IsActive
            };
            return PartialView("../Services/Booking/ServicePackDuration/_Edit", editModel);
        }

        // GET: /Admin/ServicePackDuration/Search       
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            ViewBag.Status = BookingService.PopulateServicePackDurationStatus(null, true);
            return PartialView("../Services/Booking/ServicePackDuration/_SearchForm");
        }

        //GET: /Admin/ServicePackDuration/Search
        [HttpGet]
        public ActionResult Search(ServicePackDurationSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["ServicePackDurationSearchRequest"] = filter;
            }
            else
            {
                if (TempData["ServicePackDurationSearchRequest"] != null)
                {
                    filter = (ServicePackDurationSearchEntry)TempData["ServicePackDurationSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = BookingService.GetServicePackDurations(filter, ref recordCount, "DurationId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Services/Booking/ServicePackDuration/_SearchResult", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/ServicePackDuration/Create
        [HttpPost]
        public ActionResult Create(ServicePackDurationEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BookingService.InsertServicePackDuration(entry);
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


        // PUT - /Admin/ServicePackDuration/Edit
        [HttpPut]
        public ActionResult Edit(ServicePackDurationEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BookingService.UpdateServicePackDuration(entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.DurationId
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


        // POST: /Admin/ServicePackDuration/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                BookingService.UpdateServicePackDurationStatus(id, status);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }


        //DELETE: /Admin/ServicePackDuration/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                BookingService.UpdateServicePackDurationStatus(id, false);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.DeleteSuccess }, JsonRequestBehavior.AllowGet);
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