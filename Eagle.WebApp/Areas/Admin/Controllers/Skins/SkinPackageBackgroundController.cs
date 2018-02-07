using System;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Skins;
using Eagle.Services.Skins;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Skins
{
    public class SkinPackageBackgroundController : BaseController
    {
        private IThemeService ThemeService { get; set; }
        private IDocumentService DocumentService { get; set; }

        public SkinPackageBackgroundController(IThemeService themeService, IDocumentService documentService)
            : base(new IBaseService[] { themeService, documentService })
        {
            ThemeService = themeService;
            DocumentService = documentService;
        }
       
        //
        // GET: /Admin/SkinBackground/
        public ActionResult Index()
        {
            return View("../Skins/SkinPackageBackground/Index");
        }

        [HttpGet]
        public ActionResult Create()
        {
            var entry = new SkinPackageBackgroundEntry();
            ViewBag.TypeId = ThemeService.PopulateSkinPackageTypeSelectList();
            return PartialView("../Skins/SkinPackageBackground/_Create", entry);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = ThemeService.GetSkinPackageBackgroundDetail(id);
            var model = new SkinPackageBackgroundEditEntry
            {
                TypeId = item.TypeId,
                PackageId = item.PackageId,
                BackgroundId = item.BackgroundId,
                BackgroundFile = item.BackgroundFile,
                BackgroundLink = item.BackgroundLink,
                IsExternalLink = item.IsExternalLink,
                IsActive = item.IsActive
            };

            if (item.BackgroundFile != null)
            {
                var fileInfo = DocumentService.GetFileInfoDetail(Convert.ToInt32(item.BackgroundFile));
                if (fileInfo != null)
                {
                    model.FileUrl = fileInfo.FileUrl;
                }
            }
           
            ViewBag.TypeId = ThemeService.PopulateSkinPackageTypeSelectList(item.Package.TypeId);
            //ViewBag.PackageId = ThemeService.PopulateSkinPackageSelectList(item.Package.TypeId, null, item.PackageId);
            return PartialView("../Skins/SkinPackageBackground/_Edit", model);
        }


        // GET: /Admin/SkinPackageBackground/Search
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            var searchModel = new SkinPackageBackgroundSearchEntry();
            //ViewBag.SearchPackageId = ThemeService.PopulateSkinPackageSelectList();
            ViewBag.SearchTypeId = ThemeService.PopulateSkinPackageTypeSelectList();
            ViewBag.SearchStatus = ThemeService.PopulateSkinPackageBackgroundStatus();

            return PartialView("../Skins/SkinPackageBackground/_SearchForm", searchModel);
        }

        [HttpGet]
        public ActionResult Search([System.Web.Http.FromBody]SkinPackageBackgroundSearchEntry filter, string sourceEvent, [System.Web.Http.FromUri]int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["SkinPackageBackgroundSearchRequest"] = filter;
                TempData.Keep();
            }
            else
            {
                if (TempData["SkinPackageBackgroundSearchRequest"] != null && page > 1)
                {
                    filter = (SkinPackageBackgroundSearchEntry)TempData["SkinPackageBackgroundSearchRequest"];
                    TempData.Keep();
                }
            }

            int recordCount;
            int pageSize = GlobalSettings.DefaultPageSize;
            var sources = ThemeService.GetSkinPackageBackgrounds(filter, out recordCount, "ListOrder DESC", page, pageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, pageSize, recordCount);
            return PartialView("../Skins/SkinPackageBackground/_SearchResult", lst);
        }

        [HttpGet]
        public ActionResult PopulateSkinPackageSelectList(int? typeId, int? selectedValue = null)
        {
            var lst = ThemeService.PopulateSkinPackageSelectList(ApplicationId, typeId, null, selectedValue);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/SkinPackageBackground/Create
        [HttpPost]
        public ActionResult Create(SkinPackageBackgroundEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ThemeService.InsertSkinPackageBackground(ApplicationId, UserId, entry);
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


        // PUT - /Admin/SkinPackageBackground/Edit
        [HttpPut]
        public ActionResult Edit(SkinPackageBackgroundEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ThemeService.UpdateSkinPackageBackground(ApplicationId, UserId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.BackgroundId
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


        // PUT: /Admin/SkinPackageBackground/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                ThemeService.UpdateSkinPackageBackgroundStatus(id, status);
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
                    DocumentService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
