using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;
using Eagle.WebApp.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Business.Ecommerce
{
    public class CustomerController : BaseController
    {
        private ICustomerService CustomerService { get; set; }
        private IContactService ContactService { get; set; }
        private IDocumentService DocumentService { get; set; }

        public CustomerController(ICustomerService customerService, IContactService contactService, IDocumentService documentService)
            : base(new IBaseService[] { customerService, contactService, documentService })
        {
            CustomerService = customerService;
            ContactService = contactService;
            DocumentService = documentService;
        }

        #region GET METHODS =========================================================================

        // GET: Admin/Customer
        public ActionResult Index()
        {
            return View("../Business/Ecommerce/Customer/Index");
        }

        // GET: /Admin/Customer/Create
        [HttpGet]
        public ActionResult Create()
        {
            var model = new CustomerEntry
            {
                CustomerNo = CustomerService.GenerateCode(15)
            };
            ViewBag.CustomerTypeId = CustomerService.PopulateCustomerTypeSelectList(CustomerTypeStatus.Active);
            return PartialView("../Business/Ecommerce/Customer/_Create", model);
        }

        // GET: /Admin/Customer/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = CustomerService.GetCustomerDetail(id);

            var model = new CustomerEditEntry
            {
                CustomerId = item.CustomerId,
                CustomerTypeId = item.CustomerTypeId,
                CustomerNo = item.CustomerNo,
                FirstName = item.FirstName,
                LastName = item.LastName,
                ContactName = item.ContactName,
                Email = item.Email,
                Mobile = item.Mobile,
                WorkPhone = item.WorkPhone,
                Fax = item.Fax,
                CardNo = item.CardNo,
                IdCardNo = item.IdCardNo,
                PassPortNo = item.PassPortNo,
                TaxCode = item.TaxCode,
                Gender = item.Gender,
                BirthDay = item.BirthDay,
                AddressId = item.AddressId,
                IsActive = item.IsActive
            };

            if (item.Photo != null)
            {
                var fileInfo = DocumentService.GetFileInfoDetail((int)item.Photo);
                if (fileInfo != null)
                {
                    model.FileUrl = fileInfo.FileUrl;
                }
            }

            if (item.AddressId != null && item.AddressId > 0)
            {
                var address = ContactService.GetAddressDetails((int)item.AddressId);
                model.Address = new CustomerAddressEditEntry
                {
                    AddressId = address.AddressId,
                    AddressTypeId = address.AddressTypeId,
                    CountryId = address.CountryId,
                    ProvinceId = address.ProvinceId,
                    RegionId = address.RegionId,
                    Street = address.Street,
                    PostalCode = address.PostalCode,
                    Description = address.Description
                };
            }

            ViewBag.CustomerTypeId = CustomerService.PopulateCustomerTypeSelectList(CustomerTypeStatus.Active, item.CustomerTypeId);
            return PartialView("../Business/Ecommerce/Customer/_Edit", model);
        }

        // GET: /Admin/Customer/Search
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            ViewBag.CustomerTypeId = CustomerService.PopulateCustomerTypeSelectList(CustomerTypeStatus.Active, null, true);
            return PartialView("../Business/Ecommerce/Customer/_SearchForm");
        }

        // GET: /Admin/Customer/List
        [HttpGet]
        public ActionResult Search(CustomerSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (string.IsNullOrEmpty(sourceEvent))
            {
                TempData["CustomerSearchRequest"] = filter;
            }   
            else
            {
                if (TempData["CustomerSearchRequest"] != null)
                {
                    filter = (CustomerSearchEntry)TempData["CustomerSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = CustomerService.GetCustomers(filter, ref recordCount, "CustomerId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Business/Ecommerce/Customer/_SearchResult", lst);
        }


        /// <summary>
        /// AutoComplete with select2
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCustomerAutoCompleteList(string searchTerm, int? page)
        {
            int recordCount;
            var jsonList = CustomerService.GetCustomerAutoCompleteList(searchTerm, CustomerStatus.Published, out recordCount, "CustomerId DESC", page);
            return new JsonpResult { Data = jsonList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult GenerateCode()
        {
            var customerNo = CustomerService.GenerateCode(15);
            return Json(customerNo);
        }
        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/Customer/Create
        [HttpPost]
        public ActionResult Create(CustomerEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CustomerService.InsertCustomer(ApplicationId, UserId, VendorId, entry);
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
        // PUT - /Admin/Customer/Edit
        [HttpPut]
        public ActionResult Edit(CustomerEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CustomerService.UpdateCustomer(ApplicationId, UserId, VendorId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.CustomerId
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
        // POST: /Admin/Customer/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, int status)
        {
            try
            {
                CustomerService.UpdateCustomerStatus(UserId, id, (CustomerStatus)status);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/Customer/Delete/5
        [HttpDelete]
        public ActionResult Delete([System.Web.Http.FromBody]int id)
        {
            try
            {
                CustomerService.UpdateCustomerStatus(UserId, id, CustomerStatus.InActive);
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
                    CustomerService = null;
                    ContactService = null;
                    DocumentService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}