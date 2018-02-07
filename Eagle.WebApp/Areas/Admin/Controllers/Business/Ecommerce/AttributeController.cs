using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;
using System;
using Eagle.Services;

namespace Eagle.WebApp.Areas.Admin.Controllers.Business.Ecommerce
{
    [ValidateAntiForgeryTokenOnAllPosts]
    public class AttributeController : BaseController
    {
        private IProductService ProductService { get; set; }
        public AttributeController(IProductService productService) : base(new IBaseService[] { productService })
        {
            ProductService = productService;
        }
        #region GET METHODS =========================================================================
        //
        // GET: /Admin/Attribute/
        public ActionResult Index()
        {
            return View("../Business/Ecommerce/Attribute/Index");
        }

        // GET: /Admin/Attribute/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = ProductService.GetAttributeDetails(id);

            var editModel = new AttributeEditEntry
            {
               AttributeId=entity.AttributeId,
                AttributeName = entity.AttributeName,
                IsActive = entity.IsActive
            };
            return PartialView("../Business/Ecommerce/Attribute/_Edit", editModel);
        }

        // GET: /Admin/Attribute/Search
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            return PartialView("../Business/Ecommerce/Attribute/_SearchForm");
        }

        // GET: /Admin/Attribute/Search
        [HttpGet]
        public ActionResult Search(AttributeDetail filter, string sourceEvent, int? page = 1)
        {
            if (String.IsNullOrEmpty(sourceEvent))
            {
                TempData["CustomerTypeSearchRequest"] = filter;
            }
            else
            {
                if (TempData["CustomerTypeSearchRequest"] != null)
                {
                    filter = (AttributeDetail)TempData["CustomerTypeSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = ProductService.GetAttributes(filter, ref recordCount, "AttributeId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Business/Ecommerce/Attribute/_SearchResult", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/Attribute/Create
        [HttpPost]
        public ActionResult Create(int categoryId,AttributeEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ProductService.InsertAttribute(categoryId,entry);
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
        // PUT - /Admin/Attribute/Edit
        [HttpPut]
        public ActionResult Edit(AttributeEditEntry entry,int cagogoryId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ProductService.UpdateAttribute(cagogoryId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.AttributeId
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
        // POST: /Admin/Attribute/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                ProductService.UpdateAttributeStatus( id, status? ProductAttributeStatus.Active : ProductAttributeStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/Attribute/Delete/5
        [HttpDelete]
        public ActionResult Delete([System.Web.Http.FromBody]int id)
        {
            try
            {
                ProductService.UpdateAttributeStatus( id, ProductAttributeStatus.InActive);
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