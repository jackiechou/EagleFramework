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
    [ValidateAntiForgeryTokenOnAllPosts]
    public class PaymentMethodController : BaseController
    {
        private ITransactionService TransactionService { get; set; }
        public PaymentMethodController(ITransactionService transactionService) : base(new IBaseService[] { transactionService })
        {
            TransactionService = transactionService;
        }
        #region GET METHODS =========================================================================
        //
        // GET: /Admin/PaymentMethod/
        public ActionResult Index()
        {
            return View("../Business/Ecommerce/Transaction/PaymentMethod/Index");
        }

        // GET: /Admin/PaymentMethod/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = TransactionService.GetPaymentMethodDetail(id);

            var editModel = new PaymentMethodEditEntry
            {
                PaymentMethodId = entity.PaymentMethodId,
                PaymentMethodName = entity.PaymentMethodName,
                IsCreditCard = entity.IsCreditCard,
                IsActive = entity.IsActive
            };
            return PartialView("../Business/Ecommerce/Transaction/PaymentMethod/_Edit", editModel);
        }

        // GET: /Admin/PaymentMethod/Search
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            return PartialView("../Business/Ecommerce/Transaction/PaymentMethod/_SearchForm");
        }

        // GET: /Admin/PaymentMethod/Search
        [HttpGet]
        public ActionResult Search(PaymentMethodSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["PaymentMethodSearchRequest"] = filter;
            }
            else
            {
                if (TempData["PaymentMethodSearchRequest"] != null)
                {
                    filter = (PaymentMethodSearchEntry)TempData["PaymentMethodSearchRequest"];
                }
            }
            TempData.Keep();
            int? recordCount = 0;
            var sources = TransactionService.GetPaymentMethods(filter, ref recordCount, "PaymentMethodId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Business/Ecommerce/Transaction/PaymentMethod/_SearchResult", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/PaymentMethod/Create
        [HttpPost]
        public ActionResult Create(PaymentMethodEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TransactionService.InsertPaymentMethod(entry);
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
        // PUT - /Admin/PaymentMethod/Edit
        [HttpPut]
        public ActionResult Edit(PaymentMethodEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TransactionService.UpdatePaymentMethod(entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.PaymentMethodId
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
        // POST: /Admin/PaymentMethod/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                TransactionService.UpdatePaymentMethodStatus(id, status ? PaymentMethodStatus.Active : PaymentMethodStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }


        //
        // PUT: /Admin/PaymentMethod/UpdateSelectedPaymentMethod/5
        [HttpPut]
        public ActionResult UpdateSelectedPaymentMethod(int id)
        {
            try
            {
                TransactionService.UpdateSelectedPaymentMethod(id);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }


        //
        // DELETE: /Admin/PaymentMethod/Delete/5
        [HttpDelete]
        public ActionResult Delete([System.Web.Http.FromBody]int id)
        {
            try
            {
                TransactionService.UpdatePaymentMethodStatus(id, PaymentMethodStatus.InActive);
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