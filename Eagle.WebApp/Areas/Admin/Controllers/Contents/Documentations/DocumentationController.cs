using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Contents;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Documentation;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;
using Eagle.WebApp.Attributes.ModelState;
using Eagle.WebApp.Attributes.Session;
using System;
using System.Web.Mvc;

namespace Eagle.WebApp.Areas.Admin.Controllers.Contents.Documentations
{
    public class DocumentationController : BaseController
    {
        private ICommonService CommonService { get; set; }
        private IDocumentationService DocumentationService { get; set; }

        public DocumentationController(ICommonService commonService, IDocumentationService documentationService)
            : base(new IBaseService[] { commonService, documentationService })
        {
            CommonService = commonService;
            DocumentationService = documentationService;
        }

        // GET: Admin/Documentation
        public ActionResult Index()
        {
            ViewBag.Status = CommonService.GenerateThreeStatusModeList(LanguageResource.All, true);
            return View("../Contents/Documentation/Index");
        }

        [HttpGet]
        public ActionResult Search(DocumentationSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["SearchDocumentationRequest"] = Newtonsoft.Json.JsonConvert.SerializeObject(filter);
            }
            else
            {
                if (TempData["SearchDocumentationRequest"] != null)
                {
                    filter = (DocumentationSearchEntry)Newtonsoft.Json.JsonConvert.DeserializeObject(TempData["SearchDocumentationRequest"] as string, typeof(DocumentationSearchEntry));
                }
            }
            TempData.Keep();

            int recordCount;
            var sources = DocumentationService.Search(filter, out recordCount, "DocumentationId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            if (sources != null)
            {
                var pageLst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
                return PartialView("../Contents/Documentation/_SearchResult", new DocumentationSearchResult(filter, pageLst));
            }
            else
            {
                return PartialView("../Contents/Documentation/_SearchResult", new DocumentationSearchResult(filter, null));
            }
        }               

        [HttpGet]
        public ActionResult Create()
        {
            var documentationEntry = new DocumentationEntry
            {
                CreatedDate = DateTime.UtcNow
            };

            return PartialView("../Contents/Documentation/_Create", documentationEntry);
        }
        
        // POST: /Admin/Documentation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DocumentationEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DocumentationService.InsertDocumentation(ApplicationId, UserId, VendorId, entry);
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

        // GET: /Admin/Documentation/Details/5        
        [HttpGet]
        [SessionExpiration]
        public ActionResult Edit(int id)
        {
            var item = DocumentationService.GetDocumentationDetail(id);
            var detail = new DocumentationEditEntry
            {
                DocumentationId = item.DocumentationId,
                DocumentInfo = item.DocumentInfo
            };

            return PartialView("../Contents/Documentation/_Edit", detail);
        }

        //// POST: /Admin/Documentation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [IgnoreModelErrors("DocumentInfo")]
        public ActionResult Edit(DocumentationEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DocumentationService.UpdateDocumentation(ApplicationId, UserId, VendorId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.DocumentationId
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

        // POST: /Admin/Documentation/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, DocumentationStatus status)
        {
            try
            {
                DocumentationService.UpdateDocumentationStatus(UserId, id, status);
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

        // POST: /Admin/Documentation/Delete/5
        //[HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                DocumentationService.DeleteDocumentation(id);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
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
                    CommonService = null;
                    DocumentationService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}