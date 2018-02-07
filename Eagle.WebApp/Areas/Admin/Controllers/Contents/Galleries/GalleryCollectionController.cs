using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Contents;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Galleries;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Contents.Galleries
{
    [ValidateAntiForgeryTokenOnAllPosts]
    public class GalleryCollectionController : BaseController
    {
        private IGalleryService GalleryService { get; set; }

        public GalleryCollectionController(IGalleryService galleryService) : base(new IBaseService[] { galleryService })
        {
            GalleryService = galleryService;
        }

        #region GET METHODS =========================================================================
        // GET: Admin/GalleryCollection
        public ActionResult Index()
        {
            return View("../Contents/GalleryCollection/Index");
        }

        // GET: /Admin/GalleryCollection/Details/5        
        [HttpGet]
        public ActionResult Edit([System.Web.Http.FromBody]int id)
        {
            var entity = GalleryService.GetGalleryCollectionDetail(id);
            var editModel = new GalleryCollectionEditEntry
            {
                TopicId = entity.TopicId,
                CollectionId = entity.CollectionId,
                CollectionName = entity.CollectionName,
                Status = entity.Status,
                Description = entity.Description
            };

            return PartialView("../Contents/GalleryCollection/_Edit", editModel);
        }

        // GET: /Admin/GalleryCollection/List   
        public ActionResult Search(GalleryCollectionSearchEntry entry, string sourceEvent, int? page)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["GalleryCollectionSearchRequest"] = entry;
            }
            else
            {
                if (TempData["GalleryCollectionSearchRequest"] != null)
                {
                    entry = (GalleryCollectionSearchEntry)TempData["GalleryCollectionSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = GalleryService.Search(entry, ref recordCount, "ListOrder DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Contents/GalleryCollection/_SearchResult", lst);
        }

        // Get Hierachical List 
        [HttpGet]
        public ActionResult GetGalleryTopicTreeGrid(int? selectedId, bool? isRootShowed)
        {
            var list = GalleryService.GetGalleryTopicTreeGrid(null, selectedId, isRootShowed);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetGalleryTopicSelectTree(int? selectedId, bool? isRootShowed)
        {
            var list = GalleryService.GetGalleryTopicSelectTree(null, selectedId, isRootShowed);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/GalleryCollection/Create
        [HttpPost]
        public ActionResult Create(GalleryCollectionEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    GalleryService.InsertGalleryCollection(UserId, entry);
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
        // PUT - /Admin/GalleryCollection/Edit
        [HttpPut]
        public ActionResult Edit(GalleryCollectionEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    GalleryService.UpdateGalleryCollection(UserId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.CollectionId
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
        // POST: /Admin/GalleryCollection/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                var galleryStatus = status ? GalleryCollectionStatus.Active : GalleryCollectionStatus.InActive;
                GalleryService.UpdateGalleryCollectionStatus(UserId, id, galleryStatus);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // POST: /Admin/GalleryCollection/Delete/5
        [HttpDelete]
        public ActionResult Delete([System.Web.Http.FromBody]int id)
        {
            try
            {
                GalleryService.DeleteGalleryCollection(UserId, id);
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
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}