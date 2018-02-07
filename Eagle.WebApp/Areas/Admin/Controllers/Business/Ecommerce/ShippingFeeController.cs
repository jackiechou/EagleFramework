using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Business.Ecommerce
{
    public class ShippingFeeController : BaseController
    {
        private IShippingService ShippingService { get; set; }
        private ICurrencyService CurrencyService { get; set; }

        public ShippingFeeController(IShippingService shippingService, ICurrencyService currencyService) : base(new IBaseService[] { shippingService, currencyService })
        {
            ShippingService = shippingService;
            CurrencyService = currencyService;
        }
        #region GET METHODS =========================================================================

        // GET: Admin/ShippingFee
        public ActionResult Index()
        {
            return View("../Business/Ecommerce/Shipping/ShippingFee/Index");
        }

        // GET: /Admin/ShippingFee/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.CurrencyCode = CurrencyService.GetSelectedCurrency().CurrencyCode;
            ViewBag.ShippingCarrierId = ShippingService.PopulateShippingCarrierSelectList();
            ViewBag.ShippingMethodId = ShippingService.PopulateShippingMethodSelectList();
            return PartialView("../Business/Ecommerce/Shipping/ShippingFee/_Create");
        }

        // GET: /Admin/ShippingFee/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = ShippingService.GetShippingFeeDetail(id);

            var editModel = new ShippingFeeEditEntry
            {
                ShippingFeeId = entity.ShippingFeeId,
                ShippingCarrierId = entity.ShippingCarrierId,
                ShippingMethodId = entity.ShippingMethodId,
                ShippingFeeName = entity.ShippingFeeName,
                ZipStart = entity.ZipStart,
                ZipEnd = entity.ZipEnd,
                WeightStart = entity.WeightStart,
                WeightEnd = entity.WeightEnd,
                RateFee = entity.RateFee,
                PackageFee = entity.PackageFee,
                Vat = entity.Vat,
                CurrencyCode = entity.CurrencyCode,
                IsActive = entity.IsActive
            };

            ViewBag.ShippingCarrierId = ShippingService.PopulateShippingCarrierSelectList(null, entity.ShippingCarrierId);
            ViewBag.ShippingMethodId = ShippingService.PopulateShippingMethodSelectList(null, entity.ShippingMethodId);
            return PartialView("../Business/Ecommerce/Shipping/ShippingFee/_Edit", editModel);
        }

        // GET: /Admin/ShippingMethod/Search       
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            ViewBag.IsActive = ShippingService.PopulateShippingFeeStatus();
            return PartialView("../Business/Ecommerce/Shipping/ShippingFee/_Search");
        }

        // GET: /Admin/ShippingFee/Search
        [HttpGet]
        public ActionResult Search(ShippingFeeSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["ShippingFeeSearchRequest"] = filter;
            }
            else
            {
                if (TempData["ShippingFeeSearchRequest"] != null)
                {
                    filter = (ShippingFeeSearchEntry)TempData["ShippingFeeSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = ShippingService.GetShippingFees(filter, ref recordCount, null, page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Business/Ecommerce/Shipping/ShippingFee/_SearchResult", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/ShippingFee/Create
        [HttpPost]
        public ActionResult Create(ShippingFeeEntry entry)
        {
            try
            {       
                if (ModelState.IsValid)
                {
                    ShippingService.InsertShippingFee(entry);
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
        // PUT - /Admin/ShippingFee/Edit
        [HttpPut]
        public ActionResult Edit(ShippingFeeEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ShippingService.UpdateShippingFee(entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ShippingFeeId
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
        // POST: /Admin/ShippingFee/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                ShippingService.UpdateShippingFeeStatus(id, status);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/ShippingFee/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                ShippingService.UpdateShippingFeeStatus(id, false);
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
                    CurrencyService = null;
                    ShippingService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}