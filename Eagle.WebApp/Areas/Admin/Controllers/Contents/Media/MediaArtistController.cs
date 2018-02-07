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
    public class MediaArtistController : BaseController
    {
        private IMediaService MediaService { get; set; }

        public MediaArtistController(IMediaService mediaService) : base(new IBaseService[] { mediaService })
        {
            MediaService = mediaService;
        }

        #region GET METHODS =========================================================================

        // GET: Admin/MediaArtist
        public ActionResult Index()
        {
            return View("../Contents/Media/MediaArtist/Index");
        }

        // GET: /Admin/MediaArtist/Create
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("../Contents/Media/MediaArtist/_Create");
        }

        // GET: /Admin/MediaArtist/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = MediaService.GetArtistDetail(id);

            var editModel = new MediaArtistEditEntry
            {
                ArtistId = entity.ArtistId,
                ArtistName = entity.ArtistName,
                Description = entity.Description,
                Status = entity.Status
            };
            return PartialView("../Contents/Media/MediaArtist/_Edit", editModel);
        }

        // GET: /Admin/MediaArtist/List
        [HttpGet]
        public ActionResult List(MediaArtistSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["MediaArtistSearchRequest"] = filter;
            }
            else
            {
                if (TempData["MediaArtistSearchRequest"] != null)
                {
                    filter = (MediaArtistSearchEntry)TempData["MediaArtistSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = MediaService.GetArtists(filter, ref recordCount, "ArtistId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Contents/Media/MediaArtist/_List", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/MediaArtist/Create
        [HttpPost]
        public ActionResult Create(MediaArtistEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MediaService.InsertArtist(UserId, entry);
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
        // PUT - /Admin/MediaArtist/Edit
        [HttpPut]
        public ActionResult Edit(MediaArtistEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MediaService.UpdateArtist(UserId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ArtistId
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
        // POST: /Admin/MediaArtist/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                MediaService.UpdateArtistStatus(UserId, id, status?MediaArtistStatus.Active: MediaArtistStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/MediaArtist/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                MediaService.UpdateArtistStatus(UserId, id, MediaArtistStatus.InActive);
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