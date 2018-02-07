using System.Web.Mvc;
using Eagle.Core.Extension;
using Eagle.Core.Pagination;
using Eagle.Core.Search;
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
    public class GalleryTopicController : BaseController
    {
        private IGalleryService GalleryService { get; set; }

        public GalleryTopicController(IGalleryService galleryService) : base(new IBaseService[] { galleryService })
        {
            GalleryService = galleryService;
        }
        // GET: Admin/GalleryTopic
        public ActionResult Index()
        {
            if (Request.IsAjaxRequest())
                return PartialView("../Contents/GalleryTopic/_Reset");
            else
                return View("../Contents/GalleryTopic/Index");
        }

        public ActionResult List()
        {
            var sources = new SearchDataResult<TreeGrid>()
            {
                PaginatedList = new PaginatedList()
            };
            return PartialView("../Contents/GalleryTopic/_List", sources);
        }

        // Get Hierachical List 
        [HttpGet]
        public ActionResult GetGalleryTopicSelectTree(int? selectedId, bool? isRootShowed)
        {
            var list = GalleryService.GetGalleryTopicSelectTree(null, selectedId, isRootShowed);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetGalleryTopicTreeGrid()
        {
            var list = GalleryService.GetGalleryTopicTreeGrid(null,null,null);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult PopulateHierachicalTopic()
        //{
        //    var lst = GalleryService.PopulateHierachicalGalleryTopicDropDownList();
        //    return Json(lst, JsonRequestBehavior.AllowGet);
        //}

        //
        // GET: /Admin/GalleryTopic/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = GalleryService.GetGalleryTopicDetail(id);

            var entry = new GalleryTopicEditEntry
            {
                TopicId = item.TopicId,
                TopicName = item.TopicName,
                TopicCode = item.TopicCode,
                ParentId = item.ParentId,
                Description = item.Description,
                Status = item.Status
            };

            return PartialView("../Contents/GalleryTopic/_Edit", entry);
        }

        //
        // POST: /Admin/GalleryTopic/Create-
        [HttpPost]
        public ActionResult Create(GalleryTopicEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    GalleryService.InsertGalleryTopic(UserId, entry);
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

        ////
        //// PUT: /Admin/GalleryTopic/Edit/5
        [HttpPut]
        public ActionResult Edit(GalleryTopicEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    GalleryService.UpdateGalleryTopic(UserId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.TopicId
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

        //////
        ////// PUT: /Admin/GalleryTopic/UpdateListOrder
        //[HttpPut]
        //public ActionResult UpdateGalleryTopicListOrder(GalleryTopicListOrderEntry listOrderEntry)
        //{
        //    bool flag = false;
        //    string message = string.Empty;
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            GalleryService.UpdateGalleryTopicListOrders(listOrderEntry);
        //            flag = true;
        //            message = LanguageResource.UpdateStatusSuccess;
        //        }
        //        else
        //        {
        //            var errors = ModelState.Values.SelectMany(v => v.Errors);
        //            message = errors.Aggregate(message, (current, modelError) => current + (modelError.ErrorMessage + "<br/>"));
        //        }
        //    }
        //    catch (ValidationError ex)
        //    {
        //        message = ValidationExtension.ConvertValidateErrorToString(ex);
        //    }
        //    return Json(JsonUtils.SerializeResult(flag, message), JsonRequestBehavior.AllowGet);
        //}

        public ActionResult UpdateStatus(int id, int status)
        {
            try
            {
                GalleryService.UpdateGalleryTopicStatus(UserId, id, (GalleryTopicStatus)status);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }
        
        //
        // POST: /Admin/GalleryTopic/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                GalleryService.UpdateGalleryTopicStatus(UserId, id, GalleryTopicStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.DeleteSuccess }, JsonRequestBehavior.AllowGet);

            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

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