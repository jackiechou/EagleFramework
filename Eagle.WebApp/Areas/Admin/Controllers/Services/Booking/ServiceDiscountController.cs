using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Booking;
using Eagle.Services.Service;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Services.Booking
{
    public class ServiceDiscountController : BaseController
    {
        private IBookingService BookingService { get; set; }

        public ServiceDiscountController(IBookingService bookingService) : base(new IBaseService[] { bookingService })
        {
            BookingService = bookingService;
        }

        #region GET METHODS =========================================================================
        //
        // GET: /Admin/ServiceDiscount/
        public ActionResult Index()
        {
            return View("../Services/Booking/ServiceDiscount/Index");
        }

        // GET: /Admin/ServiceDiscount/Create
        [HttpGet]
        public ActionResult Create()
        {
            var model = new ServiceDiscountEntry();
            return PartialView("../Services/Booking/ServiceDiscount/_Create", model);
        }

        // GET: /Admin/ServiceDiscount/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = new ServiceDiscountEditEntry();
            var item = BookingService.GetServiceDiscountDetails(id);
            if (item != null)
            {
                model.DiscountId = item.DiscountId;
                model.DiscountCode = item.DiscountCode;
                model.DiscountType = item.DiscountType;
                model.Quantity = item.Quantity;
                model.DiscountRate = item.DiscountRate;
                model.IsPercent = item.IsPercent;
                model.Description = item.Description;
                model.StartDate = item.StartDate;
                model.EndDate = item.EndDate;
                model.IsActive = item.IsActive;

                //if (item.StartDate != null)
                //{
                //    DateTime startDate = Convert.ToDateTime(item.StartDate);
                //    model.StartDate = startDate.ToString("MM/dd/yyyy");
                //}

                //if (item.EndDate != null)
                //{
                //    DateTime endDate = Convert.ToDateTime(item.EndDate);
                //    model.EndDate = endDate.ToString("MM/dd/yyyy");
                //}
            }
            return PartialView("../Services/Booking/ServiceDiscount/_Edit", model);
        }

        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            return PartialView("../Services/Booking/ServiceDiscount/_SearchForm");
        }

        // GET: /Admin/ServiceDiscount/List
        [HttpGet]
        public ActionResult Search(ServiceDiscountSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["ServiceDiscountSearchRequest"] = filter;
            }
            else
            {
                if (TempData["ServiceDiscountSearchRequest"] != null)
                {
                    filter = (ServiceDiscountSearchEntry)TempData["ServiceDiscountSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = BookingService.GeServiceDiscounts(filter, ref recordCount, "DiscountId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Services/Booking/ServiceDiscount/_SearchResult", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/ServiceDiscount/Create
        [HttpPost]
        public ActionResult Create(ServiceDiscountEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BookingService.InsertServiceDiscount(entry);
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
        // PUT - /Admin/ServiceDiscount/Edit
        [HttpPut]
        public ActionResult Edit(ServiceDiscountEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BookingService.UpdateServiceDiscount(entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.DiscountId
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
        // POST: /Admin/ServiceDiscount/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                BookingService.UpdateServiceDiscountStatus(id, status ? ServiceDiscountStatus.Active : ServiceDiscountStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/ServiceDiscount/Delete/5
        [HttpDelete]
        public ActionResult Delete([System.Web.Http.FromBody]int id)
        {
            try
            {
                BookingService.UpdateServiceDiscountStatus(id, ServiceDiscountStatus.InActive);
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