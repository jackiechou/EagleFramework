using System.Web.Mvc;
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
    public class ProductCategoryController : BaseController
    {
        private IProductService ProductService { get; set; }

        public ProductCategoryController(IProductService productService) : base(new IBaseService[] { productService })
        {
            ProductService = productService;
        }
        // GET: Admin/ProductCategory
        public ActionResult Index()
        {
           return View("../Business/Ecommerce/ProductCategory/Index");
        }

        // Get Hierachical List 
        [HttpGet]
        public ActionResult GetProductCategorySelectTree(int? selectedId, bool? isRootShowed)
        {
            var list = ProductService.GetProductCategorySelectTree(null, selectedId, isRootShowed);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
       
        //
        // GET: /Admin/ProductCategory/Create       
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("../Business/Ecommerce/ProductCategory/_Create");
        }
    
        //
        // GET: /Admin/ProductCategory/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = ProductService.GetProductCategoryDetails(id);

            var entry = new ProductCategoryEditEntry
            {
                CategoryId = item.CategoryId,
                CategoryCode = item.CategoryCode,
                CategoryName = item.CategoryName,
                CategoryLink = item.CategoryLink,
                ParentId = item.ParentId,
                BriefDescription = item.BriefDescription,
                Description = item.Description,
                Icon = item.Icon,
                Status = item.Status
            };

            return PartialView("../Business/Ecommerce/ProductCategory/_Edit", entry);
        }
        
        //
        // POST: /Admin/ProductCategory/Create-
        [HttpPost]
        public ActionResult Create(ProductCategoryEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ProductService.InsertProductCategory(UserId, VendorId, LanguageCode, entry);
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

        ////
        //// PUT: /Admin/ProductCategory/Edit/5
        [HttpPut]
        public ActionResult Edit(ProductCategoryEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ProductService.UpdateProductCategory(UserId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.CategoryId
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

        ////
        //// PUT: /Admin/ProductCategory/UpdateListOrder
        [HttpPut]
        public ActionResult UpdateProductCategoryListOrder(int id, int listOrder)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ProductService.UpdateProductCategoryListOrder(UserId, id, listOrder);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = id
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

        // POST: /Admin/ProductCategory/UpdateStatus/5
        public ActionResult UpdateStatus(int id, int status)
        {
            try
            {
                ProductService.UpdateProductCategoryStatus(UserId, id, (ProductCategoryStatus)status);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // POST: /Admin/ProductCategory/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                ProductService.UpdateProductCategoryStatus(UserId, id, ProductCategoryStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.DeleteSuccess }, JsonRequestBehavior.AllowGet);

            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

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