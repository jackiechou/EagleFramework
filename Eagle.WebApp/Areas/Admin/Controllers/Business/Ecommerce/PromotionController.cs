using System;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Business.Ecommerce
{
    public class PromotionController : BaseController
    {
        private ITransactionService TransactionService { get; set; }
        public PromotionController(ITransactionService transactionService) : base(new IBaseService[] { transactionService })
        {
            TransactionService = transactionService;
        }
        #region GET METHODS =========================================================================
        //
        // GET: /Admin/Promotion/
        public ActionResult Index()
        {
            return View("../Business/Ecommerce/Transaction/Promotion/Index");
        }

        [HttpGet]
        public ActionResult GeneratePromotionCode()
        {
            var customerNo = TransactionService.GeneratePromotionCode(15);
            return Json(customerNo);
        }


        // GET: /Admin/Promotion/Create     
        [HttpGet]
        public ActionResult Create()
        {
            var promotionCode = TransactionService.GeneratePromotionCode(10);
            var entry = new PromotionEntry
            {
                PromotionType = PromotionType.Coupon,
                PromotionCode = promotionCode,
                StartDate  = DateTime.UtcNow
            };
            return PartialView("../Business/Ecommerce/Transaction/Promotion/_Create", entry);
        }

        // GET: /Admin/Promotion/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = TransactionService.GetPromotionDetail(id);

            var editModel = new PromotionEditEntry
            {
                PromotionId = entity.PromotionId,
                PromotionType = entity.PromotionType,
                PromotionCode = entity.PromotionCode,
                PromotionTitle = entity.PromotionTitle,
                PromotionValue = entity.PromotionValue,
                IsPercent = entity.IsPercent,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Description = entity.Description,
                IsActive = entity.IsActive
            };
            return PartialView("../Business/Ecommerce/Transaction/Promotion/_Edit", editModel);
        }

        // GET: /Admin/Promotion/Search
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            return PartialView("../Business/Ecommerce/Transaction/Promotion/_SearchForm");
        }

        // GET: /Admin/Promotion/Search
        [HttpGet]
        public ActionResult Search(PromotionSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (String.IsNullOrEmpty(sourceEvent))
            {
                TempData["PromotionSearchRequest"] = filter;
            }
            else
            {
                if (TempData["PromotionSearchRequest"] != null)
                {
                    filter = (PromotionSearchEntry)TempData["PromotionSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = TransactionService.GetPromotions(VendorId, filter, ref recordCount, "PromotionId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Business/Ecommerce/Transaction/Promotion/_SearchResult", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/Promotion/Create
        [HttpPost]
        public ActionResult Create(PromotionEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TransactionService.InsertPromotion(UserId, VendorId, entry);
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
        // PUT - /Admin/Promotion/Edit
        [HttpPut]
        public ActionResult Edit(PromotionEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TransactionService.UpdatePromotion(UserId, VendorId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.PromotionId
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
        // POST: /Admin/Promotion/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                TransactionService.UpdatePromotionStatus(UserId, id, status ? PromotionStatus.Active : PromotionStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/Promotion/Delete/5
        [HttpDelete]
        public ActionResult Delete([System.Web.Http.FromBody]int id)
        {
            try
            {
                TransactionService.UpdatePromotionStatus(UserId, id, PromotionStatus.InActive);
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
                    TransactionService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}