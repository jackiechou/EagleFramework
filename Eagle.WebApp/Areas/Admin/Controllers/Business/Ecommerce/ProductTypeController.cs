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
    public class ProductTypeController : BaseController
    {
        private IProductService ProductService { get; set; }
        public ProductTypeController(IProductService productService) : base(new IBaseService[] { productService })
        {
            ProductService = productService;
        }
    
        #region GET METHODS =========================================================================
        //
        // GET: /Admin/ProductType/
        public ActionResult Index()
        {
            return View("../Business/Ecommerce/ProductType/Index");
        }

        // GET: /Admin/ProductType/Create
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("../Business/Ecommerce/ProductType/_Create");
        }

        // GET: /Admin/ProductType/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = ProductService.GetProductTypeDetails(id);

            var editModel = new ProductTypeEditEntry
            {
                CategoryId = entity.CategoryId,
                TypeId = entity.TypeId,
                TypeName = entity.TypeName,
                Description = entity.Description,
                IsActive = entity.IsActive
            };
            return PartialView("../Business/Ecommerce/ProductType/_Edit", editModel);
        }

        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            return PartialView("../Business/Ecommerce/ProductType/_SearchForm");
        }

        // GET: /Admin/ProductType/Search
        [HttpGet]
        public ActionResult Search(ProductTypeSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["ProductTypeSearchRequest"] = filter;
            }
            else
            {
                if (TempData["ProductTypeSearchRequest"] != null)
                {
                    filter = (ProductTypeSearchEntry)TempData["ProductTypeSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = ProductService.GeProductTypes(VendorId, filter, ref recordCount, "ListOrder DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Business/Ecommerce/ProductType/_SearchResult", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/ProductType/Create
        [HttpPost]
        public ActionResult Create(ProductTypeEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ProductService.InsertProductType(VendorId, entry);
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
        // PUT - /Admin/ProductType/Edit
        [HttpPut]
        public ActionResult Edit(ProductTypeEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ProductService.UpdateProductType(VendorId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.TypeId
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
        // POST: /Admin/ProductType/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                ProductService.UpdateProductTypeStatus(id, status? ProductTypeStatus.Active: ProductTypeStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/ProductType/Delete/5
        [HttpDelete]
        public ActionResult Delete([System.Web.Http.FromBody]int id)
        {
            try
            {
                ProductService.UpdateProductTypeStatus(id, ProductTypeStatus.InActive);
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