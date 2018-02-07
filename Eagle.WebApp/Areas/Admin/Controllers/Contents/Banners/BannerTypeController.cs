using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Contents;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Banners;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Contents.Banners
{
    [ValidateAntiForgeryTokenOnAllPosts]
    public class BannerTypeController : BaseController
    {
        private IBannerService BannerService { get; set; }
        public BannerTypeController(IBannerService bannerService) : base(new IBaseService[] { bannerService })
        {
            BannerService = bannerService;
        }

        #region GET METHODS =========================================================================
        //
        // GET: /Admin/BannerType/
        public ActionResult Index()
        {
            return View("../Contents/BannerTypes/Index");
        }

        // GET: /Admin/BannerType/Create
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("../Contents/BannerTypes/_Create");
        }

        // GET: /Admin/BannerType/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = BannerService.GetBannerTypeDetails(id);

            var editModel = new BannerTypeEditEntry
            {
                TypeId = entity.TypeId,
                TypeName = entity.TypeName,
                Description = entity.Description,
                Status = entity.Status
            };
            return PartialView("../Contents/BannerTypes/_Edit", editModel);
        }

        // GET: /Admin/BannerType/List
        [HttpGet]
        public ActionResult List(int? page=1)
        {
            int? recordCount = 0;
            var sources = BannerService.GetBannerTypes(ref recordCount, page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Contents/BannerTypes/_List", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/BannerType/Create
        [HttpPost]
        public ActionResult Create(BannerTypeEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BannerService.InsertBannerType(entry);
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
        // PUT - /Admin/BannerType/Edit
        [HttpPut]
        public ActionResult Edit(BannerTypeEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BannerService.UpdateBannerType(entry);
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
        // POST: /Admin/BannerType/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                BannerService.UpdateBannerTypeStatus(id, status? BannerTypeStatus.Active: BannerTypeStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/BannerType/Delete/5
        [HttpDelete]
        public ActionResult Delete([System.Web.Http.FromBody]int id)
        {
            try
            {
                BannerService.UpdateBannerTypeStatus(id, BannerTypeStatus.InActive);
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
                    BannerService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}