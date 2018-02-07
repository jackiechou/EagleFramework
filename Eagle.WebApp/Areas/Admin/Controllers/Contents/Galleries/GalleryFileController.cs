using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Contents;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Galleries;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Contents.Galleries
{
    public class GalleryFileController : BaseController
    {
        private IGalleryService GalleryService { get; set; }
        private IDocumentService DocumentService { get; set; }

        public GalleryFileController(IGalleryService galleryService, IDocumentService documentService)
            : base(new IBaseService[] { galleryService, documentService })
        {
            GalleryService = galleryService;
            DocumentService = documentService;
        }

        #region GET METHODS ============================================================================

        // GET: Admin/GalleryFile
        public ActionResult Index()
        {
            return View("../Contents/GalleryFile/Index");
        }
        
        [HttpGet]
        public ActionResult GetGalleryCollections(int? topicId, int? collectionId)
        {
            var list = GalleryService.GetGalleryCollections(topicId, null);
            return PartialView("../Contents/GalleryFile/_Collections", list);
        }
        
        // GET: /Admin/GalleryFile/Search   
        public ActionResult Search(GalleryFileSearchEntry filter, string sourceEvent, int? page)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["GalleryFileSearchSearchRequest"] = filter;
            }
            else
            {
                if (TempData["GalleryFileSearchSearchRequest"] != null)
                {
                    filter = (GalleryFileSearchEntry)TempData["GalleryFileSearchSearchRequest"];
                }
            }
            TempData.Keep();

            int recordCount = 0, pageSize = GlobalSettings.DefaultPageSize;
            var sources = GalleryService.GetGalleryFiles(filter.SearchCollectionId,filter.SearchStatus, out recordCount, "ListOrder DESC", page, pageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, pageSize, recordCount);
            return PartialView("../Contents/GalleryFile/_SearchResult", lst);
        }


        // GET: /Admin/GalleryFile/Create        
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("../Contents/GalleryFile/_Create");
        }

        // GET: /Admin/GalleryFile/Edit/5        
        [HttpGet]
        public ActionResult Edit(int collectionId, int fileId)
        {
            var model = new GalleryFileEditEntry();
            var item = GalleryService.GetGalleryFileDetail(collectionId, fileId);
            if (item != null)
            {
                var collectioin = GalleryService.GetGalleryCollectionDetail(collectionId);
                model.TopicId = collectioin.TopicId;
                model.GalleryFileId = item.GalleryFileId;
                model.CollectionId = item.CollectionId;
                model.FileId = item.FileId;
                model.Status = item.Status;

                var fileInfo = DocumentService.GetFileInfoDetail(item.FileId);
                if (fileInfo != null)
                {
                    model.FileUrl = fileInfo.FileUrl;
                }
            }

            return PartialView("../Contents/GalleryFile/_Edit", model);
        }


        // Get Hierachical List 
        [HttpGet]
        public ActionResult GetGalleryTopicTree(int? selectedId = null, bool? isRootShowed = false)
        {
            var list = GalleryService.GetGalleryTopicTreeGrid(null, selectedId, isRootShowed);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PoplulateGalleryTopicTree(int? selectedId=null, bool? isRootShowed=false)
        {
            var list = GalleryService.GetGalleryTopicTreeNode(null, selectedId, isRootShowed);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetGalleryCollectionSelectList(int topicId, int? selectedValue = null, bool? isShowSelectText = null)
        {
            var list = GalleryService.GetGalleryCollectionSelectList(topicId, null, selectedValue, isShowSelectText);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult PoplulateGalleryCollectionSelectList(int topicId, int? selectedValue = null, bool? isShowSelectText=null)
        {
            var list = GalleryService.PoplulateGalleryCollectionSelectList(topicId, null, selectedValue, isShowSelectText);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/GalleryFile/Create
        [HttpPost]
        public ActionResult Create(GalleryFileEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    GalleryService.InsertGalleryFile(ApplicationId, UserId, entry);
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
        // PUT - /Admin/GalleryFile/Edit
        [HttpPut]
        public ActionResult Edit(GalleryFileEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    GalleryService.UpdateGalleryFile(ApplicationId, UserId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            CollectionId = entry.CollectionId,
                            FileId = entry.FileId
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
        // POST: /Admin/GalleryFile/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int collectionId, int fileId, bool status)
        {
            try
            {
                GalleryService.UpdateGalleryFileStatus(ApplicationId, UserId, collectionId, fileId, status);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // POST: /Admin/GalleryFile/Delete/5
        [HttpDelete]
        public ActionResult Delete(int collectionId, int fileId)
        {
            try
            {
                GalleryService.DeleteGalleryFile(collectionId, fileId);
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
                    GalleryService = null;
                    DocumentService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}