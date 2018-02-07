using System.Web.Mvc;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement.FileStorage;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Sys.FileStorage
{
    public class DocumentFolderController : BaseController
    {
        private IDocumentService DocumentService { get; }

        public DocumentFolderController(IDocumentService documentService) : base(new IBaseService[] { documentService })
        {
            DocumentService = documentService;
        }

        #region GET METHODS =========================================================================
        //
        // GET: /Admin/DocumentFolder/
        public ActionResult Index()
        {
            return View("../Sys/FileStorage/DocumentFolder/Index");
        }

        // GET: /Admin/DocumentFolder/Create
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("../Sys/FileStorage/DocumentFolder/_Create");
        }

        // GET: /Admin/DocumentFolder/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = DocumentService.GetFolderDetailByFolderId(id);

            var editModel = new DocumentFolderEditEntry
            {
                FolderId = entity.FolderId,
                ParentId = entity.ParentId,
                FolderName = entity.FolderName,
                FolderPath = entity.FolderPath,
                FolderIcon = entity.FolderIcon,
                Description = entity.Description,
                IsActive = entity.IsActive
            };
            return PartialView("../Sys/FileStorage/DocumentFolder/_Edit", editModel);
        }

        // Get Hierachical List 
        [HttpGet]
        public ActionResult GetDocumentFolderTree(int? selectedId, bool? isRootShowed)
        {
            var list = DocumentService.GetDocumentFolderTree(null, selectedId, isRootShowed);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //// GET: /Admin/DocumentFolder/List
        //[HttpGet]
        //public ActionResult List(int? page = 1)
        //{
        //    int? recordCount = 0;
        //    var sources = DocumentService.GetDocumentFolders(DocumentFolderStatus.Active, ref recordCount, page, GlobalSettings.DefaultPageSize);
        //    int currentPageIndex = page - 1 ?? 0;
        //    var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
        //    return PartialView("../Contents/FileManager/DocumentFolder/_List", lst);
        //}

        #endregion =====================================================================================


        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/DocumentFolder/Create
        [HttpPost]
        public ActionResult Create(DocumentFolderEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DocumentService.InsertDocumentFolder(ApplicationId, UserId, entry);
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
        // PUT - /Admin/DocumentFolder/Edit
        [HttpPut]
        public ActionResult Edit(DocumentFolderEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DocumentService.UpdateDocumentFolder(UserId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.FolderId
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
        //// POST: /Admin/DocumentFolder/UpdateStatus/5
        //[HttpPut]
        //public ActionResult UpdateStatus(int id, int status)
        //{
        //    try
        //    {
        //        DocumentService.UpdateDocumentFolderStatus(id, (DocumentFolderStatus)status);
        //        return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (ValidationError ex)
        //    {
        //        var errorLst = new List<Error>();
        //        var errorExtraInfos = ex.Data["ValidationErrors"] as List<RuleViolation>;
        //        if (errorExtraInfos != null)
        //        {
        //            errorLst.AddRange(errorExtraInfos.Select(error => new Error
        //            {
        //                ErrorMessage = $"{error.PropertyName} - {error.ErrorMessage}"
        //            }));
        //        }
        //        return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errorLst }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //
        // DELETE: /Admin/DocumentFolder/Delete/5
        [HttpDelete]
        public ActionResult Delete([System.Web.Http.FromBody]int id)
        {
            try
            {
                DocumentService.DeleteDocumentFolder(id);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.DeleteSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion ==============================================================================
    }
}