using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Skins;
using Eagle.Services.Skins;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;
using Eagle.WebApp.Attributes.Session;

namespace Eagle.WebApp.Areas.Admin.Controllers.Skins
{
    public class SkinPackageTemplateController : BaseController
    {
        private IThemeService ThemeService { get; set; }

        public SkinPackageTemplateController(IThemeService themeService)
            : base(new IBaseService[] { themeService })
        {
            ThemeService = themeService;
        }
        //
        // GET: /Admin/SkinPackageTemplate/
        [SessionExpiration]
        public ActionResult Index()
        {
            return View("../Skins/SkinPackageTemplate/Index");
        }

        [HttpGet]
        public ActionResult Create()
        {
            var entry = new SkinPackageTemplateEntry();
            ViewBag.TypeId = ThemeService.PopulateSkinPackageTypeSelectList();
            return PartialView("../Skins/SkinPackageTemplate/_Create", entry);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = ThemeService.GetSkinPackageTemplateDetail(id);
            var model = new SkinPackageTemplateEditEntry
            {
                TypeId = item.TypeId,
                PackageId = item.PackageId,
                TemplateId = item.TemplateId,
                TemplateName = item.TemplateName,
                TemplateKey = item.TemplateKey,
                TemplateSrc = item.TemplateSrc,
                IsActive = item.IsActive
            };

            ViewBag.TypeId = ThemeService.PopulateSkinPackageTypeSelectList(item.TypeId);
            //ViewBag.PackageId = ThemeService.PopulateSkinPackageSelectList(item.TypeId,null,item.PackageId);
            return PartialView("../Skins/SkinPackageTemplate/_Edit", model);
        }

        // GET: /Admin/SkinPackageTemplate/Search
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            ViewBag.SearchTypeId = ThemeService.PopulateSkinPackageTypeSelectList();
            ViewBag.SearchStatus = ThemeService.PopulateSkinPackageStatus();
            return PartialView("../Skins/SkinPackageTemplate/_SearchForm");
        }

        [HttpGet]
        public ActionResult Search([System.Web.Http.FromBody]SkinPackageTemplateSearchEntry filter, string sourceEvent, [System.Web.Http.FromUri]int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["SkinPackageTemplateSearchRequest"] = filter;
                TempData.Keep();
            }
            else
            {
                if (TempData["SkinPackageTemplateSearchRequest"] != null && page > 1)
                {
                    filter = (SkinPackageTemplateSearchEntry)TempData["SkinPackageTemplateSearchRequest"];
                    TempData.Keep();
                }
            }

            int recordCount;
            var lst = ThemeService.GetSkinPackageTemplates(filter, out recordCount, "PackageId DESC, TemplateId ASC", page, GlobalSettings.DefaultPageSize);
            return PartialView("../Skins/SkinPackageTemplate/_SearchResult", lst);
        }


        [HttpGet]
        public ActionResult PopulateSkinPackageSelectList(int? typeId, bool? status = null, int? selectedValue = null)
        {
            var lst = ThemeService.PopulateSkinPackageSelectList(ApplicationId, typeId,status, selectedValue);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/SkinPackageTemplate/Create
        [HttpPost]
        public ActionResult Create(SkinPackageTemplateEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ThemeService.InsertSkinPackageTemplate(entry);
                    return Json(new SuccessResult { Status = ResultStatusType.Success, Data = new { Message = LanguageResource.CreateSuccess } }, JsonRequestBehavior.AllowGet);
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


        // PUT - /Admin/SkinPackageTemplate/Edit
        [HttpPut]
        public ActionResult Edit(SkinPackageTemplateEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ThemeService.UpdateSkinPackageTemplate(entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.TemplateId
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


        // PUT: /Admin/SkinPackageTemplate/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                ThemeService.UpdateSkinPackageTemplateStatus(id, status);
                return Json(new SuccessResult
                {
                    Status = ResultStatusType.Success,
                    Data = new
                    {
                        Message = LanguageResource.UpdateStatusSuccess,
                        Id = id
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion =====================================================================================

        #region Dispose

        private bool _disposed;

        [NonAction]
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    ThemeService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
