using System.Web.Mvc;
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
    public class MediaTopicController : BaseController
    {
        private IMediaService MediaService { get; set; }

        public MediaTopicController(IMediaService mediaService) : base(new IBaseService[] { mediaService })
        {
            MediaService = mediaService;
        }
        // GET: Admin/MediaTopic
        public ActionResult Index()
        {
            return View("../Contents/Media/MediaTopic/Index");
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


        //
        // GET: /Admin/MediaTopic/Create       
        [HttpGet]
        public ActionResult Create()
        {
            //ViewBag.TypeId = MediaService.PoplulateMediaTypeSelectList(null, true, MediaTypeStatus.Active);
            return PartialView("../Contents/Media/MediaTopic/_Create");
        }
        //
        // GET: /Admin/MediaTopic/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = MediaService.GetMediaTopicDetail(id);

            var entry = new MediaTopicEditEntry
            {
                TypeId = item.TypeId,
                TopicId = item.TopicId,
                TopicName = item.TopicName,
                ParentId = item.ParentId,
                Description = item.Description,
                Icon = item.Icon,
                Status = item.Status
            };
            ViewBag.TypeId = MediaService.PoplulateMediaTypeSelectList(item.TypeId, true, MediaTypeStatus.Active);
            return PartialView("../Contents/Media/MediaTopic/_Edit", entry);
        }


        //
        // POST: /Admin/MediaTopic/Create-
        [HttpPost]
        public ActionResult Create(MediaTopicEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MediaService.InsertMediaTopic(UserId, entry);
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
        //// PUT: /Admin/MediaTopic/Edit/5
        [HttpPut]
        public ActionResult Edit(MediaTopicEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MediaService.UpdateMediaTopic(UserId, entry);
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

        ////
        //// PUT: /Admin/MediaTopic/UpdateListOrder
        [HttpPut]
        public ActionResult UpdateMediaTopicListOrder(int id, int listOrder)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MediaService.UpdateMediaTopicListOrder(UserId, id, listOrder);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = id
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

        // POST: /Admin/MediaTopic/UpdateStatus/5
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                MediaService.UpdateMediaTopicStatus(UserId, id, status? MediaTopicStatus.Active: MediaTopicStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // POST: /Admin/MediaTopic/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                MediaService.UpdateMediaTopicStatus(UserId, id, MediaTopicStatus.InActive);
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
                    MediaService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}