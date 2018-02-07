using System.Web.Mvc;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Business.Personnel;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Business.Ecommerce
{
    public class QualificationController : BaseController
    {
        public IEmployeeService EmployeeService { get; set; }

        public QualificationController(IEmployeeService employeeService) : base(new IBaseService[] { employeeService })
        {
            EmployeeService = employeeService;
        }

        #region GET METHODS =========================================================================

        // GET: Admin/Qualification
        public ActionResult Index()
        {
            return View("../Business/Ecommerce/Personnel/Qualification/Index");
        }

        // GET: /Admin/Qualification/Create
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("../Business/Ecommerce/Personnel/Qualification/_Create");
        }

        //// GET: /Admin/Qualification/Details/5        
        //[HttpGet]
        //public ActionResult Edit(int id)
        //{
        //    var entity = EmployeeService.GetQualificationDetail(id);

        //    var editModel = new QualificationEditEntry
        //    {
        //        EmployeeId = entity.EmployeeId,
        //        QualificationId = entity.QualificationId,
        //        QualificationNo = entity.QualificationNo,
        //        QualificationDate = entity.QualificationDate,
        //        FileId = entity.FileId,
        //        Note = entity.Note
        //    };
        //    return PartialView("../Business/Ecommerce/Personnel/Qualification/_Edit", editModel);
        //}

        // GET: /Admin/Qualification/Search
        //[HttpGet]
        //public ActionResult Search(QualificationSearchEntry filter, int? page = 1)
        //{
        //    if (page == 1)
        //    {
        //        if (filter != null)
        //        {
        //            TempData["QualificationSearchRequest"] = filter;
        //        }
        //    }
        //    else
        //    {
        //        if (TempData["QualificationSearchRequest"] != null)
        //        {
        //            filter = (QualificationSearchEntry)TempData["QualificationSearchRequest"];
        //        }
        //    }
        //    TempData.Keep();

        //    int? recordCount = 0;
        //    var sources = EmployeeService.GetQualifications(filter, ref recordCount, null, page, GlobalSettings.DefaultPageSize);
        //    int currentPageIndex = page - 1 ?? 0;
        //    var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
        //    return PartialView("../Business/Ecommerce/Personnel/Qualification/_List", lst);
        //}

        #endregion =====================================================================================
        
        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/Qualification/Create
        [HttpPost]
        public ActionResult Create(QualificationEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    EmployeeService.InsertQualification(ApplicationId, UserId, entry);
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
        // PUT - /Admin/Qualification/Edit
        [HttpPut]
        public ActionResult Edit(QualificationEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    EmployeeService.UpdateQualification(ApplicationId, UserId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.QualificationId
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
        // DELETE: /Admin/Qualification/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                EmployeeService.DeleteQualification(id);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.DeleteSuccess }, JsonRequestBehavior.AllowGet);
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
                    EmployeeService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}