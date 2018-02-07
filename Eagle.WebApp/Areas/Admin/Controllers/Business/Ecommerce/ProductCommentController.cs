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
    public class ProductCommentController : BaseController
    {
        private IProductService ProductService { get; set; }
        public ProductCommentController(IProductService productService) : base(new IBaseService[] { productService })
        {
            ProductService = productService;
        }

        #region GET METHODS =========================================================================
        //
        // GET: /Admin/ProductComment/
        public ActionResult Index()
        {
            return View("../Business/Ecommerce/ProductComment/Index");
        }

        //// GET: /Admin/ProductComment/Details/5        
        //[HttpGet]
        //public ActionResult Edit(int id)
        //{
        //    var model = new ProductCommentEditEntry();
        //    var item = ProductService.GetProductCommentDetails(id);
        //    if (item != null)
        //    {
        //        model.ProductId = item.ProductId;
        //        model.CommentId = item.CommentId;
        //        model.CommentName = item.CommentName;
        //        model.CommentEmail = item.CommentEmail;
        //        model.CommentText = item.CommentText;
        //        model.IsReplied = item.IsReplied;
        //        model.IsActive = item.IsActive;
        //    }
        //    return PartialView("../Business/Ecommerce/ProductComment/_Edit", model);
        //}

        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            return PartialView("../Business/Ecommerce/ProductComment/_SearchForm");
        }

        // GET: /Admin/ProductComment/List
        [HttpGet]
        public ActionResult Search(ProductCommentSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["ProductCommentSearchRequest"] = filter;
            }
            else
            {
                if (TempData["ProductCommentSearchRequest"] != null)
                {
                    filter = (ProductCommentSearchEntry)TempData["ProductCommentSearchRequest"];
                }
            }
            TempData.Keep();
            int? recordCount = 0;
            var sources = ProductService.GeProductComments(filter, ref recordCount, "CommentId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Business/Ecommerce/ProductComment/_SearchResult", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/ProductComment/Create
        [HttpPost]
        public ActionResult Create(ProductCommentEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ProductService.InsertProductComment(entry);
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
        // POST: /Admin/ProductComment/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                ProductService.UpdateProductCommentStatus(id, status ? ProductCommentStatus.Active : ProductCommentStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/ProductComment/Delete/5
        [HttpDelete]
        public ActionResult Delete([System.Web.Http.FromBody]int id)
        {
            try
            {
                ProductService.UpdateProductCommentStatus(id, ProductCommentStatus.InActive);
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