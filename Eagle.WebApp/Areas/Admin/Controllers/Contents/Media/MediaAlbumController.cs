using System;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Contents;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Media;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Contents.Media
{
    public class MediaAlbumController : BaseController
    {
        public IDocumentService DocumentService { get; set; }
        private IMediaService MediaService { get; set; }

        public MediaAlbumController(IMediaService mediaService,IDocumentService documentService) : base(new IBaseService[] { mediaService, documentService })
        {
            MediaService = mediaService;
            DocumentService = documentService;
        }

        #region GET METHODS =========================================================================

        // GET: Admin/MediaAlbum
        public ActionResult Index()
        {
            return View("../Contents/Media/MediaAlbum/Index");
        }

        // GET: /Admin/MediaAlbum/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.TypeId = MediaService.PoplulateMediaTypeSelectList(null, true, MediaTypeStatus.Active);
            return PartialView("../Contents/Media/MediaAlbum/_Create");
        }

        // GET: /Admin/MediaAlbum/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = MediaService.GetAlbumDetail(id);

            var editModel = new MediaAlbumEditEntry
            {
                TypeId = item.TypeId,
                TopicId = item.TopicId,
                AlbumId = item.AlbumId,
                AlbumName = item.AlbumName,
                FrontImage = item.FrontImage,
                MainImage = item.MainImage,
                Description = item.Description,
                Status = item.Status,
                FrontImageUrl = (item.FrontImage != null && item.FrontImage > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(item.FrontImage)).FileUrl : GlobalSettings.NotFoundFileUrl,
                MainImageUrl = (item.MainImage != null && item.FrontImage > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(item.MainImage)).FileUrl : GlobalSettings.NotFoundFileUrl,
                //Type = item.Type.ToDto<MediaType, MediaTypeDetail>(),
                //Topic = item.Topic.ToDto<MediaTopic, MediaTopicDetail>(),
            };

            ViewBag.TypeId = MediaService.PoplulateMediaTypeSelectList(item.TypeId, true, MediaTypeStatus.Active);
            return PartialView("../Contents/Media/MediaAlbum/_Edit", editModel);
        }

        // GET: /Admin/MediaAlbum/List
        [HttpGet]
        public ActionResult Search(MediaAlbumSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["MediaAlbumSearchRequest"] = filter;
            }
            else
            {
                if (TempData["MediaAlbumSearchRequest"] != null)
                {
                    filter = (MediaAlbumSearchEntry)TempData["MediaAlbumSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = MediaService.GetAlbums(filter, ref recordCount, "AlbumId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Contents/Media/MediaAlbum/_SearchResult", lst);
        }

        // GET: Hierachical List 
        [HttpGet]
        public ActionResult GetMediaTopicSelectTree(int typeId=1, int? selectedId=null, bool? isRootShowed=true)
        {
            var list = MediaService.GetMediaTopicSelectTree(typeId, null, selectedId, isRootShowed);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // GET: Media Type 
        [HttpGet]
        public ActionResult PoplulateMediaTypeSelectList(int? selectedId, bool? isShowSelectText)
        {
            var list = MediaService.PoplulateMediaTypeSelectList(selectedId, isShowSelectText);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/MediaAlbum/Create
        [HttpPost]
        public ActionResult Create(MediaAlbumEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MediaService.InsertAlbum(ApplicationId, UserId, entry);

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
        // PUT - /Admin/MediaAlbum/Edit
        [HttpPut]
        public ActionResult Edit(MediaAlbumEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MediaService.UpdateAlbum(ApplicationId, UserId, entry);

                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.AlbumId
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
        // POST: /Admin/MediaAlbum/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                MediaService.UpdateAlbumStatus(UserId, id, status? MediaAlbumStatus.Active: MediaAlbumStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/MediaAlbum/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                MediaService.UpdateAlbumStatus(UserId, id, MediaAlbumStatus.InActive);
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
                    DocumentService = null;
                    MediaService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}