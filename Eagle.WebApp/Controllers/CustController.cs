using System;
using System.Web.Mvc;
using System.Web.Security;
using Eagle.Common.Session;
using Eagle.Common.Utilities;
using Eagle.Core.Configuration;
using Eagle.Core.Cookie;
using Eagle.Core.Session;
using Eagle.Resources;
using Eagle.Services.Business;
using Eagle.Services.Common;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;

namespace Eagle.WebApp.Controllers
{
    public class CustController : BasicController
    {
        private IApplicationService ApplicationService { get; set; }
        private ICustomerService CustomerService { get; set; }
        private IContactService ContactService { get; set; }
        private IDocumentService DocumentService { get; set; }

        public CustController(IApplicationService applicationService, ICustomerService customerService, IContactService contactService, IDocumentService documentService)
        {
            ApplicationService = applicationService;
            CustomerService = customerService;
            ContactService = contactService;
            DocumentService = documentService;
        }

        public ActionResult PopulateCountrySelectList(int? selectedValue = null, bool isShowSelectText = true)
        {
            var sources = ContactService.PopulateCountrySelectList(true, selectedValue, isShowSelectText);
            return Json(sources, JsonRequestBehavior.AllowGet);
        }

        // GET: Cust
        public ActionResult Index()
        {
            return View("../Cust/Index");
        }

        #region SIGN IN
        [HttpGet]
        public ActionResult SignIn(string desiredUrl)
        {
            var customerLogin = new CustomerLogin { RememberMe = true };
            var userInfo = Request.Cookies[CookieSetting.CustInfo];
            if (userInfo != null)
            {
                customerLogin.Email = userInfo[CookieSetting.CustInfoEmail];
                customerLogin.Password = (!string.IsNullOrEmpty(userInfo[CookieSetting.CustInfoPassword])) ? StringUtils.DecodePassword(userInfo[CookieSetting.CustInfoPassword]) : null;
            }

            if (string.IsNullOrEmpty(desiredUrl))
            {
                if (Request.Url != null)
                {
                    var url = Request.Url.AbsoluteUri;
                    if (url.Contains("?desiredUrl="))
                    {
                        desiredUrl = url.Substring(url.IndexOf("?desiredUrl=", StringComparison.Ordinal) + 12);
                    }
                }
            }

            customerLogin.DesiredUrl = desiredUrl;
            customerLogin.RememberMe = true;

            return PartialView("../Cust/_SignIn", customerLogin);
        }

        /// <summary>
        /// POST: /Admin/Customer/LogIn
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(CustomerLogin entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CustomerService.SignIn(entry);
                    string desiredUrl = (!string.IsNullOrEmpty(entry.DesiredUrl))
                        ? entry.DesiredUrl
                        : "/Cust/MyProfile";
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.LoginSuccess,
                            ReturnedUrl = desiredUrl
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

        /// <summary>
        /// POST: /Admin/Customer/SignIn
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SignIn(CustomerLogin entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SessionExtension.CustomerInfo = CustomerService.SignIn(entry);
                    if (!string.IsNullOrEmpty(entry.DesiredUrl))
                    {
                        return Redirect(entry.DesiredUrl);
                    }
                    return RedirectToAction("MyProfile", "Cust");
                }
                else
                {
                    this.ShowModelState(ModelState);
                    return RedirectToAction("Index", "Cust");
                }
            }
            catch (ValidationError ex)
            {
                this.ShowException(ex);
                return RedirectToAction("Index", "Cust");
            }
        }

        // GET: Customer Profile
        [HttpGet]
        public ActionResult MyProfile()
        {
            if (SessionExtension.CustomerInfo == null)
            {
                return View("../Cust/Index");
            }
            return View("../Cust/CustOverall", SessionExtension.CustomerInfo);
        }
        #endregion


        [HttpGet]
        public ActionResult SignUp()
        {
            var countrySetting = ApplicationService.GetAddressSetting(GlobalSettings.DefaultApplicationId, "CountryId").Setting;
            var provinceSetting = ApplicationService.GetAddressSetting(GlobalSettings.DefaultApplicationId, "ProvinceId").Setting;
            var postalCodeSetting = ApplicationService.GetAddressSetting(GlobalSettings.DefaultApplicationId, "PostalCode").Setting;

            var customerRegisterEntry = new CustomerRegisterEntry
            {
                Address = new CustomerRegisterAddressEntry
                {
                    CountryId = Convert.ToInt32(countrySetting.KeyValue),
                    ProvinceId = Convert.ToInt32(provinceSetting.KeyValue),
                    PostalCode = postalCodeSetting.KeyValue
                }
            };

            return PartialView("../Cust/_SignUp", customerRegisterEntry);
        }

        // POST: /Cust/Register
        [HttpPost]
        public ActionResult SignUp(CustomerRegisterEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int vendorId = GlobalSettings.DefaultVendorId;
                    var applicationId = GlobalSettings.DefaultApplicationId;

                    var customer = CustomerService.Register(applicationId, vendorId, entry);
                    
                    var customerInfo = new CustomerInfoDetail
                    {
                        CustomerId = customer.CustomerId,
                        CustomerTypeId = customer.CustomerTypeId,
                        AddressId = customer.AddressId,
                        VendorId = customer.VendorId,
                        CustomerNo = customer.CustomerNo,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        PasswordSalt = customer.PasswordSalt,
                        PasswordHash = customer.PasswordHash,
                        CardNo = customer.CardNo,
                        ContactName = customer.ContactName,
                        IdCardNo = customer.IdCardNo,
                        PassPortNo = customer.PassPortNo,
                        TaxCode = customer.TaxCode,
                        Photo = customer.Photo,
                        Gender = customer.Gender,
                        BirthDay = customer.BirthDay,
                        HomePhone = customer.HomePhone,
                        WorkPhone = customer.WorkPhone,
                        Mobile = customer.Mobile,
                        Fax = customer.Fax,
                        Email = customer.Email,
                        Verified = customer.Verified,
                        IsActive = customer.IsActive,
                        Ip = customer.Ip,
                        LastUpdatedIp = customer.LastUpdatedIp,
                        CreatedDate = customer.CreatedDate,
                        LastModifiedDate = customer.LastModifiedDate
                    };

                    if (customer.AddressId != null)
                    {
                        var addressInfo = ContactService.GetAddressDetails(Convert.ToInt32(customer.AddressId));
                        if (addressInfo != null)
                        {
                            customerInfo.Address = new AddressInfoDetail
                            {
                                AddressTypeId = addressInfo.AddressTypeId,
                                Street = addressInfo.Street,
                                PostalCode = addressInfo.PostalCode,
                                Description = addressInfo.Description,
                                CountryId = addressInfo.CountryId,
                                ProvinceId = addressInfo.ProvinceId,
                                RegionId = addressInfo.RegionId
                            };

                            if (addressInfo.CountryId != null)
                            {
                                customerInfo.Address.Country = ContactService.GetCountryDetails(Convert.ToInt32(addressInfo.CountryId));
                            }

                            if (addressInfo.ProvinceId != null)
                            {
                                customerInfo.Address.Province =
                                    ContactService.GetProvinceDetails(Convert.ToInt32(addressInfo.ProvinceId));
                            }

                            if (addressInfo.RegionId != null)
                            {
                                customerInfo.Address.Region = ContactService.GetRegionDetails(Convert.ToInt32(addressInfo.RegionId));
                            }
                        }
                    }
                  

                    //Save customer info to session
                    var sessionManager = new SessionManager();
                    sessionManager.SetSession(SessionKey.CustomerInfo, customerInfo);

                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.LoginSuccess,
                            ReturnedUrl = "/Cust/MyProfile"
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

        public ActionResult SignOut(string desiredUrl)
        {
            System.Web.HttpContext.Current.Session.Clear();
            Session.Clear();
            Session.Abandon();

            //Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetNoStore();

            FormsAuthentication.SignOut();
            return View("../Cust/Index");
        }

        [HttpPut]
        public ActionResult ChangePassword(CustomerChangePassword entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CustomerService.ChangePassword(entry);
                    return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateSuccess }, JsonRequestBehavior.AllowGet);
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

        // GET: /Admin/Customer/Details/5        
        [HttpGet]
        public ActionResult EditProfile(int id)
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

            return PartialView("../Cust/_EditCustProfile", model);
        }

        //
        // PUT - /Admin/Customer/EditProfile
        [HttpPut]
        public ActionResult EditProfile(CustomerEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var applicationId = GlobalSettings.DefaultApplicationId;
                    int vendorId = GlobalSettings.DefaultVendorId;
                    var userId = Guid.Parse(GlobalSettings.DefaultUserId);

                    CustomerService.EditProfile(applicationId, userId, vendorId, entry);
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
        
        #region Dispose

        private bool _disposed;

        [NonAction]
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    ApplicationService = null;
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