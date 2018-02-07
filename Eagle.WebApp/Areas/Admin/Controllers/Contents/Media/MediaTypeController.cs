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
    public class MediaTypeController : BaseController
    {
        private IMediaService MediaService { get; set; }

        public MediaTypeController(IMediaService mediaService) : base(new IBaseService[] { mediaService })
        {
            MediaService = mediaService;
        }

        #region GET METHODS =========================================================================
        //
        // GET: /Admin/MediaType/
        public ActionResult Index()
        {
            return View("../Contents/Media/MediaType/Index");
        }

        // GET: /Admin/MediaType/Create
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("../Contents/Media/MediaType/_Create");
        }

        // GET: /Admin/MediaType/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = MediaService.GetMediaTypeDetail(id);

            var editModel = new MediaTypeEditEntry
            {
                TypeId = entity.TypeId,
                TypeName = entity.TypeName,
                TypeExtension = entity.TypeExtension,
                TypePath = entity.TypePath,
                Description = entity.Description,
                Status = entity.Status
            };
            return PartialView("../Contents/Media/MediaType/_Edit", editModel);
        }

        // GET: /Admin/MediaType/List
        [HttpGet]
        public ActionResult List(int? page = 1)
        {
            int? recordCount = 0;
            var sources = MediaService.GetMediaTypes(null, ref recordCount, page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Contents/Media/MediaType/_List", lst);
        }

        #endregion =====================================================================================
        
        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/MediaType/Create
        [HttpPost]
        public ActionResult Create(MediaTypeEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MediaService.InsertMediaType(UserId, entry);
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
        // PUT - /Admin/MediaType/Edit
        [HttpPut]
        public ActionResult Edit(MediaTypeEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MediaService.UpdateMediaType(UserId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.TypeId
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
        // POST: /Admin/MediaType/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                MediaService.UpdateMediaTypeStatus(UserId, id, status? MediaTypeStatus.Active: MediaTypeStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/MediaType/Delete/5
        [HttpDelete]
        public ActionResult Delete([System.Web.Http.FromBody]int id)
        {
            try
            {
                MediaService.UpdateMediaTypeStatus(UserId, id, MediaTypeStatus.InActive);
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
                    MediaService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}