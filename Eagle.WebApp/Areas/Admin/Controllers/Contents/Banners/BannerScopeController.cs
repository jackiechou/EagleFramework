using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Contents;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Banners;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Contents.Banners
{
    [ValidateAntiForgeryTokenOnAllPosts]
    public class BannerScopeController : BaseController
    {
        private IBannerService BannerService { get; set; }
        public BannerScopeController(IBannerService bannerService) : base(new IBaseService[] { bannerService })
        {
            BannerService = bannerService;
        }
        #region GET METHODS =========================================================================

        // GET: Admin/BannerScope
        public ActionResult Index()
        {
            return View("../Contents/BannerScopes/Index");
        }

        // GET: /Admin/BannerScope/Create
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("../Contents/BannerScopes/_Create");
        }

        // GET: /Admin/BannerScope/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = BannerService.GetBannerScopeDetails(id);

            var editModel = new BannerScopeEditEntry
            {
                ScopeId = entity.ScopeId,
                ScopeName = entity.ScopeName,
                Description = entity.Description,
                Status = entity.Status
            };
            return PartialView("../Contents/BannerScopes/_Edit", editModel);
        }

        // GET: /Admin/BannerScope/List
        [HttpGet]
        public ActionResult List(int? page = 1)
        {
            int? recordCount = 0;
            var sources = BannerService.GetBannerScopes(ref recordCount, page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Contents/BannerScopes/_List", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/BannerScope/Create
        [HttpPost]
        public ActionResult Create(BannerScopeEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BannerService.InsertBannerScope(entry);
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
        // PUT - /Admin/BannerScope/Edit
        [HttpPut]
        public ActionResult Edit(BannerScopeEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BannerService.UpdateBannerScope(entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ScopeId
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
        // POST: /Admin/BannerScope/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                BannerService.UpdateBannerScopeStatus(id, status? BannerScopeStatus.Active: BannerScopeStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/BannerScope/Delete/5
        [HttpDelete]
        public ActionResult Delete([System.Web.Http.FromBody]int id)
        {
            try
            {
                BannerService.UpdateBannerScopeStatus(id, BannerScopeStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.DeleteSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion =============================================================================================================

        #region Dispose

        private bool _disposed;

        [NonAction]
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    BannerService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}