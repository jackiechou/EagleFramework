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
    public class ShippingMethodController : BaseController
    {
        private IShippingService ShippingService { get; set; }

        public ShippingMethodController(IShippingService shippingService) : base(new IBaseService[] { shippingService })
        {
            ShippingService = shippingService;
        }

        #region GET METHODS =========================================================================

        // GET: Admin/ShippingMethod
        public ActionResult Index()
        {
            return View("../Business/Ecommerce/Shipping/ShippingMethod/Index");
        }

        // GET: /Admin/ShippingMethod/Create
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("../Business/Ecommerce/Shipping/ShippingMethod/_Create");
        }

        // GET: /Admin/ShippingMethod/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = ShippingService.GetShippingMethodDetail(id);

            var editModel = new ShippingMethodEditEntry
            {
                ShippingMethodId = entity.ShippingMethodId,
                ShippingMethodName = entity.ShippingMethodName,
                IsActive = entity.IsActive
            };
            return PartialView("../Business/Ecommerce/Shipping/ShippingMethod/_Edit", editModel);
        }

        // GET: /Admin/ShippingMethod/Search       
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            ViewBag.IsActive = ShippingService.PopulateShippingMethodStatus();
            return PartialView("../Business/Ecommerce/Shipping/ShippingMethod/_Search");
        }

        // GET: /Admin/ShippingMethod/Search
        [HttpGet]
        public ActionResult Search(ShippingMethodSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["ShippingMethodSearchRequest"] = filter;
            }
            else
            {
                if (TempData["ShippingMethodSearchRequest"] != null)
                {
                    filter = (ShippingMethodSearchEntry)TempData["ShippingMethodSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = ShippingService.GetShippingMethods(filter, ref recordCount, null, page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Business/Ecommerce/Shipping/ShippingMethod/_SearchResult", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/ShippingMethod/Create
        [HttpPost]
        public ActionResult Create(ShippingMethodEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ShippingService.InsertShippingMethod(entry);
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
        // PUT - /Admin/ShippingMethod/Edit
        [HttpPut]
        public ActionResult Edit(ShippingMethodEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ShippingService.UpdateShippingMethod(entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ShippingMethodId
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
        // POST: /Admin/ShippingMethod/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                ShippingService.UpdateShippingMethodStatus(id, status);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/ShippingMethod/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                ShippingService.UpdateShippingMethodStatus(id, false);
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