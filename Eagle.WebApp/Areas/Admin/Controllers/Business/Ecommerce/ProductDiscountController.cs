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
    public class ProductDiscountController : BaseController
    {
        private IProductService ProductService { get; set; }
        public ProductDiscountController(IProductService productService) : base(new IBaseService[] { productService })
        {
            ProductService = productService;
        }

        #region GET METHODS =========================================================================
        //
        // GET: /Admin/ProductDiscount/
        public ActionResult Index()
        {
            return View("../Business/Ecommerce/ProductDiscount/Index");
        }

        // GET: /Admin/ProductDiscount/Create
        [HttpGet]
        public ActionResult Create()
        {
            var model = new ProductDiscountEditEntry();
            return PartialView("../Business/Ecommerce/ProductDiscount/_Create", model);
        }

        // GET: /Admin/ProductDiscount/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = new ProductDiscountEditEntry();
            var item = ProductService.GetProductDiscountDetails(id);
            if (item != null)
            {
                model.DiscountId = item.DiscountId;
                model.DiscountCode = item.DiscountCode;
                model.DiscountType = item.DiscountType;
                model.Quantity = item.Quantity;
                model.DiscountRate = item.DiscountRate;
                model.IsPercent = item.IsPercent;
                model.Description = item.Description;
                model.StartDate = item.StartDate;
                model.EndDate = item.EndDate;
                model.IsActive = item.IsActive;

                if (item.StartDate != null)
                {
                    DateTime startDate = Convert.ToDateTime(item.StartDate);
                    model.StartDateText = startDate.ToString("MM/dd/yyyy");
                }

                if (item.EndDate != null)
                {
                    DateTime endDate = Convert.ToDateTime(item.EndDate);
                    model.EndDateText = endDate.ToString("MM/dd/yyyy");
                }
            }
            return PartialView("../Business/Ecommerce/ProductDiscount/_Edit", model);
        }

        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            return PartialView("../Business/Ecommerce/ProductDiscount/_SearchForm");
        }

        // GET: /Admin/ProductDiscount/List
        [HttpGet]
        public ActionResult Search(ProductDiscountSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (String.IsNullOrEmpty(sourceEvent))
            {
                TempData["ProductDiscountSearchRequest"] = filter;
            }
            else
            {
                if (TempData["ProductDiscountSearchRequest"] != null)
                {
                    filter = (ProductDiscountSearchEntry)TempData["ProductDiscountSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = ProductService.GeProductDiscounts(VendorId, filter, ref recordCount, "DiscountId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Business/Ecommerce/ProductDiscount/_SearchResult", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/ProductDiscount/Create
        [HttpPost]
        public ActionResult Create(ProductDiscountEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ProductService.InsertProductDiscount(UserId, VendorId, entry);
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
        // PUT - /Admin/ProductDiscount/Edit
        [HttpPut]
        public ActionResult Edit(ProductDiscountEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ProductService.UpdateProductDiscount(UserId, VendorId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.DiscountId
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
        // POST: /Admin/ProductDiscount/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                ProductService.UpdateProductDiscountStatus(UserId, id, status ? ProductDiscountStatus.Active : ProductDiscountStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/ProductDiscount/Delete/5
        [HttpDelete]
        public ActionResult Delete([System.Web.Http.FromBody]int id)
        {
            try
            {
                ProductService.UpdateProductDiscountStatus(UserId, id, ProductDiscountStatus.InActive);
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