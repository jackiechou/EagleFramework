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
    public class ServiceTaxRateController : BaseController
    {
        private IBookingService BookingService { get; set; }

        public ServiceTaxRateController(IBookingService bookingService) : base(new IBaseService[] { bookingService })
        {
            BookingService = bookingService;
        }
        #region GET METHODS =========================================================================
        //
        // GET: /Admin/ServiceTaxRate/
        public ActionResult Index()
        {
            return View("../Services/Booking/ServiceTaxRate/Index");
        }

        // GET: /Admin/ServiceTaxRate/Create
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("../Services/Booking/ServiceTaxRate/_Create");
        }

        // GET: /Admin/ServiceTaxRate/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = new ServiceTaxRateEditEntry();
            var item = BookingService.GetServiceTaxRateDetails(id);
            if (item != null)
            {
                model.TaxRateId = item.TaxRateId;
                model.TaxRate = item.TaxRate;
                model.IsPercent = item.IsPercent;
                model.Description = item.Description;
                model.IsActive = item.IsActive;
            }
            return PartialView("../Services/Booking/ServiceTaxRate/_Edit", model);
        }

        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            return PartialView("../Services/Booking/ServiceTaxRate/_SearchForm");
        }

        // GET: /Admin/ServiceTaxRate/List
        [HttpGet]
        public ActionResult Search(ServiceTaxRateSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["ServiceTaxRateSearchRequest"] = filter;
            }
            else
            {
                if (TempData["ServiceTaxRateSearchRequest"] != null)
                {
                    filter = (ServiceTaxRateSearchEntry)TempData["ServiceTaxRateSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = BookingService.GeServiceTaxRates(filter, ref recordCount, "TaxRateId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Services/Booking/ServiceTaxRate/_SearchResult", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/ServiceTaxRate/Create
        [HttpPost]
        public ActionResult Create(ServiceTaxRateEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BookingService.InsertServiceTaxRate(entry);
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

        //
        // PUT - /Admin/ServiceTaxRate/Edit
        [HttpPut]
        public ActionResult Edit(ServiceTaxRateEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BookingService.UpdateServiceTaxRate(entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.TaxRateId
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

        //
        // POST: /Admin/ServiceTaxRate/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                BookingService.UpdateServiceTaxRateStatus(id, status);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/ServiceTaxRate/Delete/5
        [HttpDelete]
        public ActionResult Delete([System.Web.Http.FromBody]int id)
        {
            try
            {
                BookingService.UpdateServiceTaxRateStatus(id, false);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.DeleteSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion ==============================================================================
        
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