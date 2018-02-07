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
    public class MediaFileController : BaseController
    {
        private IMediaService MediaService { get; set; }
        private IDocumentService DocumentService { get; set; }

        public MediaFileController(IMediaService mediaService, IDocumentService documentService) : base(new IBaseService[] { mediaService, documentService })
        {
            MediaService = mediaService;
            DocumentService = documentService;
        }
        #region GET METHODS =========================================================================

        // GET: Admin/MediaFile
        public ActionResult Index()
        {
            return View("../Contents/Media/MediaFile/Index");
        }

        // GET: /Admin/MediaFile/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.ComposerId = MediaService.PoplulateMediaComposerSelectList(null, false);
            //ViewBag.FileType = MediaService.PopulateMediaFileTypes(null, false);
            ViewBag.StorageType = DocumentService.PopulateStorageTypes(null, false);

            return PartialView("../Contents/Media/MediaFile/_Create");
        }

        // GET: /Admin/MediaFile/Edit/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = MediaService.GetMediaFileInfoDetail(id);
            var editModel = new MediaFileEditEntry
            {
                MediaId = item.MediaId,
                FileId = item.FileId,
                TypeId = item.TypeId,
                TopicId = item.TopicId,
                ComposerId = item.ComposerId,
                Artist = item.Artist,
                AutoStart = item.AutoStart,
                MediaLoop = item.MediaLoop,
                Lyric = item.Lyric,
                SmallPhoto = item.SmallPhoto,
                LargePhoto = item.LargePhoto,
                StorageType = item.DocumentFileInfo.StorageType,
                FileTitle = item.DocumentFileInfo.FileTitle,
                FileDescription = item.DocumentFileInfo.FileDescription,
                FileUrl = item.DocumentFileInfo.FileUrl,
                Width = item.DocumentFileInfo.Width,
                Height = item.DocumentFileInfo.Height
            };
           
            ViewBag.ComposerId = MediaService.PoplulateMediaComposerSelectList(item.ComposerId, true);
            ViewBag.StorageType = DocumentService.PopulateStorageTypes(item.DocumentFileInfo.StorageType, true);
            //  ViewBag.FileType = MediaService.PopulateMediaFileTypes(item.FileType, true);

            return PartialView("../Contents/Media/MediaFile/_Edit", editModel);
        }

        // GET: /Admin/MediaFile/Search
        [HttpGet]
        public ActionResult Search(MediaFileSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["MediaFileSearchRequest"] = filter;
            }
            else
            {
                if (TempData["MediaFileSearchRequest"] != null)
                {
                    filter = (MediaFileSearchEntry)TempData["MediaFileSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = MediaService.GetMediaFiles(filter, ref recordCount, "ListOrder DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Contents/Media/MediaFile/_SearchResult", lst);
        }

        // GET: Hierachical List 
        [HttpGet]
        public ActionResult GetMediaTopicSelectTree(int typeId = 1, int? selectedId = null, bool? isRootShowed = true)
        {
            var list = MediaService.GetMediaTopicSelectTree(typeId, null, selectedId, isRootShowed);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // GET: Media Type 
        [HttpGet]
        public ActionResult PoplulateMediaTypeSelectList(int? selectedId, bool? isShowSelectText = false)
        {
            var list = MediaService.PoplulateMediaTypeSelectList(selectedId, isShowSelectText);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // GET: Media File Type 
        [HttpGet]
        public ActionResult PoplulateMediaFileTypeSelectList(string selectedValue = null, bool? isShowSelectText = false)
        {
            var list = MediaService.PopulateMediaFileTypes(selectedValue, isShowSelectText);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PoplulateMediaComposerSelectList(int? selectedId, bool? isShowSelectText)
        {
            var list = MediaService.PoplulateMediaComposerSelectList(selectedId, isShowSelectText);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // GET: Media Album 
        [HttpGet]
        public ActionResult PoplulateMediaAlbumMultiSelectList(int typeId, int topicId, MediaAlbumStatus? status = null, bool? isShowSelectText = null, int[] selectedValues = null)
        {
            var list = MediaService.PoplulateMediaAlbumMultiSelectList(typeId, topicId, status, isShowSelectText, selectedValues);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // GET: Media PlayList 
        [HttpGet]
        public ActionResult PoplulateMediaPlayListMultiSelectList(int typeId, int topicId, MediaPlayListStatus? status = null, bool? isShowSelectText = null, int[] selectedValues = null)
        {
            var list = MediaService.PoplulateMediaPlayListMultiSelectList(typeId, topicId, status, isShowSelectText, selectedValues);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/MediaFile/Create
        [HttpPost]
        public ActionResult Create(MediaFileEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MediaService.InsertMediaFile(ApplicationId, UserId, entry);
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
        // PUT - /Admin/MediaFile/Edit
        [HttpPut]
        public ActionResult Edit(MediaFileEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MediaService.UpdateMediaFile(ApplicationId, UserId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.MediaId
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
        // POST: /Admin/MediaFile/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, int status)
        {
            try
            {
                MediaService.UpdateMediaFileStatus(id, (DocumentFileStatus)status);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/MediaFile/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                MediaService.DeleteMediaFile(id);
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
                    DocumentService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}