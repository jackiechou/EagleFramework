using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Business.Personnel;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Business.Ecommerce
{
    public class JobPositionController : BaseController
    {
        private IEmployeeService EmployeeService { get; set; }
        public JobPositionController(IEmployeeService employeeService) : base(new IBaseService[] { employeeService })
        {
            EmployeeService = employeeService;
        }
        
        #region GET METHODS =========================================================================

        // GET: Admin/JobPosition
        public ActionResult Index()
        {
            return View("../Business/Ecommerce/Personnel/JobPosition/Index");
        }

        // GET: /Admin/JobPosition/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.PositionId = EmployeeService.PoplulateJobPositionSelectList(true);
            ViewBag.IsActive = EmployeeService.PopulateJobPositionStatus();

            return PartialView("../Business/Ecommerce/Personnel/JobPosition/_Create");
        }

        // GET: /Admin/JobPosition/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = EmployeeService.GetJobPositionDetail(id);

            var editModel = new JobPositionEditEntry
            {
                PositionId = entity.PositionId,
                PositionName = entity.PositionName,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            ViewBag.PositionId = EmployeeService.PoplulateJobPositionSelectList(null,entity.PositionId);
            ViewBag.IsActive = EmployeeService.PopulateJobPositionStatus(entity.IsActive);

            return PartialView("../Business/Ecommerce/Personnel/JobPosition/_Edit", editModel);
        }

        // GET: /Admin/JobPosition/List
        [HttpGet]
        public ActionResult List(JobPositionSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["JobPositionSearchRequest"] = filter;
            }
            else
            {
                if (TempData["JobPositionSearchRequest"] != null)
                {
                    filter = (JobPositionSearchEntry)TempData["JobPositionSearchRequest"];
                }
            }
            TempData.Keep();
            int? recordCount = 0;
            var sources = EmployeeService.GetJobPositions(filter, ref recordCount, "PositionId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Business/Ecommerce/Personnel/JobPosition/_List", lst);
        }

        public ActionResult PoplulatePositionSelectList(bool? status = null, int? selectedValue = null, bool? isShowSelectText = true)
        {
            var list = EmployeeService.PoplulateJobPositionSelectList(status, selectedValue, isShowSelectText);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/JobPosition/Create
        [HttpPost]
        public ActionResult Create(JobPositionEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    EmployeeService.InsertJobPosition(entry);
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
        // PUT - /Admin/JobPosition/Edit
        [HttpPut]
        public ActionResult Edit(JobPositionEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    EmployeeService.UpdateJobPosition(entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.PositionId
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
        // POST: /Admin/JobPosition/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                EmployeeService.UpdateJobPositionStatus(id, status);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/JobPosition/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                EmployeeService.UpdateJobPositionStatus(id, false);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.DeleteSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion =============================================================================================================

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