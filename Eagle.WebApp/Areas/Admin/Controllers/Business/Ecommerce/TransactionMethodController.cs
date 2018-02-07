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
    public class TransactionMethodController : BaseController
    {
        private ITransactionService TransactionService { get; set; }

        public TransactionMethodController(ITransactionService transactionService) : base(new IBaseService[] { transactionService })
        {
            TransactionService = transactionService;
        }

        #region GET METHODS =========================================================================
        //
        // GET: /Admin/TransactionMethod/
        public ActionResult Index()
        {
            return View("../Business/Ecommerce/Transaction/TransactionMethod/Index");
        }

        // GET: /Admin/TransactionMethod/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = TransactionService.GetTransactionMethodDetail(id);
            var editModel = new TransactionMethodEditEntry
            {
                TransactionMethodId = entity.TransactionMethodId,
                TransactionMethodName = entity.TransactionMethodName,
                IsActive = entity.IsActive
            };

            if (entity.TransactionMethodFee != null)
            {
                editModel.TransactionMethodFee = $"{entity.TransactionMethodFee:#.###}";
            }
            return PartialView("../Business/Ecommerce/Transaction/TransactionMethod/_Edit", editModel);
        }

        // GET: /Admin/TransactionMethod/Search
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            return PartialView("../Business/Ecommerce/Transaction/TransactionMethod/_SearchForm");
        }

        // GET: /Admin/TransactionMethod/Search
        [HttpGet]
        public ActionResult Search(TransactionMethodSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["TransactionMethodSearchRequest"] = filter;
            }
            else
            {
                if (TempData["TransactionMethodSearchRequest"] != null)
                {
                    filter = (TransactionMethodSearchEntry)TempData["TransactionMethodSearchRequest"];
                }
            }
            TempData.Keep();
            int? recordCount = 0;
            var sources = TransactionService.GetTransactionMethods(filter, ref recordCount, "TransactionMethodId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Business/Ecommerce/Transaction/TransactionMethod/_SearchResult", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/TransactionMethod/Create
        [HttpPost]
        public ActionResult Create(TransactionMethodEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TransactionService.InsertTransactionMethod(entry);
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
        // PUT - /Admin/TransactionMethod/Edit
        [HttpPut]
        public ActionResult Edit(TransactionMethodEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TransactionService.UpdateTransactionMethod(entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.TransactionMethodId
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
        // POST: /Admin/TransactionMethod/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                TransactionService.UpdateTransactionMethodStatus(id, status ? TransactionMethodStatus.Active : TransactionMethodStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/TransactionMethod/Delete/5
        [HttpDelete]
        public ActionResult Delete([System.Web.Http.FromBody]int id)
        {
            try
            {
                TransactionService.UpdateTransactionMethodStatus(id, TransactionMethodStatus.InActive);
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