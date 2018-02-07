using System;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Business.Personnel;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.Dtos.SystemManagement.Identity;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Business.Ecommerce
{
    public class EmployeeController : BaseController
    {
        private IContactService ContactService { get; set; }
        private ICurrencyService CurrencyService { get; set; }
        public IDocumentService DocumentService { get; set; }
        private IEmployeeService EmployeeService { get; set; }
        private ILanguageService LanguageService { get; set; }
        
        public EmployeeController(IEmployeeService employeeService, IContactService contactService, IDocumentService documentService, 
            ILanguageService languageService, ICurrencyService currencyService)
            : base(new IBaseService[] { employeeService, contactService, documentService, languageService, currencyService })
        {
            EmployeeService = employeeService;
            LanguageService = languageService;
            ContactService = contactService;
            CurrencyService = currencyService;
            DocumentService = documentService;
        }

        #region GET METHODS =========================================================================
        //
        // GET: /Admin/Employee/
        public ActionResult Index()
        {
            return View("../Business/Ecommerce/Personnel/Employee/Index");
        }

        // GET: /Admin/Employee/Create       
        [HttpGet]
        public ActionResult Create()
        {
            var employeeNo = EmployeeService.GenerateCode(15);
            var model = new EmployeeEntry
            {
                EmployeeNo = employeeNo
            };
            
            ViewBag.PostionId = EmployeeService.PoplulateJobPositionSelectList();
            return PartialView("../Business/Ecommerce/Personnel/Employee/_Create", model);
        }


        [HttpGet]
        public ActionResult GenerateCode()
        {
            var employeeNo = EmployeeService.GenerateCode(15);
            return Json(employeeNo, JsonRequestBehavior.AllowGet);
        }

        // GET: /Admin/Employee/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var editModel = new EmployeeEditEntry();
            var entity = EmployeeService.GetEmployeeDetail(id);
            if (entity != null)
            {
                editModel.EmployeeId = entity.EmployeeId;
                editModel.EmployeeNo = entity.EmployeeNo;
                editModel.ContactId = entity.ContactId;
                editModel.VendorId = entity.VendorId;
                editModel.CompanyId = entity.CompanyId;
                editModel.PositionId = entity.PositionId;
                editModel.PasswordHash = entity.PasswordHash;
                editModel.PasswordSalt = entity.PasswordSalt;
                editModel.JoinedDate = entity.JoinedDate;
                editModel.Status = entity.Status;

                if (entity.Contact != null)
                {
                    editModel.Contact = new ContactEditEntry
                    {
                        ContactId = entity.Contact.ContactId,
                        FirstName = entity.Contact.FirstName,
                        LastName = entity.Contact.LastName,
                        DisplayName = entity.Contact.DisplayName,
                        Sex = entity.Contact.Sex,
                        Email = entity.Contact.Email,
                        LinePhone1 = entity.Contact.LinePhone1,
                        LinePhone2 = entity.Contact.LinePhone2,
                        Mobile = entity.Contact.Mobile,
                        Dob = entity.Contact.Dob,
                        Photo = entity.Contact.Photo,
                        JobTitle = entity.Contact.JobTitle,
                        Fax = entity.Contact.Fax,
                        Website = entity.Contact.Website,
                        IdNo = entity.Contact.IdNo,
                        IdIssuedDate = entity.Contact.IdIssuedDate,
                        TaxNo = entity.Contact.TaxNo,
                        FileUrl = entity.Contact.Photo != null && entity.Contact.Photo > 0 ? DocumentService.GetFileInfoDetail(Convert.ToInt32(entity.Contact.Photo)).FileUrl : GlobalSettings.NotFoundFileUrl
                    };
                }

                if (entity.EmergencyAddress != null)
                {
                    editModel.EmergencyAddress = new EmergencyAddressEditEntry
                    {
                        AddressId = entity.EmergencyAddress.AddressId,
                        AddressTypeId = entity.EmergencyAddress.AddressTypeId,
                        CountryId = entity.EmergencyAddress.CountryId,
                        ProvinceId = entity.EmergencyAddress.ProvinceId,
                        RegionId = entity.EmergencyAddress.RegionId,
                        Street = entity.EmergencyAddress.Street,
                        PostalCode = entity.EmergencyAddress.PostalCode,
                        Description = entity.EmergencyAddress.Description
                    };
                }

                if (entity.PermanentAddress != null)
                {
                    editModel.PermanentAddress = new PermanentAddressEditEntry
                    {
                        AddressId = entity.PermanentAddress.AddressId,
                        AddressTypeId = entity.PermanentAddress.AddressTypeId,
                        CountryId = entity.PermanentAddress.CountryId,
                        ProvinceId = entity.PermanentAddress.ProvinceId,
                        RegionId = entity.PermanentAddress.RegionId,
                        Street = entity.PermanentAddress.Street,
                        PostalCode = entity.PermanentAddress.PostalCode,
                        Description = entity.PermanentAddress.Description
                    };
                }
            }

            ViewBag.PostionId = EmployeeService.PoplulateJobPositionSelectList(null, entity.PositionId);
            return PartialView("../Business/Ecommerce/Personnel/Employee/_Edit", editModel);
        }

        // GET: /Admin/Employee/Search
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            return PartialView("../Business/Ecommerce/Personnel/Employee/_SearchForm");
        }

        // GET: /Admin/Employee/Search
        [HttpGet]
        public ActionResult Search(EmployeeSearchEntry filter, string sourceEvent, int? page = 1)
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
            return PartialView("../Business/Ecommerce/Personnel/Employee/_SearchResult", lst);
        }

        [HttpGet]
        public ActionResult LoadContact(int? contactId = null)
        {
            if (contactId != null && contactId > 0)
            {
                var contact = ContactService.GetContactDetails(Convert.ToInt32(contactId));
                var contactEdit = new ContactEditEntry
                {
                    ContactId = contact.ContactId,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    DisplayName = contact.DisplayName,
                    Sex = contact.Sex,
                    Email = contact.Email,
                    LinePhone1 = contact.LinePhone1,
                    LinePhone2 = contact.LinePhone2,
                    Mobile = contact.Mobile,
                    Dob = contact.Dob,
                    Photo = contact.Photo,
                    JobTitle = contact.JobTitle,
                    Fax = contact.Fax,
                    Website = contact.Website,
                    IdNo = contact.IdNo,
                    IdIssuedDate = contact.IdIssuedDate,
                    TaxNo = contact.TaxNo,
                    FileUrl = contact.Photo != null && contact.Photo > 0 ? DocumentService.GetFileInfoDetail(Convert.ToInt32(contact.Photo)).FileUrl : GlobalSettings.NotFoundFileUrl
                };
                var model = new EmployeeEditEntry { Contact = contactEdit };
                return PartialView("../Business/Ecommerce/Personnel/Employee/Contact/_ContactInfoEdit", model);
            }
            else
            {
                var model = new EmployeeEntry();
                return PartialView("../Business/Ecommerce/Personnel/Employee/Contact/_ContactInfo", model);
            }
        }

        [HttpGet]
        public ActionResult LoadEmergencyAddress()
        {
            var model = new EmployeeEntry();
            return PartialView("../Business/Ecommerce/Personnel/Employee/Contact/_EmergencyAddress", model);
        }

        [HttpGet]
        public ActionResult LoadEmergencyAddressEdit(int? addressId = null)
        {
            var emergencyAddressEditEntry = new EmergencyAddressEditEntry {AddressTypeId = AddressType.Emergency};

            if (addressId != null && addressId > 0)
            {
                var address = ContactService.GetAddressDetails(Convert.ToInt32(addressId));
                emergencyAddressEditEntry.AddressId = address.AddressId;
                emergencyAddressEditEntry.CountryId = address.CountryId;
                emergencyAddressEditEntry.ProvinceId = address.ProvinceId;
                emergencyAddressEditEntry.RegionId = address.RegionId;
                emergencyAddressEditEntry.Street = address.Street;
                emergencyAddressEditEntry.PostalCode = address.PostalCode;
                emergencyAddressEditEntry.Description = address.Description;
            }
            var model = new EmployeeEditEntry
            {
                EmergencyAddress = emergencyAddressEditEntry
            };
            return PartialView("../Business/Ecommerce/Personnel/Employee/Contact/_EmergencyAddressEdit", model);
        }

        [HttpGet]
        public ActionResult LoadPermanentAddress()
        {
            var model = new EmployeeEntry();
            return PartialView("../Business/Ecommerce/Personnel/Employee/Contact/_PermanentAddress", model);
        }

        [HttpGet]
        public ActionResult LoadPermanentAddressEdit(int? addressId = null)
        {
            var permanentAddressEditEntry = new PermanentAddressEditEntry { AddressTypeId = AddressType.Permanent };
            if (addressId != null && addressId > 0)
            {
                var address = ContactService.GetAddressDetails(Convert.ToInt32(addressId));
                permanentAddressEditEntry.AddressId = address.AddressId;
                permanentAddressEditEntry.CountryId = address.CountryId;
                permanentAddressEditEntry.ProvinceId = address.ProvinceId;
                permanentAddressEditEntry.RegionId = address.RegionId;
                permanentAddressEditEntry.Street = address.Street;
                permanentAddressEditEntry.PostalCode = address.PostalCode;
                permanentAddressEditEntry.Description = address.Description;
            }
            var model = new EmployeeEditEntry
            {
                PermanentAddress = permanentAddressEditEntry
            };
            return PartialView("../Business/Ecommerce/Personnel/Employee/Contact/_PermanentAddressEdit", model);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/Employee/Create
        [HttpPost]
        public ActionResult Create(EmployeeEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    EmployeeService.InsertEmployee(ApplicationId, UserId, VendorId, entry);
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
        // PUT - /Admin/Employee/Edit
        [HttpPut]
        public ActionResult Edit(EmployeeEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    EmployeeService.UpdateEmployee(ApplicationId, UserId, VendorId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.EmployeeId
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
        // POST: /Admin/Employee/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                EmployeeService.UpdateEmployeeStatus(UserId, id, status ? EmployeeStatus.Active : EmployeeStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/Employee/Delete/5
        [HttpDelete]
        public ActionResult Delete([System.Web.Http.FromBody]int id)
        {
            try
            {
                EmployeeService.UpdateEmployeeStatus(UserId, id, EmployeeStatus.InActive);
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
                    ContactService = null;
                    CurrencyService = null;
                    DocumentService = null;
                    EmployeeService = null;
                    LanguageService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}