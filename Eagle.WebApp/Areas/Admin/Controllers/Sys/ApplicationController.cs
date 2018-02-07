using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Eagle.Core.Configuration;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.Dtos.SystemManagement.Settings;
using Eagle.Services.Messaging;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;
using Newtonsoft.Json;

namespace Eagle.WebApp.Areas.Admin.Controllers.Sys
{
    public class ApplicationController : BaseController
    {
        private IApplicationService ApplicationService { get; set; }
        private IContactService ContactService { get; set; }
        private ICurrencyService CurrencyService { get; set; }
        private ICacheService CacheService { get; set; }
        private ILanguageService LanguageService { get; set; }
        private IMailService MailService { get; set; }
        private INotificationService NotificationService { get; set; }
        private ITransactionService TransactionService { get; set; }

        public ApplicationController(IApplicationService applicationService, IContactService contactService, ICurrencyService currencyService, ICacheService cacheService,
            ILanguageService languageService, IMailService mailService, INotificationService notificationService, ITransactionService transactionService)
            : base(new IBaseService[] { applicationService, currencyService, cacheService, languageService, mailService, notificationService })
        {
            ApplicationService = applicationService;
            ContactService = contactService;
            CurrencyService = currencyService;
            CacheService = cacheService;
            LanguageService = languageService;
            MailService = mailService;
            NotificationService = notificationService;
            TransactionService = transactionService;
        }

        #region APPLICATION

        //
        // GET: /Admin/Application/
        public ActionResult Index()
        {
            return View("../Sys/Application/Index");
        }

        [HttpGet]
        public string GetDefaultLanguage(Guid applicationId)
        {
            return ApplicationService.GetDefaultLanguage(applicationId);
        }

        [HttpGet]
        public ApplicationDetail GetDetail(Guid id)
        {
            return ApplicationService.GetApplicationDetail(id);
        }

        [HttpPost]
        public void Create(ApplicationEntry entry, out string message)
        {
            message = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    bool isDuplicate = ApplicationService.HasApplicationExisted(entry.ApplicationName);
                    if (isDuplicate == false)
                    {
                        ApplicationService.InsertApplication(entry);
                        message = LanguageResource.CreateSuccess;
                    }
                }
                catch (Exception ex)
                {
                    message = ex.ToString();

                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                message = errors.Aggregate(message, (current, modelError) => current + (modelError.ErrorMessage + "<br/>"));

            }
        }

        [HttpPut]
        public void Edit(Guid id, ApplicationEntry entry, out string message)
        {
            try
            {
                var entity = ApplicationService.GetApplicationDetail(id);
                if (entity != null)
                {
                    entity.ExpiryDate = entry.ExpiryDate;
                    entity.Currency = entry.Currency;
                    entity.DefaultLanguage = entry.DefaultLanguage;
                    entity.TimeZoneOffset = entry.TimeZoneOffset;
                    entity.HomeDirectory = entry.HomeDirectory;
                    entity.Url = entry.Url;
                    entity.LogoFile = entry.LogoFile;
                    entity.BackgroundFile = entry.BackgroundFile;
                    entity.KeyWords = entry.KeyWords;
                    entity.CopyRight = entry.CopyRight;
                    entity.Description = entry.Description;
                    entity.FooterText = entry.FooterText;
                }
                message = LanguageResource.UpdateSuccess;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }

        #endregion

        #region ADVANCED SETTINGS

        [HttpGet]
        public ActionResult GetAdvancedSettings()
        {
            var lst = ApplicationService.GetApplicationSettings(ApplicationId);
            return PartialView("../Sys/Application/Setting/_AdvancedSettings", lst);
        }

        [HttpGet]
        public ApplicationSettingDetail GetApplicationSettingDetail(int applicationSettingId)
        {
            return ApplicationService.GetApplicationSettingDetail(applicationSettingId);
        }

        [HttpGet]
        public string GetDefaultApplicationSetting(string settingName)
        {
            return ApplicationService.GetDefaultApplicationSetting(ApplicationId, settingName);
        }

        // GET: /Admin/Application/CreateSetting      
        [HttpGet]
        public ActionResult CreateSetting()
        {
            return PartialView("../Sys/Application/_CreateSetting");
        }

        // GET: /Admin/Application/EditSetting/5        
        [HttpGet]
        public ActionResult EditSetting(int id)
        {
            var entity = ApplicationService.GetApplicationSettingDetail(id);
            var model = new ApplicationSettingEditEntry
            {
                SettingId = entity.SettingId,
                SettingName = entity.SettingName,
                SettingValue = entity.SettingValue,
                IsSecured = entity.IsSecured,
                IsActive = entity.IsActive
            };

            return PartialView("../Sys/Application/_EditSetting", model);
        }

        // POST: /Admin/Application/CreateSetting
        [HttpPost]
        public ActionResult CreateSetting(ApplicationSettingEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationService.InsertApplicationSetting(ApplicationId, entry);
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
        // PUT - /Admin/Application/EditSetting
        [HttpPut]
        public ActionResult EditSetting(ApplicationSettingEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationService.UpdateApplicationSetting(entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.SettingId
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
        // POST: /Admin/Application/UpdateSettingStatus/5
        [HttpPut]
        public ActionResult UpdateSettingStatus(int id, bool status)
        {
            try
            {
                ApplicationService.UpdateApplicationSettingStatus(id, status ? ApplicationSettingStatus.Active : ApplicationSettingStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // POST: /Admin/Application/UpdateSettingSecure/5
        [HttpPut]
        public ActionResult UpdateSettingSecure(int id, bool isSecured)
        {
            try
            {
                ApplicationService.UpdateApplicationSettingSecure(id, isSecured);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/Application/DeleteSetting/5
        [HttpDelete]
        public ActionResult DeleteSetting(int id)
        {
            try
            {
                ApplicationService.DeleteApplicationSetting(id);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.DeleteSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion

        #region CACHE SETTING
        [HttpGet]
        public ActionResult GetCacheSetting()
        {
            return PartialView("../Sys/Application/Setting/_Cache");
        }

        [HttpPost]
        public ActionResult ClearCache()
        {
            try
            {
                System.Web.HttpContext.Current.Session.Clear();
                Session.Clear();
                Session.Abandon();

                Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
                Request.Cookies.Remove(SettingKeys.UserId);
                Request.Cookies.Remove(SettingKeys.AccountInfo);
                Request.Cookies.Remove(SettingKeys.UserInfo);

                //Remove Cache
                CacheService.Remove(CacheKeySetting.MenuByRole);
                CacheService.Remove(CacheKeySetting.MenuDesktop);

                FormsAuthentication.SignOut();
                //FederatedAuthentication.SessionAuthenticationModule.SignOut();
                //FederatedAuthentication.SessionAuthenticationModule.DeleteSessionTokenCookie();
                //FederatedAuthentication.WSFederationAuthenticationModule.SignOut(false);

                return Json(new SuccessResult
                {
                    Status = ResultStatusType.Success,
                    Data = new
                    {
                        Message = LanguageResource.ClearCacheSuccessfully
                    }
                }, JsonRequestBehavior.AllowGet);

            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region DATE-TIME-CONFIG

        [HttpGet]
        public ActionResult GetDateTimeConfigSettings()
        {
            var applicationSettings = ApplicationService.GetDateTimeConfigSettings(ApplicationId).ToList();
            if (!applicationSettings.Any())
            {
                var violations = new List<RuleViolation>
                {
                    new RuleViolation(ErrorCode.NotFoundApplicationSetting, "ApplicationSetting",
                        ErrorMessage.Messages[ErrorCode.NotFoundApplicationSetting])
                };
                throw new ValidationError(violations);
            }

            var item = applicationSettings.FirstOrDefault();

            var entry = new SettingItemEditEntry
            {
                ApplicationSetting = new ApplicationSettingCustomEntry
                {
                    SettingId = item.SettingId,
                    SettingName = item.SettingName,
                    IsSecured = item.IsSecured,
                    IsActive = item.IsActive
                }
            };

            var lst = new List<BaseSettingDetail>();
            foreach (var applicationSetting in applicationSettings)
            {
                var baseSetting = new BaseSettingDetail
                {
                    KeyName = applicationSetting.Setting.KeyName,
                    KeyValue = applicationSetting.Setting.KeyValue
                };
                lst.Add(baseSetting);
            };

            entry.Settings = lst;

            return PartialView("../Sys/Application/Setting/_DateTimeConfig", entry);
        }

        [HttpPut]
        public ActionResult EditDateTimeConfigSetting(SettingItemEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationService.EditApplicationSetting(entry);

                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ApplicationSetting.SettingId
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

        #endregion

        #region PAGE-CONFIG

        [HttpGet]
        public ActionResult GetPageConfigSettings()
        {
            var applicationSettings = ApplicationService.GetPageConfigSettings(ApplicationId).ToList();
            if (!applicationSettings.Any())
            {
                var violations = new List<RuleViolation>
                {
                    new RuleViolation(ErrorCode.NotFoundApplicationSetting, "ApplicationSetting",
                        ErrorMessage.Messages[ErrorCode.NotFoundApplicationSetting])
                };
                throw new ValidationError(violations);
            }

            var item = applicationSettings.FirstOrDefault();

            var entry = new SettingItemEditEntry
            {
                ApplicationSetting = new ApplicationSettingCustomEntry
                {
                    SettingId = item.SettingId,
                    SettingName = item.SettingName,
                    IsSecured = item.IsSecured,
                    IsActive = item.IsActive
                }
            };

            var lst = new List<BaseSettingDetail>();
            foreach (var applicationSetting in applicationSettings)
            {
                var baseSetting = new BaseSettingDetail
                {
                    KeyName = applicationSetting.Setting.KeyName,
                    KeyValue = applicationSetting.Setting.KeyValue
                };
                lst.Add(baseSetting);
            };

            entry.Settings = lst;

            return PartialView("../Sys/Application/Setting/_PageConfig", entry);
        }

        [HttpPut]
        public ActionResult EditPageConfigSetting(SettingItemEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationService.EditApplicationSetting(entry);

                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ApplicationSetting.SettingId
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

        #endregion

        #region FILE-CONFIG

        [HttpGet]
        public ActionResult GetFileConfigSettings()
        {
            var applicationSettings = ApplicationService.GetFileConfigSettings(ApplicationId).ToList();
            if (!applicationSettings.Any())
            {
                var violations = new List<RuleViolation>
                {
                    new RuleViolation(ErrorCode.NotFoundApplicationSetting, "ApplicationSetting",
                        ErrorMessage.Messages[ErrorCode.NotFoundApplicationSetting])
                };
                throw new ValidationError(violations);
            }

            var item = applicationSettings.FirstOrDefault();

            var entry = new SettingItemEditEntry
            {
                ApplicationSetting = new ApplicationSettingCustomEntry
                {
                    SettingId = item.SettingId,
                    SettingName = item.SettingName,
                    IsSecured = item.IsSecured,
                    IsActive = item.IsActive
                }
            };

            var lst = new List<BaseSettingDetail>();
            foreach (var applicationSetting in applicationSettings)
            {
                var baseSetting = new BaseSettingDetail
                {
                    KeyName = applicationSetting.Setting.KeyName,
                    KeyValue = applicationSetting.Setting.KeyValue
                };
                lst.Add(baseSetting);
            };

            entry.Settings = lst;

            return PartialView("../Sys/Application/Setting/_FileConfig", entry);
        }

        [HttpPut]
        public ActionResult EditFileConfigSetting(SettingItemEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationService.EditApplicationSetting(entry);

                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ApplicationSetting.SettingId
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

        #endregion

        #region ORDER

        [HttpGet]
        public ActionResult GetOrderSettings()
        {
            var applicationSettings = ApplicationService.GetOrderSettings(ApplicationId).ToList();
            if (!applicationSettings.Any())
            {
                var violations = new List<RuleViolation>
                {
                    new RuleViolation(ErrorCode.NotFoundApplicationSetting, "ApplicationSetting",
                        ErrorMessage.Messages[ErrorCode.NotFoundApplicationSetting])
                };
                throw new ValidationError(violations);
            }

            var item = applicationSettings.FirstOrDefault();

            var entry = new SettingItemEditEntry
            {
                ApplicationSetting = new ApplicationSettingCustomEntry
                {
                    SettingId = item.SettingId,
                    SettingName = item.SettingName,
                    IsSecured = item.IsSecured,
                    IsActive = item.IsActive
                }
            };

            var lst = new List<BaseSettingDetail>();
            foreach (var applicationSetting in applicationSettings)
            {
                var baseSetting = new BaseSettingDetail
                {
                    KeyName = applicationSetting.Setting.KeyName,
                    KeyValue = applicationSetting.Setting.KeyValue
                };
                lst.Add(baseSetting);
            }

            entry.Settings = lst;

            return PartialView("../Sys/Application/Setting/_Order", entry);
        }

        [HttpPut]
        public ActionResult EditOrderSetting(SettingItemEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationService.EditApplicationSetting(entry);

                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ApplicationSetting.SettingId
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

        #endregion

        #region PAYGATE -SETTING

        [HttpGet]
        public ActionResult GetPayGateSettings()
        {
            var lst = TransactionService.GetPaymentMethods();
            return PartialView("../Sys/Application/Setting/_PayGate", lst);
        }

        [HttpPut]
        public ActionResult SelectPayGate(SettingItemEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationService.EditApplicationSetting(entry);

                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ApplicationSetting.SettingId
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
       
        #endregion

        #region PAYPAL -SETTING

        [HttpGet]
        public ActionResult GetPayPal()
        {
            var paypalSettings = ApplicationService.GetPaypalSettings(ApplicationId);
            return PartialView("../Sys/Application/Setting/_PayPal", paypalSettings);
        }

        [HttpGet]
        public ActionResult EditPaypalSetting(string mode = "sandbox")
        {
            var setting = ApplicationService.GetPaypalSetting(ApplicationId, mode);

            var editEntry = new PaypalSettingEditEntry
            {
                ApplicationSetting = new ApplicationSettingCustomEntry
                {
                    SettingId = setting.SettingId,
                    SettingName = setting.SettingName,
                    IsSecured = setting.IsSecured,
                    IsActive = setting.IsActive
                },
                PaypalSetting = new PaypalSettingEntry
                {
                    Mode = setting.Mode,
                    ClientId = setting.ClientId,
                    ClientSecret = setting.ClientSecret,
                    ConnectionTimeout = setting.ConnectionTimeout,
                    RequestRetries = setting.RequestRetries
                }
            };

            return PartialView("../Sys/Application/Setting/_PayPalDetail", editEntry);
        }

        [HttpPut]
        public ActionResult EditPaypalSetting(PaypalSettingEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var setting = new PaypalSettingEntry
                    {
                        Mode = entry.PaypalSetting.Mode,
                        ClientId = entry.PaypalSetting.ClientId,
                        ClientSecret = entry.PaypalSetting.ClientSecret,
                        ConnectionTimeout = entry.PaypalSetting.ConnectionTimeout,
                        RequestRetries = entry.PaypalSetting.RequestRetries
                    };

                    var applicationSettingEditEntry = new ApplicationSettingEditEntry
                    {
                        SettingId = entry.ApplicationSetting.SettingId,
                        SettingName = entry.ApplicationSetting.SettingName,
                        SettingValue = JsonConvert.SerializeObject(setting),
                        IsSecured = entry.ApplicationSetting.IsSecured,
                        IsActive = entry.ApplicationSetting.IsActive
                    };

                    ApplicationService.UpdateApplicationSetting(applicationSettingEditEntry);

                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ApplicationSetting.SettingId
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

        [HttpPut]
        public ActionResult UpdatePaypalMode(string mode)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationService.UpdatePaypalMode(ApplicationId, mode);

                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess
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

        #endregion

        #region TRANSACTION

        [HttpGet]
        public ActionResult GetTransactionSettings()
        {
            var applicationSettings = ApplicationService.GetTwitterSettings(ApplicationId).ToList();
            if (!applicationSettings.Any())
            {
                var violations = new List<RuleViolation>
                {
                    new RuleViolation(ErrorCode.NotFoundApplicationSetting, "ApplicationSetting",
                        ErrorMessage.Messages[ErrorCode.NotFoundApplicationSetting])
                };
                throw new ValidationError(violations);
            }

            var item = applicationSettings.FirstOrDefault();

            var entry = new SettingItemEditEntry
            {
                ApplicationSetting = new ApplicationSettingCustomEntry
                {
                    SettingId = item.SettingId,
                    SettingName = item.SettingName,
                    IsSecured = item.IsSecured,
                    IsActive = item.IsActive
                }
            };

            var lst = new List<BaseSettingDetail>();
            foreach (var applicationSetting in applicationSettings)
            {
                var baseSetting = new BaseSettingDetail
                {
                    KeyName = applicationSetting.Setting.KeyName,
                    KeyValue = applicationSetting.Setting.KeyValue
                };
                lst.Add(baseSetting);
            };

            entry.Settings = lst;

            return PartialView("../Sys/Application/Setting/_Transaction", entry);
        }

        [HttpPut]
        public ActionResult EditTransactionSetting(SettingItemEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationService.EditApplicationSetting(entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ApplicationSetting.SettingId
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
        
        #endregion
        
        #region RATING -SETTING

        public ActionResult GetRateSettings()
        {
            var settings = ApplicationService.GetRateSettings(ApplicationId).ToList();
            if (!settings.Any())
            {
                var violations = new List<RuleViolation>
                {
                    new RuleViolation(ErrorCode.NotFoundApplicationSetting, "ApplicationSetting",
                        ErrorMessage.Messages[ErrorCode.NotFoundApplicationSetting])
                };
                throw new ValidationError(violations);
            }

            var item = settings.FirstOrDefault();

            var entry = new SettingItemEditEntry
            {
                ApplicationSetting = new ApplicationSettingCustomEntry
                {
                    SettingId = item.SettingId,
                    SettingName = item.SettingName,
                    IsSecured = item.IsSecured,
                    IsActive = item.IsActive
                }
            };

            var lst = new List<BaseSettingDetail>();
            foreach (var rateSetting in settings)
            {
                var baseSetting = new BaseSettingDetail
                {
                    KeyName = rateSetting.Setting.KeyName,
                    KeyValue = rateSetting.Setting.KeyValue
                };
                lst.Add(baseSetting);
            };

            entry.Settings = lst;

            return PartialView("../Sys/Application/Setting/_Rating", entry);
        }

        [HttpPut]
        public ActionResult EditRateSetting(SettingItemEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationService.EditApplicationSetting(entry);

                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ApplicationSetting.SettingId
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

        #endregion

        #region SMTP -SETTING

        [HttpGet]
        public ActionResult GetSmtpSettings()
        {
            var applicationSettings = ApplicationService.GetSmtpSettings(ApplicationId).ToList();
            if (!applicationSettings.Any())
            {
                var violations = new List<RuleViolation>
                {
                    new RuleViolation(ErrorCode.NotFoundApplicationSetting, "ApplicationSetting",
                        ErrorMessage.Messages[ErrorCode.NotFoundApplicationSetting])
                };
                throw new ValidationError(violations);
            }

            var applicationSetting = applicationSettings.FirstOrDefault();

            var entry = new SettingItemEditEntry
            {
                ApplicationSetting = new ApplicationSettingCustomEntry
                {
                    SettingId = applicationSetting.SettingId,
                    SettingName = applicationSetting.SettingName,
                    IsSecured = applicationSetting.IsSecured,
                    IsActive = applicationSetting.IsActive
                }
            };

            var lst = new List<BaseSettingDetail>();
            foreach (var smtpSetting in applicationSettings)
            {
                var baseSetting = new BaseSettingDetail
                {
                    KeyName = smtpSetting.Setting.KeyName,
                    KeyValue = smtpSetting.Setting.KeyValue
                };
                lst.Add(baseSetting);
            };

            entry.Settings = lst;

            var item = lst.FirstOrDefault(x => x.KeyName == "MailServerProviderId");
            var mailServerProviderId = Convert.ToInt32(item.KeyValue);

            var notificationSenderSetting = lst.FirstOrDefault(x => x.KeyName == "NotificationSenderId");
            var notificationSenderId = Convert.ToInt32(notificationSenderSetting.KeyValue);

            ViewBag.MailServerProviderId = MailService.PopulateMailServerProviderSelectList(mailServerProviderId);
            ViewBag.NotificationSenderId = NotificationService.PopulateNotificationSenderSelectList(mailServerProviderId, notificationSenderId);

            return PartialView("../Sys/Application/Setting/_Smtp", entry);
        }

        [HttpGet]
        public ActionResult PopulateNotificationSenderSelectList(int? mailServerProviderId, int? selectedValue = null, bool? isShowSelectText = true)
        {
            var senders = NotificationService.PopulateNotificationSenderSelectList(mailServerProviderId, selectedValue, isShowSelectText);
            return Json(senders, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public ActionResult EditSmtpSetting(SettingItemEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationService.EditApplicationSetting(entry);

                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ApplicationSetting.SettingId
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
        #endregion

        #region GOOGLE RECAPTCHA

        [HttpGet]
        public ActionResult GetRecaptchaSettings()
        {
            var applicationSettings = ApplicationService.GetRecaptchaSettings(ApplicationId).ToList();
            if (!applicationSettings.Any())
            {
                var violations = new List<RuleViolation>
                {
                    new RuleViolation(ErrorCode.NotFoundApplicationSetting, "ApplicationSetting",
                        ErrorMessage.Messages[ErrorCode.NotFoundApplicationSetting])
                };
                throw new ValidationError(violations);
            }

            var item = applicationSettings.FirstOrDefault();

            var entry = new SettingItemEditEntry
            {
                ApplicationSetting = new ApplicationSettingCustomEntry
                {
                    SettingId = item.SettingId,
                    SettingName = item.SettingName,
                    IsSecured = item.IsSecured,
                    IsActive = item.IsActive
                }
            };

            var lst = new List<BaseSettingDetail>();
            foreach (var applicationSetting in applicationSettings)
            {
                var baseSetting = new BaseSettingDetail
                {
                    KeyName = applicationSetting.Setting.KeyName,
                    KeyValue = applicationSetting.Setting.KeyValue
                };
                lst.Add(baseSetting);
            }

            entry.Settings = lst;

            return PartialView("../Sys/Application/Setting/_ReCaptcha", entry);
        }

        [HttpPut]
        public ActionResult EditRecaptchaSetting(SettingItemEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationService.EditApplicationSetting(entry);

                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ApplicationSetting.SettingId
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

        #endregion

        #region TWITTER

        [HttpGet]
        public ActionResult GetTwitterSettings()
        {
            var applicationSettings = ApplicationService.GetTwitterSettings(ApplicationId).ToList();
            if (!applicationSettings.Any())
            {
                var violations = new List<RuleViolation>
                {
                    new RuleViolation(ErrorCode.NotFoundApplicationSetting, "ApplicationSetting",
                        ErrorMessage.Messages[ErrorCode.NotFoundApplicationSetting])
                };
                throw new ValidationError(violations);
            }

            var item = applicationSettings.FirstOrDefault();

            var entry = new SettingItemEditEntry
            {
                ApplicationSetting = new ApplicationSettingCustomEntry
                {
                    SettingId = item.SettingId,
                    SettingName = item.SettingName,
                    IsSecured = item.IsSecured,
                    IsActive = item.IsActive
                }
            };

            var lst = new List<BaseSettingDetail>();
            foreach (var applicationSetting in applicationSettings)
            {
                var baseSetting = new BaseSettingDetail
                {
                    KeyName = applicationSetting.Setting.KeyName,
                    KeyValue = applicationSetting.Setting.KeyValue
                };
                lst.Add(baseSetting);
            };

            entry.Settings = lst;

            return PartialView("../Sys/Application/Setting/_Twitter", entry);
        }

        [HttpPut]
        public ActionResult EditTwitterSetting(SettingItemEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationService.EditApplicationSetting(entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ApplicationSetting.SettingId
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

        #endregion

        #region FACEBOOK

        [HttpGet]
        public ActionResult GetFacebookSettings()
        {
            var applicationSettings = ApplicationService.GetFacebookSettings(ApplicationId).ToList();
            if (!applicationSettings.Any())
            {
                var violations = new List<RuleViolation>
                {
                    new RuleViolation(ErrorCode.NotFoundApplicationSetting, "ApplicationSetting",
                        ErrorMessage.Messages[ErrorCode.NotFoundApplicationSetting])
                };
                throw new ValidationError(violations);
            }

            var item = applicationSettings.FirstOrDefault();

            var entry = new SettingItemEditEntry
            {
                ApplicationSetting = new ApplicationSettingCustomEntry
                {
                    SettingId = item.SettingId,
                    SettingName = item.SettingName,
                    IsSecured = item.IsSecured,
                    IsActive = item.IsActive
                }
            };

            var lst = new List<BaseSettingDetail>();
            foreach (var applicationSetting in applicationSettings)
            {
                var baseSetting = new BaseSettingDetail
                {
                    KeyName = applicationSetting.Setting.KeyName,
                    KeyValue = applicationSetting.Setting.KeyValue
                };
                lst.Add(baseSetting);
            };

            entry.Settings = lst;

            return PartialView("../Sys/Application/Setting/_Facebook", entry);
        }

        [HttpPut]
        public ActionResult EditFacebookSetting(SettingItemEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationService.EditApplicationSetting(entry);

                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ApplicationSetting.SettingId
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

        #endregion

        #region Currency

        [HttpGet]
        public ActionResult GetCurrencySettings()
        {
            var applicationSettings = ApplicationService.GetCurrencySettings(ApplicationId).ToList();
            if (!applicationSettings.Any())
            {
                var violations = new List<RuleViolation>
                {
                    new RuleViolation(ErrorCode.NotFoundApplicationSetting, "ApplicationSetting",
                        ErrorMessage.Messages[ErrorCode.NotFoundApplicationSetting])
                };
                throw new ValidationError(violations);
            }

            var item = applicationSettings.FirstOrDefault();

            var entry = new SettingItemEditEntry
            {
                ApplicationSetting = new ApplicationSettingCustomEntry
                {
                    SettingId = item.SettingId,
                    SettingName = item.SettingName,
                    IsSecured = item.IsSecured,
                    IsActive = item.IsActive
                }
            };

            var lst = new List<BaseSettingDetail>();
            string currencyCode = string.Empty;
            foreach (var applicationSetting in applicationSettings)
            {
                var baseSetting = new BaseSettingDetail
                {
                    KeyName = applicationSetting.Setting.KeyName,
                    KeyValue = applicationSetting.Setting.KeyValue
                };

                if (applicationSetting.Setting.KeyName == "CurrencyCode")
                {
                    currencyCode = applicationSetting.Setting.KeyValue;
                }

                lst.Add(baseSetting);
            };

            entry.Settings = lst;
            ViewBag.Currencies = CurrencyService.PopulateCurrencySelectList(CurrencyStatus.Active, currencyCode, true);
            return PartialView("../Sys/Application/Setting/_Currency", entry);
        }

        [HttpPut]
        public ActionResult EditCurrencySetting(SettingItemEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationService.EditApplicationSetting(entry);

                    var item = entry.Settings.FirstOrDefault(x => x.KeyName == "LanguageCode");
                    if (item != null)
                    {
                        CurrencyService.SetSelectedCurrency(item.KeyValue);
                    }

                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ApplicationSetting.SettingId
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

        #endregion

        #region PRODUCT FILTERED PRICE RANGE

        [HttpGet]
        public ActionResult GetProductFilteredPriceRangeSettings()
        {
            var applicationSettings = ApplicationService.GetProductFilteredPriceRangeSettings(ApplicationId).ToList();
            if (!applicationSettings.Any())
            {
                var violations = new List<RuleViolation>
                {
                    new RuleViolation(ErrorCode.NotFoundApplicationSetting, "ApplicationSetting",
                        ErrorMessage.Messages[ErrorCode.NotFoundApplicationSetting])
                };
                throw new ValidationError(violations);
            }

            var item = applicationSettings.FirstOrDefault();

            var entry = new SettingItemEditEntry
            {
                ApplicationSetting = new ApplicationSettingCustomEntry
                {
                    SettingId = item.SettingId,
                    SettingName = item.SettingName,
                    IsSecured = item.IsSecured,
                    IsActive = item.IsActive
                }
            };

            var lst = new List<BaseSettingDetail>();
            foreach (var applicationSetting in applicationSettings)
            {
                var baseSetting = new BaseSettingDetail
                {
                    KeyName = applicationSetting.Setting.KeyName,
                    KeyValue = applicationSetting.Setting.KeyValue
                };
                lst.Add(baseSetting);
            };

            entry.Settings = lst;

            return PartialView("../Sys/Application/Setting/_ProductPriceRange", entry);
        }

        [HttpPut]
        public ActionResult EditProductFilteredPriceRangeSetting(SettingItemEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationService.EditApplicationSetting(entry);

                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ApplicationSetting.SettingId
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

        #endregion

        #region PRODUCT PRICE RANGE

        [HttpGet]
        public ActionResult GetProductPriceRangeSettings()
        {
            var applicationSettings = ApplicationService.GetProductPriceRangeSettings(ApplicationId).ToList();
            if (!applicationSettings.Any())
            {
                var violations = new List<RuleViolation>
                {
                    new RuleViolation(ErrorCode.NotFoundApplicationSetting, "ApplicationSetting",
                        ErrorMessage.Messages[ErrorCode.NotFoundApplicationSetting])
                };
                throw new ValidationError(violations);
            }

            var item = applicationSettings.FirstOrDefault();

            var entry = new SettingItemEditEntry
            {
                ApplicationSetting = new ApplicationSettingCustomEntry
                {
                    SettingId = item.SettingId,
                    SettingName = item.SettingName,
                    IsSecured = item.IsSecured,
                    IsActive = item.IsActive
                }
            };

            var lst = new List<BaseSettingDetail>();
            foreach (var applicationSetting in applicationSettings)
            {
                var baseSetting = new BaseSettingDetail
                {
                    KeyName = applicationSetting.Setting.KeyName,
                    KeyValue = applicationSetting.Setting.KeyValue
                };
                lst.Add(baseSetting);
            };

            entry.Settings = lst;

            return PartialView("../Sys/Application/Setting/_ProductPriceRange", entry);
        }

        [HttpPut]
        public ActionResult EditProductPriceRangeSetting(SettingItemEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationService.EditApplicationSetting(entry);

                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ApplicationSetting.SettingId
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

        #endregion

        #region Language

        [HttpGet]
        public ActionResult GetLanguageSettings()
        {
            var applicationSettings = ApplicationService.GetLanguageSettings(ApplicationId).ToList();
            if (!applicationSettings.Any())
            {
                var violations = new List<RuleViolation>
                {
                    new RuleViolation(ErrorCode.NotFoundApplicationSetting, "ApplicationSetting",
                        ErrorMessage.Messages[ErrorCode.NotFoundApplicationSetting])
                };
                throw new ValidationError(violations);
            }

            var item = applicationSettings.FirstOrDefault();

            var entry = new SettingItemEditEntry
            {
                ApplicationSetting = new ApplicationSettingCustomEntry
                {
                    SettingId = item.SettingId,
                    SettingName = item.SettingName,
                    IsSecured = item.IsSecured,
                    IsActive = item.IsActive
                }
            };

            var lst = new List<BaseSettingDetail>();
            string languageCode = string.Empty;
            foreach (var applicationSetting in applicationSettings)
            {
                var baseSetting = new BaseSettingDetail
                {
                    KeyName = applicationSetting.Setting.KeyName,
                    KeyValue = applicationSetting.Setting.KeyValue
                };

                if (applicationSetting.Setting.KeyName == "LanguageCode")
                {
                    languageCode = applicationSetting.Setting.KeyValue;
                }

                lst.Add(baseSetting);
            };

            entry.Settings = lst;
            ViewBag.Languages = LanguageService.PopulateApplicationLanguages(ApplicationId, ApplicationLanguageStatus.Active, languageCode, true);
            return PartialView("../Sys/Application/Setting/_Language", entry);
        }

        [HttpPut]
        public ActionResult EditLanguageSetting(SettingItemEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationService.EditApplicationSetting(entry);

                    var item = entry.Settings.FirstOrDefault(x => x.KeyName == "LanguageCode");
                    if (item != null)
                    {
                        LanguageService.UpdateSelectedApplicationLanguage(ApplicationId, item.KeyValue);
                        ApplicationService.UpdateDefaultLanguage(ApplicationId, item.KeyValue);
                    }

                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ApplicationSetting.SettingId
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

        #endregion

        #region ADDRESS

        [HttpGet]
        public ActionResult PopulateProvinceSelectList(int countryId, int? selectedValue = null, bool? isShowSelectText = true)
        {
            var senders = ContactService.PopulateProvinceSelectList(countryId, true, selectedValue, isShowSelectText);
            return Json(senders, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetAddressSettings()
        {
            var applicationSettings = ApplicationService.GetAddressSettings(ApplicationId).ToList();
            if (!applicationSettings.Any())
            {
                var violations = new List<RuleViolation>
                {
                    new RuleViolation(ErrorCode.NotFoundApplicationSetting, "ApplicationSetting",
                        ErrorMessage.Messages[ErrorCode.NotFoundApplicationSetting])
                };
                throw new ValidationError(violations);
            }

            var item = applicationSettings.FirstOrDefault();

            var entry = new SettingItemEditEntry
            {
                ApplicationSetting = new ApplicationSettingCustomEntry
                {
                    SettingId = item.SettingId,
                    SettingName = item.SettingName,
                    IsSecured = item.IsSecured,
                    IsActive = item.IsActive
                }
            };

            int countryId = GlobalSettings.DefaultCountryId;
            int provinceId = GlobalSettings.DefaultProvinceId;

            var lst = new List<BaseSettingDetail>();
            foreach (var applicationSetting in applicationSettings)
            {
                var baseSetting = new BaseSettingDetail
                {
                    KeyName = applicationSetting.Setting.KeyName,
                    KeyValue = applicationSetting.Setting.KeyValue
                };

                if (applicationSetting.Setting.KeyName == "CountryId")
                {
                    countryId = Convert.ToInt32(applicationSetting.Setting.KeyValue);
                }

                if (applicationSetting.Setting.KeyName == "ProvinceId")
                {
                    provinceId = Convert.ToInt32(applicationSetting.Setting.KeyValue);
                }

                lst.Add(baseSetting);
            };

            entry.Settings = lst;

            ViewBag.Country = ContactService.PopulateCountrySelectList(true, countryId);
            ViewBag.Province = ContactService.PopulateProvinceSelectList(countryId, true, provinceId);

            return PartialView("../Sys/Application/Setting/_Address", entry);
        }

        [HttpPut]
        public ActionResult EditAddressSetting(SettingItemEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationService.EditApplicationSetting(entry);

                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ApplicationSetting.SettingId
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

        #endregion

        #region DELIVERY

        [HttpGet]
        public ActionResult GetDeliverySettings()
        {
            var applicationSettings = ApplicationService.GetDeliverySettings(ApplicationId).ToList();
            if (!applicationSettings.Any())
            {
                var violations = new List<RuleViolation>
                {
                    new RuleViolation(ErrorCode.NotFoundApplicationSetting, "ApplicationSetting",
                        ErrorMessage.Messages[ErrorCode.NotFoundApplicationSetting])
                };
                throw new ValidationError(violations);
            }

            var item = applicationSettings.FirstOrDefault();

            var entry = new SettingItemEditEntry
            {
                ApplicationSetting = new ApplicationSettingCustomEntry
                {
                    SettingId = item.SettingId,
                    SettingName = item.SettingName,
                    IsSecured = item.IsSecured,
                    IsActive = item.IsActive
                }
            };

            var lst = new List<BaseSettingDetail>();
            foreach (var applicationSetting in applicationSettings)
            {
                var baseSetting = new BaseSettingDetail
                {
                    KeyName = applicationSetting.Setting.KeyName,
                    KeyValue = applicationSetting.Setting.KeyValue
                };

                lst.Add(baseSetting);
            };

            entry.Settings = lst;
            return PartialView("../Sys/Application/Setting/_Delivery", entry);
        }

        [HttpPut]
        public ActionResult EditDeliverySetting(SettingItemEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationService.EditApplicationSetting(entry);

                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ApplicationSetting.SettingId
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

        [HttpPut]
        public ActionResult UpdateDeliverySettingStatus(int id, bool status)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationService.UpdateDeliverySettingStatus(id, "IsShippingEnable", Convert.ToString(status));

                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess
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
        #endregion

        #region NOTIFICATION

        [HttpGet]
        public ActionResult GetNotificationSettings()
        {
            var applicationSettings = ApplicationService.GetNotificationSettings(ApplicationId).ToList();
            if (!applicationSettings.Any())
            {
                var violations = new List<RuleViolation>
                    {
                        new RuleViolation(ErrorCode.NotFoundApplicationSetting, "ApplicationSetting",
                            ErrorMessage.Messages[ErrorCode.NotFoundApplicationSetting])
                    };
                throw new ValidationError(violations);
            }

            var item = applicationSettings.FirstOrDefault();

            var entry = new SettingItemEditEntry
            {
                ApplicationSetting = new ApplicationSettingCustomEntry
                {
                    SettingId = item.SettingId,
                    SettingName = item.SettingName,
                    IsSecured = item.IsSecured,
                    IsActive = item.IsActive
                }
            };

            var lst = new List<BaseSettingDetail>();
            foreach (var applicationSetting in applicationSettings)
            {
                var baseSetting = new BaseSettingDetail
                {
                    KeyName = applicationSetting.Setting.KeyName,
                    KeyValue = applicationSetting.Setting.KeyValue
                };

                lst.Add(baseSetting);
            }
                ;

            entry.Settings = lst;
            return PartialView("../Sys/Application/Setting/_Notification", entry);

        }

        [HttpPut]
        public ActionResult EditNotificationSetting(SettingItemEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationService.EditApplicationSetting(entry);

                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.ApplicationSetting.SettingId
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

        [HttpPut]
        public ActionResult UpdateNotificationSettingStatus(int id, bool status)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationService.UpdateNotificationSettingStatus(id, "IsNotificationEnable", Convert.ToString(status));

                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess
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
                    ApplicationService = null;
                    ContactService = null;
                    CacheService = null;
                    CurrencyService = null;
                    LanguageService = null;
                    MailService = null;
                    NotificationService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
