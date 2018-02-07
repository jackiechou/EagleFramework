using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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
    public class BannerPositionController : BaseController
    {
        private IBannerService BannerService { get; set; }
        public BannerPositionController(IBannerService bannerService) : base(new IBaseService[] { bannerService })
        {
            BannerService = bannerService;
        }
        #region GET METHODS =========================================================================
        //
        // GET: /Admin/BannerPosition/
        public ActionResult Index()
        {
            return View("../Contents/BannerPositions/Index");
        }

        // GET: /Admin/BannerPosition/Create
        public ActionResult Create()
        {
            return PartialView("../Contents/BannerPositions/_Create");
        }

        // GET: /Admin/BannerPosition/Details/5        
        [HttpGet]
        public ActionResult Edit([System.Web.Http.FromBody]int id)
        {
            var entity = BannerService.GetBannerPositionDetail(id);
            var editModel = new BannerPositionEditEntry
            {
                PositionId = entity.PositionId,
                PositionName = entity.PositionName,
                Status = entity.Status,
                Description = entity.Description
            };

            return PartialView("../Contents/BannerPositions/_Edit", editModel);
        }

        // GET: /Admin/BannerPosition/List   
        public ActionResult List()
        {
            var sources = BannerService.GetBannerPositions(null);
            return PartialView("../Contents/BannerPositions/_List", sources);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/BannerPosition/Create
        [HttpPost]
        public ActionResult Create(BannerPositionEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BannerService.InsertBannerPosition(entry);
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
        // PUT - /Admin/BannerPosition/Edit
        [HttpPut]
        public ActionResult Edit(BannerPositionEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BannerService.UpdateBannerPosition(entry);
                    return Json(new SuccessResult { Status = ResultStatusType.Success, Data = new {
                            Message = LanguageResource.UpdateSuccess,
                            Id =entry.PositionId
                    } }, JsonRequestBehavior.AllowGet);
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
        // POST: /Admin/BannerPosition/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                BannerService.UpdateBannerPositionStatus(id, status?BannerPositionStatus.Active:BannerPositionStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // POST: /Admin/BannerPosition/Delete/5
        [HttpDelete]
        public ActionResult Delete([System.Web.Http.FromBody]int id)
        {
            try
            {
                BannerService.UpdateBannerPositionStatus(id,BannerPositionStatus.InActive);
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
                    BannerService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
