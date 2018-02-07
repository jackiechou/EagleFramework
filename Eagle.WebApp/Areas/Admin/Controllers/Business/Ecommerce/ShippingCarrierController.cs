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
    public class ShippingCarrierController : BaseController
    {
        private IShippingService ShippingService { get; set; }

        public ShippingCarrierController(IShippingService shippingService) : base(new IBaseService[] { shippingService })
        {
            ShippingService = shippingService;
        }
       
        #region GET METHODS =========================================================================

        // GET: Admin/ShippingCarrier
        public ActionResult Index()
        {
            return View("../Business/Ecommerce/Shipping/ShippingCarrier/Index");
        }

        // GET: /Admin/ShippingCarrier/Create
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("../Business/Ecommerce/Shipping/ShippingCarrier/_Create");
        }

        // GET: /Admin/ShippingCarrier/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = ShippingService.GetShippingCarrierDetail(id);

            var editModel = new ShippingCarrierEditEntry
            {
                ShippingCarrierId = entity.ShippingCarrierId,
                ShippingCarrierName = entity.ShippingCarrierName,
                Phone = entity.Phone,
                Address = entity.Address,
                IsActive = entity.IsActive
            };
            return PartialView("../Business/Ecommerce/Shipping/ShippingCarrier/_Edit", editModel);
        }

        // GET: /Admin/ShippingCarrier/Search       
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            ViewBag.IsActive = ShippingService.PopulateShippingCarrierStatus();
            return PartialView("../Business/Ecommerce/Shipping/ShippingCarrier/_Search");
        }

        // GET: /Admin/ShippingCarrier/Search
        [HttpGet]
        public ActionResult Search(ShippingCarrierSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["ShippingCarrierSearchRequest"] = filter;
            }
            else
            {
                if (TempData["ShippingCarrierSearchRequest"] != null)
                {
                    filter = (ShippingCarrierSearchEntry)TempData["ShippingCarrierSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = ShippingService.GetShippingCarriers(VendorId, filter, ref recordCount, null, page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Business/Ecommerce/Shipping/ShippingCarrier/_SearchResult", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/ShippingCarrier/Create
        [HttpPost]
        public ActionResult Create(ShippingCarrierEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ShippingService.InsertShippingCarrier(VendorId, entry);
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
        // PUT - /Admin/ShippingCarrier/Edit
        [HttpPut]
        public ActionResult Edit(ShippingCarrierEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ShippingService.UpdateShippingCarrier(VendorId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ShippingCarrierId
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
        // POST: /Admin/ShippingCarrier/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                ShippingService.UpdateShippingCarrierStatus(id, status);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/ShippingCarrier/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                ShippingService.UpdateShippingCarrierStatus(id, false);
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
                    ShippingService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}