using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Common.Utilities;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.Dtos.SystemManagement.Identity;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Sys
{
    public class UserController : BaseController
    {
        public IDocumentService DocumentService { get; set; }
        private ISecurityService SecurityService { get; set; }
        private ICurrencyService CurrencyService { get; set; }
        private ILanguageService LanguageService { get; set; }
        private IContactService ContactService { get; set; }
        private IUserService UserService { get; set; }
        private IRoleService RoleService { get; set; }

        public UserController(IUserService userService, IRoleService roleService, IContactService contactService,
            IDocumentService documentService, ILanguageService languageService, ICurrencyService currencyService,
            ISecurityService securityService) : base(new IBaseService[]
        {userService, roleService, contactService, languageService,currencyService,securityService})
        {
            UserService = userService;
            RoleService = roleService;
            LanguageService = languageService;
            ContactService = contactService;
            CurrencyService = currencyService;
            SecurityService = securityService;
            DocumentService = documentService;
        }

        #region GET
        //
        // GET: /Admin/User/
        [HttpGet]
        public ActionResult Index()
        {
            if (Request.IsAjaxRequest())
                return PartialView("../Sys/Users/_Reset");
            else
                return View("../Sys/Users/Index");
        }
        public ActionResult PopulateProvinceSelectList(int countryId, int? selectedValue, bool isShowSelectText = true)
        {
            var sources = ContactService.PopulateProvinceSelectList(countryId, true, selectedValue, isShowSelectText);
            return Json(sources, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult LoadContactForm(int? contactId = null)
        {
            if (contactId != null && contactId > 0)
            {
                var contact = ContactService.GetContactDetails(Convert.ToInt32(contactId));

                var fileUrl = contact.Photo != null && contact.Photo > 0
                    ? DocumentService.GetFileInfoDetail(Convert.ToInt32(contact.Photo)).FileUrl
                    : GlobalSettings.NotFoundFileUrl;

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
                    JobTitle = contact.JobTitle,
                    Photo = contact.Photo,
                    Fax = contact.Fax,
                    Website = contact.Website,
                    IdNo = contact.IdNo,
                    IdIssuedDate = contact.IdIssuedDate,
                    TaxNo = contact.TaxNo,
                    FileUrl = fileUrl
                };
                var model = new UserEditEntry {Contact = contactEdit};
                return PartialView("../Sys/Users/Contact/_ContactInfoEdit", model);
            }
            else
            {
                var model = new UserEntry();
                return PartialView("../Sys/Users/Contact/_ContactInfo", model);
            }
        }

        [HttpGet]
        public ActionResult LoadRoleForm()
        {
            var userEntry = new UserEntry();
            var userRoles = new List<UserRoleCreate>();

            var roles = RoleService.GetRoles(ApplicationId, true).ToList();
            if (roles.Any())
            {
                userRoles.AddRange(roles.Select(role => new UserRoleCreate
                {
                    RoleId = role.RoleId,
                    IsAllowed = false,
                    IsDefaultRole = false,
                    IsTrialUsed = false,
                    EffectiveDate = DateTime.UtcNow,
                    ExpiryDate = null,
                    Role = role
                }));
            }
            userEntry.UserRoles = userRoles;
            return PartialView("../Sys/Users/RoleGroup/_AddRole", userEntry);
        }

        [HttpGet]
        public ActionResult LoadEditRoleForm(Guid userId)
        {
            var userEditEntry = new UserEditEntry();
            var userEditRoles = new List<UserRoleEdit>();

            var roles = RoleService.GetRoles(ApplicationId, true).ToList();
            var selectedRoles = RoleService.GetUserRolesByUserId(ApplicationId, userId).ToList();

            if (selectedRoles.Any())
            {
                var roleIds = roles.Select(x => x.RoleId).ToList();
                var selectedRoleIds = selectedRoles.Select(x => x.RoleId).ToList();

                var differenceRoleIds = roleIds.Except(selectedRoleIds).ToList();
                if (differenceRoleIds.Any())
                {
                    foreach (var roleId in differenceRoleIds)
                    {
                        var differenceRoleGroups = RoleService.GetRoleGroups(ApplicationId, roleId);
                        var differenceUserRoles = (from roleGroup in differenceRoleGroups
                                                   let userRoleGroups = UserService.GeUserRoleGroups(ApplicationId, roleGroup.RoleId, userId).ToList()
                                                   select new UserRoleEdit
                                                   {
                                                       UserId = userId,
                                                       RoleId = roleId,
                                                       IsAllowed = false,
                                                       IsDefaultRole = false,
                                                       IsTrialUsed = false,
                                                       EffectiveDate = null,
                                                       ExpiryDate = null,
                                                       Role = roleGroup.Role,
                                                       UserRoleGroups = userRoleGroups
                                                   }).ToList();
                        userEditRoles.AddRange(differenceUserRoles);
                    }
                }

                var selectedList = (from r in selectedRoles
                                    let userRoleGroups = UserService.GeUserRoleGroups(ApplicationId, r.RoleId, userId).ToList()
                                    select new UserRoleEdit
                                        {
                                            UserId = r.UserId,
                                            RoleId = r.RoleId,
                                            IsAllowed = true,
                                            IsDefaultRole = r.IsDefaultRole ?? false,
                                            IsTrialUsed = r.IsTrialUsed ?? false,
                                            EffectiveDate = r.EffectiveDate,
                                            ExpiryDate = r.ExpiryDate,
                                            Role = r.Role,
                                            UserRoleGroups = userRoleGroups
                                    }).ToList();
                userEditRoles.AddRange(selectedList);
            }
            else
            {
                userEditRoles.AddRange(roles.Select(role => new UserRoleEdit
                {
                    UserId = userId,
                    RoleId = role.RoleId,
                    IsAllowed = false,
                    IsDefaultRole = false,
                    IsTrialUsed = false,
                    EffectiveDate = null,
                    ExpiryDate = null,
                    Role = role
                }));
            }

            userEditEntry.UserRoles = userEditRoles.OrderBy(x=>x.Role.ListOrder).ToList();
            return PartialView("../Sys/Users/RoleGroup/_EditRole", userEditEntry);
        }

        [HttpGet]
        public ActionResult LoadGroupForm(Guid? userId, Guid roleId)
        {
            if (userId == null)
            {
                var userEntry = new UserEntry();
                var userGroups = new List<UserRoleGroupCreate>();
                var roleGroups = RoleService.GetRoleGroups(ApplicationId, roleId, true).ToList();
                if (roleGroups.Any())
                {
                    userGroups.AddRange(roleGroups.Select(group => new UserRoleGroupCreate
                    {
                        RoleGroupId = group.RoleGroupId,
                        IsAllowed = false,
                        IsDefault = false,
                        EffectiveDate = DateTime.UtcNow,
                        ExpiryDate = null,
                        RoleGroup = group
                    }));
                }
                userEntry.UserRoleGroups = userGroups;
                return PartialView("../Sys/Users/RoleGroup/_AddGroup", userEntry);
            }
            else
            {
                var userEditEntry = new UserEditEntry();
                //var userRoleGroups = UserService.GeUserRoleGroups(roleId, (Guid)userId).ToList();
                //userEditEntry.UserRoleGroups = userRoleGroups;
                return PartialView("../Sys/Users/RoleGroup/_EditGroup", userEditEntry);
            }
        }

        [HttpGet]
        public ActionResult PopulateRoleDropDownList()
        {
            var list = RoleService.PopulateRoleDropDownList(null, null, true);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PopulateGroupDropDownList(Guid roleId)
        {
            var list = RoleService.PopulateGroupDropDownList(ApplicationId, roleId, true,true).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            ViewBag.RoleId = RoleService.PopulateRoleDropDownList(null,null,true);

            var statusList = UserService.PopulateStatusSelectList(null, true);
            ViewBag.IsApproved = statusList;
            ViewBag.IsSuperUser = statusList;
            ViewBag.IsLockedOut = statusList;

            return PartialView("../Sys/Users/_SearchForm");
        }

        [HttpGet]
        public ActionResult Search([System.Web.Http.FromBody]UserSearchEntry filter, string sourceEvent, [System.Web.Http.FromUri] int? page = 1)
        {
            if (String.IsNullOrEmpty(sourceEvent))
            {
                TempData["UserSearchRequest"] = filter;
            }
            else
            {
                if (TempData["UserSearchRequest"] != null)
                {
                    filter = (UserSearchEntry)TempData["UserSearchRequest"];
                }
            }
            TempData.Keep();

            int recordCount;
            var sources = UserService.SearchUsers(ApplicationId, filter, out recordCount, "SeqNo DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Sys/Users/_SearchResult", lst);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewData["AvailableRoles"] = RoleService.PopulateRoleMultiSelectList(true, null, null);
            ViewBag.PasswordQuestion = UserService.PopulateQuestionsSelectList(null, true);
            return View("../Sys/Users/Create");
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var item = UserService.GetProfileDetails(id);
            var fileUrl = item.Contact.Photo != null && item.Contact.Photo > 0
                ? DocumentService.GetFileInfoDetail(Convert.ToInt32(item.Contact.Photo)).FileUrl
                : GlobalSettings.NotFoundFileUrl;

            var editModel = new UserEditEntry
            {
                UserId = item.UserId,
                UserName = item.User.UserName,
                PasswordSalt = item.User.PasswordSalt,
                ConfirmedPassword = item.User.PasswordSalt,
                PasswordQuestion = item.User.PasswordQuestion,
                PasswordAnswer = item.User.PasswordAnswer,
                StartDate = item.User.StartDate,
                IsSuperUser = item.User.IsSuperUser,
                Contact = new ContactEditEntry
                {
                    ContactId = item.Contact.ContactId,
                    Email = item.Contact.Email,
                    FirstName = item.Contact.FirstName,
                    LastName = item.Contact.LastName,
                    DisplayName = item.Contact.DisplayName,
                    Sex = item.Contact.Sex,
                    Dob = item.Contact.Dob,
                    JobTitle = item.Contact.JobTitle,
                    Photo = item.Contact.Photo,
                    LinePhone1 = item.Contact.LinePhone1,
                    LinePhone2 = item.Contact.LinePhone2,
                    Mobile = item.Contact.Mobile,
                    Fax = item.Contact.Fax,
                    Website = item.Contact.Website,
                    IdNo = item.Contact.IdNo,
                    IdIssuedDate = item.Contact.IdIssuedDate,
                    TaxNo = item.Contact.TaxNo,
                    FileUrl = fileUrl
                }
            };

            var addresses = item.Addresses;
            if (addresses != null)
            {
                foreach (var address in addresses)
                {
                    if (address.Address.AddressTypeId == AddressType.Emergency)
                    {
                        editModel.EmergencyAddress = new EmergencyAddressEditEntry
                        {
                            AddressId = address.Address.AddressId,
                            AddressTypeId = address.Address.AddressTypeId,
                            CountryId = address.Address.CountryId,
                            ProvinceId = address.Address.ProvinceId,
                            RegionId = address.Address.RegionId,
                            Street = address.Address.Street,
                            PostalCode = address.Address.PostalCode,
                            Description = address.Address.Description
                        };
                    }

                    if (address.Address.AddressTypeId == AddressType.Permanent)
                    {
                        editModel.PermanentAddress = new PermanentAddressEditEntry
                        {
                            AddressId = address.Address.AddressId,
                            AddressTypeId = address.Address.AddressTypeId,
                            CountryId = address.Address.CountryId,
                            ProvinceId = address.Address.ProvinceId,
                            RegionId = address.Address.RegionId,
                            Street = address.Address.Street,
                            PostalCode = address.Address.PostalCode,
                            Description = address.Address.Description
                        };
                    }
                }
            }

            ViewBag.PasswordQuestion = UserService.PopulateQuestionsSelectList(item.User.PasswordQuestion, true);
            //ViewData["AvailableRoles"] = RoleService.PopulateRoleMultiSelectList(true, null, null);
            //ViewData["SelectedRoles"] = RoleService.PopulateSelectedRoleMultiSelectList(id, true, null, null);
            //ViewData["Contact.Sex"] = UserService.PopulateSexSelectList(Convert.ToInt32(item.Contact.Sex), null);

            return View("../Sys/Users/Edit", editModel);
        }


        #endregion

        #region POST
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(UserEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = UserService.CreateUser(ApplicationId, UserId, VendorId, entry);
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

        [HttpPut]
        public ActionResult Edit(UserEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserService.EditUser(ApplicationId, UserId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.UserId
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
                //message = ValidationExtension.ConvertValidateErrorToString(ex);
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            var result = false;
            string message;
            try
            {
                UserService.LockUser(id);
                result = true;
                message = "Update Service Request successfully!";
            }
            catch (ValidationError ex)
            {
                message = ValidationExtension.ConvertValidateErrorToString(ex);
            }
            return Json(JsonUtils.SerializeResult(result, message), JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public ActionResult Approve(Guid userId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserService.Approve(userId);
                    return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
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

        [HttpPut]
        public ActionResult UnApprove(Guid userId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserService.UnApprove(userId);
                    return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
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

        [HttpPut]
        public ActionResult Lock(Guid userId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserService.LockUser(userId);
                    return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
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

        [HttpPut]
        public ActionResult UnLock(Guid userId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserService.UnLockUser(userId);
                    return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
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

        #endregion

        #region Dispose

        private bool _disposed;

        [NonAction]
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    DocumentService = null;
                    SecurityService = null;
                    CurrencyService = null;
                    LanguageService = null;
                    ContactService = null;
                    UserService = null;
                    RoleService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
