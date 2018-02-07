using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Contents;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Media;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Contents.Media
{
    public class MediaComposerController : BaseController
    {
        private IMediaService MediaService { get; set; }

        public MediaComposerController(IMediaService mediaService) : base(new IBaseService[] { mediaService })
        {
            MediaService = mediaService;
        }

        #region GET METHODS =========================================================================

        // GET: Admin/MediaComposer
        public ActionResult Index()
        {
            return View("../Contents/Media/MediaComposer/Index");
        }

        // GET: /Admin/MediaComposer/Create
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("../Contents/Media/MediaComposer/_Create");
        }

        // GET: /Admin/MediaComposer/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = MediaService.GetComposerDetail(id);

            var editModel = new MediaComposerEditEntry
            {
                ComposerId = entity.ComposerId,
                ComposerName = entity.ComposerName,
                Description = entity.Description,
                Status = entity.Status
            };
            return PartialView("../Contents/Media/MediaComposer/_Edit", editModel);
        }

        // GET: /Admin/MediaComposer/List
        [HttpGet]
        public ActionResult List(MediaComposerSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["MediaComposerSearchRequest"] = filter;
            }
            else
            {
                if (TempData["MediaComposerSearchRequest"] != null)
                {
                    filter = (MediaComposerSearchEntry)TempData["MediaComposerSearchRequest"];
                }
            }
            TempData.Keep();
            int? recordCount = 0;
            var sources = MediaService.GetComposers(filter, ref recordCount, "ComposerId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Contents/Media/MediaComposer/_List", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/MediaComposer/Create
        [HttpPost]
        public ActionResult Create(MediaComposerEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MediaService.InsertComposer(UserId, entry);
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
        // PUT - /Admin/MediaComposer/Edit
        [HttpPut]
        public ActionResult Edit(MediaComposerEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MediaService.UpdateComposer(UserId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ComposerId
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
        // POST: /Admin/MediaComposer/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                MediaService.UpdateComposerStatus(UserId, id, status?MediaComposerStatus.Active: MediaComposerStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/MediaComposer/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                MediaService.UpdateComposerStatus(UserId, id,MediaComposerStatus.InActive);
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
                    MediaService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}