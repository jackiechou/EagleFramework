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
    public class MediaPlayListController : BaseController
    {
        public IDocumentService DocumentService { get; set; }
        private IMediaService MediaService { get; set; }

        public MediaPlayListController(IMediaService mediaService, IDocumentService documentService) : base(new IBaseService[] { mediaService, documentService })
        {
            MediaService = mediaService;
            DocumentService = documentService;
        }

        #region GET METHODS =========================================================================

        // GET: Admin/MediaPlayList
        public ActionResult Index()
        {
            return View("../Contents/Media/MediaPlayList/Index");
        }

        // GET: /Admin/MediaPlayList/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.TypeId = MediaService.PoplulateMediaTypeSelectList(null, true);
            return PartialView("../Contents/Media/MediaPlayList/_Create");
        }

        // GET: /Admin/MediaPlayList/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = MediaService.GetPlayListDetail(id);
            if(item == null) return PartialView("../Contents/Media/MediaPlayList/_Edit");
            
            var editModel = new MediaPlayListEditEntry
            {
                TypeId = item.TypeId,
                TopicId = item.TopicId,
                PlayListId = item.PlayListId,
                PlayListName = item.PlayListName,
                PlayListAlias = item.PlayListAlias,
                FrontImage = item.FrontImage,
                MainImage = item.MainImage,
                Description = item.Description,
                Status = item.Status,
                FrontImageUrl = (item.FrontImage != null && item.FrontImage > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(item.FrontImage)).FileUrl : GlobalSettings.NotFoundFileUrl,
                MainImageUrl = (item.MainImage != null && item.FrontImage > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(item.MainImage)).FileUrl : GlobalSettings.NotFoundFileUrl,
                //Type = item.Type.ToDto<MediaType, MediaTypeDetail>(),
                //Topic = item.Topic.ToDto<MediaTopic, MediaTopicDetail>()
            };

            ViewBag.TypeId = MediaService.PoplulateMediaTypeSelectList(item.TypeId, true);
            return PartialView("../Contents/Media/MediaPlayList/_Edit", editModel);
        }

        // GET: /Admin/MediaPlayList/List
        [HttpGet]
        public ActionResult Search(MediaPlayListSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["MediaPlayListSearchRequest"] = filter;
            }
            else
            {
                if (TempData["MediaPlayListSearchRequest"] != null)
                {
                    filter = (MediaPlayListSearchEntry)TempData["MediaPlayListSearchRequest"];
                }
            }
            TempData.Keep();
            int? recordCount = 0;
            var sources = MediaService.GetPlayLists(filter, ref recordCount, "ListOrder DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Contents/Media/MediaPlayList/_SearchResult", lst);
        }

        // GET: Hierachical List 
        [HttpGet]
        public ActionResult GetMediaTopicSelectTree(int typeId, int? selectedId, bool? isRootShowed)
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

        // POST: /Admin/MediaPlayList/Create
        [HttpPost]
        public ActionResult Create(MediaPlayListEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MediaService.InsertPlayList(ApplicationId, UserId, entry);

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
        // PUT - /Admin/MediaPlayList/Edit
        [HttpPut]
        public ActionResult Edit(MediaPlayListEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MediaService.UpdatePlayList(ApplicationId, UserId, entry);

                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.PlayListId
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
        // POST: /Admin/MediaPlayList/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                MediaService.UpdatePlayListStatus(UserId, id, status? MediaPlayListStatus.Active: MediaPlayListStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/MediaPlayList/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                MediaService.UpdatePlayListStatus(UserId, id, MediaPlayListStatus.InActive);
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