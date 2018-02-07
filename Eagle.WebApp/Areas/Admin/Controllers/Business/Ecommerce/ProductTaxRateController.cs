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
    public class ProductTaxRateController : BaseController
    {
        private IProductService ProductService { get; set; }
        public ProductTaxRateController(IProductService productService) : base(new IBaseService[] { productService })
        {
            ProductService = productService;
        }

        #region GET METHODS =========================================================================
        //
        // GET: /Admin/ProductTaxRate/
        public ActionResult Index()
        {
            return View("../Business/Ecommerce/ProductTaxRate/Index");
        }

        // GET: /Admin/ProductTaxRate/Create
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("../Business/Ecommerce/ProductTaxRate/_Create");
        }

        // GET: /Admin/ProductTaxRate/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = new ProductTaxRateEditEntry();
            var item = ProductService.GetProductTaxRateDetails(id);
            if (item != null)
            {
                model.TaxRateId = item.TaxRateId;
                model.TaxRate = item.TaxRate;
                model.IsPercent = item.IsPercent;
                model.Description = item.Description;
                model.IsActive = item.IsActive;
            }
            return PartialView("../Business/Ecommerce/ProductTaxRate/_Edit", model);
        }

        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            return PartialView("../Business/Ecommerce/ProductTaxRate/_SearchForm");
        }

        // GET: /Admin/ProductTaxRate/List
        [HttpGet]
        public ActionResult Search(ProductTaxRateSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["ProductTaxRateSearchRequest"] = filter;
            }
            else
            {
                if (TempData["ProductTaxRateSearchRequest"] != null)
                {
                    filter = (ProductTaxRateSearchEntry)TempData["ProductTaxRateSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = ProductService.GeProductTaxRates(filter, ref recordCount, "TaxRateId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Business/Ecommerce/ProductTaxRate/_SearchResult", lst);
        }

        #endregion =====================================================================================
        
        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/ProductTaxRate/Create
        [HttpPost]
        public ActionResult Create(ProductTaxRateEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ProductService.InsertProductTaxRate(entry);
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
        // PUT - /Admin/ProductTaxRate/Edit
        [HttpPut]
        public ActionResult Edit(ProductTaxRateEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ProductService.UpdateProductTaxRate(entry);
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
        // POST: /Admin/ProductTaxRate/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                ProductService.UpdateProductTaxRateStatus(id, status ? ProductTaxRateStatus.Active : ProductTaxRateStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/ProductTaxRate/Delete/5
        [HttpDelete]
        public ActionResult Delete([System.Web.Http.FromBody]int id)
        {
            try
            {
                ProductService.UpdateProductTaxRateStatus(id, ProductTaxRateStatus.InActive);
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
                    ProductService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}