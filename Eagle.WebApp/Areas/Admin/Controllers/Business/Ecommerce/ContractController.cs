using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Resources;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Business.Personnel;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;
using System;
using Eagle.Services;

namespace Eagle.WebApp.Areas.Admin.Controllers.Business.Ecommerce
{
    public class ContractController : BaseController
    {
        private IContractService ContractService { get; set; }
        private ICurrencyService CurrencyService { get; set; }
        private IEmployeeService EmployeeService { get; set; }
       
        public ContractController(IContractService contractService, IEmployeeService employeeService, ICurrencyService currencyService)
            : base(new IBaseService[] { contractService, employeeService, currencyService })
        {
            ContractService = contractService;
            EmployeeService = employeeService;
            CurrencyService = currencyService;
        }

        #region Choose Employee
        
        // GET: /Admin/Employee/Details/5        
        [HttpGet]
        public ActionResult GetEmployeeDetail(int id)
        {
            var employee = EmployeeService.GetEmployeeDetails(id);
            return Json(employee, JsonRequestBehavior.AllowGet);
        }

        // GET: /Admin/Contract/LoadSearchEmployeeForm
        [HttpGet]
        public ActionResult LoadSearchEmployeeForm()
        {
            return PartialView("../Business/Ecommerce/Personnel/Contract/Employee/_SearchEmployeeForm");
        }

        // GET: /Admin/Contract/Search
        [HttpGet]
        public ActionResult SearchEmployee(EmployeeSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (String.IsNullOrEmpty(sourceEvent))
            {
                TempData["EmployeeSearchRequest"] = filter;
            }
            else
            {
                if (TempData["EmployeeSearchRequest"] != null)
                {
                    filter = (EmployeeSearchEntry)TempData["EmployeeSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = EmployeeService.GetEmployees(VendorId, filter, ref recordCount, "EmployeeId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Business/Ecommerce/Personnel/Contract/Employee/_SearchEmployeeResult", lst);
        }
     
        #endregion

        #region GET METHODS =========================================================================

        // GET: Admin/Contract
        public ActionResult Index()
        {
            return View("../Business/Ecommerce/Personnel/Contract/Index");
        }

        // GET: /Admin/Contract/Create
        [HttpGet]
        public ActionResult Create()
        {
            var model = new ContractEntry
            {
                ContractNo = ContractService.GenerateCode(15),
                CurrencyCode = CurrencyService.GetSelectedCurrency().CurrencyCode
            };
           
            ViewBag.PositionId = EmployeeService.PoplulateJobPositionSelectList(true);
            ViewBag.IsActive = ContractService.PopulateContractStatus();
            return PartialView("../Business/Ecommerce/Personnel/Contract/_Create", model);
        }

        [HttpGet]
        public ActionResult GenerateCode()
        {
            var contractNo = ContractService.GenerateCode(15);
            return Json(contractNo, JsonRequestBehavior.AllowGet);
        }


        // GET: /Admin/Contract/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = ContractService.GetContractDetails(id);

            var model = new ContractEditEntry
            {
                ContractId = entity.ContractId,
                ContractNo = entity.ContractNo,
                CompanyId = entity.CompanyId,
                EmployeeId = entity.EmployeeId,
                PositionId = entity.PositionId,
                CurrencyCode = entity.CurrencyCode,
                ProbationSalary = entity.ProbationSalary,
                InsuranceSalary = entity.InsuranceSalary,
                ProbationFrom = entity.ProbationFrom,
                ProbationTo = entity.ProbationTo,
                SignedDate = entity.SignedDate,
                EffectiveDate = entity.EffectiveDate,
                ExpiredDate = entity.ExpiredDate,
                Description = entity.Description,
                IsActive = entity.IsActive,
                EmployeeName = $"{entity.Contact.FirstName} {entity.Contact.LastName}"
            };

            ViewBag.PositionId = EmployeeService.PoplulateJobPositionSelectList(true,entity.PositionId);
            ViewBag.IsActive = ContractService.PopulateContractStatus(entity.IsActive);
            return PartialView("../Business/Ecommerce/Personnel/Contract/_Edit", model);
        }

        // GET: /Admin/Contract/Search       
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            ViewBag.PositionId = EmployeeService.PoplulateJobPositionSelectList();
            ViewBag.IsActive = ContractService.PopulateContractStatus(null,true);
            return PartialView("../Business/Ecommerce/Personnel/Contract/_SearchForm");
        }

        // GET: /Admin/Contract/Search
        [HttpGet]
        public ActionResult Search(ContractSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["ContractSearchRequest"] = filter;
            }
            else
            {
                if (TempData["ContractSearchRequest"] != null)
                {
                    filter = (ContractSearchEntry)TempData["ContractSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = ContractService.GetContracts(filter, ref recordCount, null, page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Business/Ecommerce/Personnel/Contract/_SearchResult", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/Contract/Create
        [HttpPost]
        public ActionResult Create(ContractEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContractService.InsertContract(entry);
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
        // PUT - /Admin/Contract/Edit
        [HttpPut]
        public ActionResult Edit(ContractEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContractService.UpdateContract(entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ContractId
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
        // POST: /Admin/Contract/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                ContractService.UpdateContractStatus(id, status);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/Contract/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                ContractService.UpdateContractStatus(id, false);
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
                    ContractService = null;
                    CurrencyService = null;
                    EmployeeService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}