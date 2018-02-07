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
    public class CurrencyController : BaseController
    {
        private ICurrencyService CurrencyService { get; set; }
        public CurrencyController(ICurrencyService currencyService) : base(new IBaseService[] { currencyService })
        {
            CurrencyService = currencyService;
        }
        #region GET METHODS =========================================================================
        //
        // GET: /Admin/Currency/
        public ActionResult Index()
        {
            return View("../Business/Ecommerce/Transaction/Currency/Index");
        }

        // GET: /Admin/Currency/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = CurrencyService.GetCurrencyDetail(id);

            var editModel = new CurrencyEditEntry
            {
                CurrencyId = entity.CurrencyId,
                CurrencyCode = entity.CurrencyCode,
                CurrencyName = entity.CurrencyName,
                IsActive = entity.IsActive
            };
            return PartialView("../Business/Ecommerce/Transaction/Currency/_Edit", editModel);
        }

        // GET: /Admin/Currency/Search
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            return PartialView("../Business/Ecommerce/Transaction/Currency/_SearchForm");
        }

        // GET: /Admin/Currency/Search
        [HttpGet]
        public ActionResult Search(CurrencySearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["CurrencySearchRequest"] = filter;
            }
            else
            {
                if (TempData["CurrencySearchRequest"] != null)
                {
                    filter = (CurrencySearchEntry)TempData["CurrencySearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = CurrencyService.GetCurrencies(filter, ref recordCount, "CurrencyId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Business/Ecommerce/Transaction/Currency/_SearchResult", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/Currency/Create
        [HttpPost]
        public ActionResult Create(CurrencyEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CurrencyService.InsertCurrency(entry);
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
        // PUT - /Admin/Currency/Edit
        [HttpPut]
        public ActionResult Edit(CurrencyEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CurrencyService.UpdateCurrency(entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.CurrencyId
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
        // PUT: /Admin/Currency/UpdateSelectedCurrency/5
        [HttpPut]
        public ActionResult UpdateSelectedCurrency(int id)
        {
            try
            {
                CurrencyService.UpdateSelectedCurrency(id);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // PUT: /Admin/Currency/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                CurrencyService.UpdateCurrencyStatus(id, status ? CurrencyStatus.Active : CurrencyStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/Currency/Delete/5
        [HttpDelete]
        public ActionResult Delete([System.Web.Http.FromBody]int id)
        {
            try
            {
                CurrencyService.UpdateCurrencyStatus(id, CurrencyStatus.InActive);
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
                    CurrencyService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}