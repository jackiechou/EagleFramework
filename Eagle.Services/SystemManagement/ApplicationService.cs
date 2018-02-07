using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Eagle.Core.Configuration;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.Entities.SystemManagement.Settings;
using Eagle.Repositories;
using Eagle.Resources;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.Dtos.SystemManagement.Settings;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.Skins;
using Eagle.Services.SystemManagement.Validation;
using Eagle.Services.Validations;
using Newtonsoft.Json;

namespace Eagle.Services.SystemManagement
{
    public class ApplicationService : BaseService, IApplicationService
    {
        public ICacheService CacheService { get; set; }
        public ILogService LogService { get; set; }
        public ILanguageService LanguageService { get; set; }
        private IThemeService ThemeService { get; set; }
        private IVendorService VendorService { get; set; }

        public ApplicationService(IUnitOfWork unitOfWork, ICacheService cacheService, ILanguageService languageService,
            ILogService logService, IThemeService themeService, IVendorService vendorService) : base(unitOfWork)
        {
            CacheService = cacheService;
            LanguageService = languageService;
            LogService = logService;
            ThemeService = themeService;
            VendorService = vendorService;
        }

        #region Configuration Application Setting

        public void SetUpAppConfig(Guid applicationId)
        {
            
            string themeSrc = string.Empty;
            string themeName = CacheService.Get<string>(CacheKeySetting.ThemeName);
            string currencyCode = GetCurrentCurrencyCode();
            if (!string.IsNullOrEmpty(themeName))
            {
                themeName = CacheService.Get<string>(CacheKeySetting.ThemeName);
                themeSrc = CacheService.Get<string>(CacheKeySetting.ThemeSrc);
            }
            else
            {
                var theme = ThemeService.GetSelectedTheme(applicationId);
                if (theme != null)
                {
                    themeName = theme.PackageName;
                    themeSrc = theme.PackageSrc;
                }

                CacheService.Add(CacheKeySetting.ThemeName, themeName);
                CacheService.Add(CacheKeySetting.ThemeSrc, themeSrc);
            }

            GetCurrentVendorId();

            //Register ThemedViewEngine - Replace the Default WebFormViewEngine with our custom WebFormThemeViewEngine
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new ThemeViewEngine(themeName));
            HttpContext.Current.Items[CacheKeySetting.ThemeName] = themeName;
        }

        public void SetUpCulture(Guid applicationId)
        {
            string languageCode = CacheService.Get<string>(CacheKeySetting.LanguageCode);
            if (string.IsNullOrEmpty(languageCode))
            {
                languageCode = GetCurrentLanguageCode(applicationId);
                CacheService.Add(CacheKeySetting.LanguageCode, languageCode);
            }

            CultureInfo cultureInfo = new CultureInfo(languageCode);
            var dateformat = new DateTimeFormatInfo
            {
                ShortDatePattern = "MM/dd/yyyy",
                LongTimePattern = "MM/dd/yyyy HH:mm:ss"
            };

            var numberFormat = new NumberFormatInfo()
            {
                CurrencyGroupSeparator = ".",
            };

            cultureInfo.DateTimeFormat = dateformat;
            cultureInfo.NumberFormat = numberFormat;

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCode);
        }

        public int GetCurrentVendorId()
        {
            string vendorId = CacheService.Get<string>(CacheKeySetting.VendorId);
            if (!string.IsNullOrEmpty(vendorId) && vendorId != "0") return Convert.ToInt32(vendorId);
            var vendor = CacheService.Get<VendorInfoDetail>(CacheKeySetting.Vendor);
            if (vendor == null)
            {
                vendor = VendorService.GetDefaultVendor();
                CacheService.Set(CacheKeySetting.Vendor, vendor);
                vendorId = vendor.VendorId.ToString();
            }
            CacheService.Add(CacheKeySetting.VendorId, vendorId);
            return vendor.VendorId;
        }

        public string GetCurrentCurrencyCode()
        {
            //Set up Currency Code
            string currencyCode = CacheService.Get<string>(CacheKeySetting.CurrencyCode);
            if (!string.IsNullOrEmpty(currencyCode)) return currencyCode;

            var selectedCurrency = UnitOfWork.CurrencyRepository.GetSelectedCurrency();
            currencyCode = selectedCurrency != null ? selectedCurrency.CurrencyCode : GlobalSettings.DefaultCurrencyCode;
            CacheService.Add(CacheKeySetting.CurrencyCode, currencyCode);

            return currencyCode;
        }

        public string GetCurrentLanguageCode(Guid applicationId)
        {
            string languageCode = CacheService.Get<string>(CacheKeySetting.LanguageCode);
            if (!string.IsNullOrEmpty(languageCode)) return languageCode;

            var selectedLanguage = UnitOfWork.ApplicationLanguageRepository.GetSelectedLanguage(applicationId);
            languageCode = selectedLanguage != null ? selectedLanguage.LanguageCode : GlobalSettings.DefaultLanguageCode;
            CacheService.Add(CacheKeySetting.LanguageCode, languageCode);

            return languageCode;
        }

        #endregion

        #region Application

        public IEnumerable<ApplicationDetail> GetApplications(ref int? recordCount, string orderBy = null,
            int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.ApplicationRepository.GetList(ref recordCount, orderBy, page, pageSize).AsEnumerable();
            return lst.ToDtos<ApplicationEntity, ApplicationDetail>();
        }
        public ApplicationDetail GetApplicationInfo(Guid applicationId)
        {
            var dataViolations = new List<RuleViolation>();
            var application = UnitOfWork.ApplicationRepository.FindById(applicationId);
            if (application == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundForApplication, "ApplicationId", LanguageResource.NotFoundApplication + " : " + string.Join(", ", applicationId)));
                throw new ValidationError(dataViolations);
            }
            return application.ToDto<ApplicationEntity, ApplicationDetail>();
        }
        public ApplicationDetail GetApplicationDetail(Guid id)
        {
            var entity = UnitOfWork.ApplicationRepository.FindById(id);
            return entity.ToDto<ApplicationEntity, ApplicationDetail>();
        }
        public string GetDefaultLanguage(Guid applicationId)
        {
            return UnitOfWork.ApplicationRepository.FindById(applicationId).DefaultLanguage;
        }
        public bool HasApplicationExisted(string applicationName)
        {
            return UnitOfWork.ApplicationRepository.HasDataExisted(applicationName);
        }
        public void InsertApplication(ApplicationEntry entry)
        {
            var entity = entry.ToEntity<ApplicationEntry, ApplicationEntity>();
            UnitOfWork.ApplicationRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateApplication(Guid id, ApplicationEntry entry)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ApplicationRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundApplication, "Application"));
                throw new ValidationError(violations);
            }

            entity.ApplicationName = entry.ApplicationName;
            entity.DefaultLanguage = entry.DefaultLanguage;
            entity.HomeDirectory = entry.HomeDirectory;
            entity.Currency = entry.Currency;
            entity.TimeZoneOffset = entry.TimeZoneOffset;
            entity.Url = entry.Url;
            entity.LogoFile = entry.LogoFile;
            entity.BackgroundFile = entry.BackgroundFile;
            entity.KeyWords = entry.KeyWords;
            entity.CopyRight = entry.CopyRight;
            entity.FooterText = entry.FooterText;
            entity.Description = entry.Description;
            entity.HostSpace = entry.HostSpace;
            entity.HostFee = entry.HostFee;
            entity.ExpiryDate = entry.ExpiryDate;
            entity.RegisteredUserId = entry.RegisteredUserId;

            UnitOfWork.ApplicationRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
   
        #endregion

        #region Application Setting
        public IEnumerable<ApplicationSettingDetail> GetApplicationSettings(Guid applicationId)
        {
            var lst = UnitOfWork.ApplicationSettingRepository.GetApplicationSettings(applicationId).AsEnumerable();
            return lst.ToDtos<ApplicationSetting, ApplicationSettingDetail>();
        }
        public string GetDefaultApplicationSetting(Guid applicationId, string settingName)
        {
            return UnitOfWork.ApplicationSettingRepository.GetDefaultApplicationSetting(applicationId, settingName, null);
        }

        public string GetDefaultApplicationSetting(Guid applicationId, string settingName, string defaultValue)
        {
            return UnitOfWork.ApplicationSettingRepository.GetDefaultApplicationSetting(applicationId, settingName, defaultValue);
        }
        public ApplicationSettingDetail GetApplicationSettingDetail(int id)
        {
            var entity = UnitOfWork.ApplicationSettingRepository.FindById(id);
            return entity.ToDto<ApplicationSetting, ApplicationSettingDetail>();
        }
        public void InsertApplicationSetting(Guid applicationId, ApplicationSettingEntry entry)
        {
            ISpecification<ApplicationSettingEntry> validator = new ApplicationSettingEntryValidator(UnitOfWork, PermissionLevel.Create);
            var violations = new List<RuleViolation>();
            var isValid = validator.IsSatisfyBy(entry, violations);
            if (!isValid) throw new ValidationError(violations);

            var entity = entry.ToEntity<ApplicationSettingEntry, ApplicationSetting>();
            entity.ApplicationId = applicationId;

            UnitOfWork.ApplicationSettingRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void EditApplicationSetting(SettingItemEditEntry entry)
        {
            if (!entry.Settings.Any())
            {
                var violations = new List<RuleViolation>
                {
                    new RuleViolation(ErrorCode.NotFoundApplicationSetting, "ApplicationSetting",null,
                        ErrorMessage.Messages[ErrorCode.NotFoundApplicationSetting])
                };
                throw new ValidationError(violations);
            }

            var settingValue = JsonConvert.SerializeObject(entry.Settings);

            var applicationSettingEditEntry = new ApplicationSettingEditEntry
            {
                SettingId = entry.ApplicationSetting.SettingId,
                SettingName = entry.ApplicationSetting.SettingName,
                SettingValue = settingValue,
                IsSecured = entry.ApplicationSetting.IsSecured,
                IsActive = entry.ApplicationSetting.IsActive
            };

            UpdateApplicationSetting(applicationSettingEditEntry);
        }
        public void UpdateApplicationSetting(ApplicationSettingEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ApplicationSettingRepository.FindById(entry.SettingId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundApplicationSetting, "ApplicationSetting", entry.SettingId, ErrorMessage.Messages[ErrorCode.NotFoundApplicationSetting]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.SettingName))
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidSettingName, "SettingName",null, ErrorMessage.Messages[ErrorCode.InvalidSettingName]));
                throw new ValidationError(violations);
            }
            else
            {

                if (entry.SettingName.Length > 250)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidSettingName, "SettingName",
                           entry.SettingName));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.SettingName != entry.SettingName)
                    {
                        bool isDuplicate = UnitOfWork.ApplicationSettingRepository.HasDataExisted(entry.SettingName);
                        if (isDuplicate)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateSettingName, "SettingName",
                                    entry.SettingName, ErrorMessage.Messages[ErrorCode.DuplicateSettingName]));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            entity.SettingName = entry.SettingName;
            entity.SettingValue = entry.SettingValue;
            entity.IsSecured = entry.IsSecured;
            entity.IsActive = entry.IsActive;

            UnitOfWork.ApplicationSettingRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateApplicationSettingStatus(int id, ApplicationSettingStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ApplicationSettingRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundApplicationSetting, "ApplicationSetting"));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(ApplicationSettingStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", status, ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            entity.IsActive = status;
            UnitOfWork.ApplicationSettingRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateApplicationSettingSecure(int id, bool isSecured)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ApplicationSettingRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundApplicationSetting, "ApplicationSetting"));
                throw new ValidationError(violations);
            }

            if (entity.IsSecured == isSecured) return;

            entity.IsSecured = isSecured;
            UnitOfWork.ApplicationSettingRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void DeleteApplicationSetting(int id)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ApplicationSettingRepository.Find(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullApplicationSetting, "id", id, ErrorMessage.Messages[ErrorCode.NullApplicationSetting]));
                throw new ValidationError(violations);
            }

            UnitOfWork.ApplicationSettingRepository.Delete(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region HOST SETTING from APPLICATION SETTINGS
        public bool GetApplicationSettingDataAsBoolean(Guid applicationId, string key, bool defaultValue)
        {
            bool retValue;
            string setting = UnitOfWork.ApplicationSettingRepository.GetApplicationSettingValue(applicationId, null, key);
            if (string.IsNullOrEmpty(setting))
            {
                retValue = defaultValue;
            }
            else
            {
                retValue = (setting.ToUpperInvariant().StartsWith("Y") || setting.ToUpperInvariant() == "TRUE" || setting == "1");
            }
            return retValue;
        }
        public double GetApplicationSettingDataAsDouble(Guid applicationId, string key, double defaultValue)
        {
            string setting = UnitOfWork.ApplicationSettingRepository.GetApplicationSettingValue(applicationId, null, key);
            var retValue = string.IsNullOrEmpty(setting) ? defaultValue : Convert.ToDouble(setting);
            return retValue;
        }
        public int GetApplicationSettingAsInteger(Guid applicationId, string key, int defaultValue)
        {
            string setting = UnitOfWork.ApplicationSettingRepository.GetApplicationSettingValue(applicationId, null, key);
            var retValue = string.IsNullOrEmpty(setting) ? defaultValue : Convert.ToInt32(setting);
            return retValue;
        }
        public string GetApplicationSettingDataAsString(Guid applicationId, string key, string defaultValue)
        {
            string setting = UnitOfWork.ApplicationSettingRepository.GetApplicationSettingValue(applicationId, null, key);
            var result = !string.IsNullOrEmpty(setting) ? setting : defaultValue;
            return result;
        }
        #endregion

        #region DATE-TIME-CONFIG

        public IEnumerable<SettingItemDetail> GetDateTimeConfigSettings(Guid applicationId)
        {
            var settings = UnitOfWork.ApplicationSettingRepository.GetDateTimeConfigSettings(applicationId);
            return settings.ToDtos<SettingItem, SettingItemDetail>();
        }
        public SettingItemDetail GetDateTimeConfigSetting(Guid applicationId, string keyName)
        {
            var setting = UnitOfWork.ApplicationSettingRepository.GetDateTimeConfigSetting(applicationId, keyName);
            return setting.ToDto<SettingItem, SettingItemDetail>();
        }
        public string GetDateTimeFormat(Guid applicationId)
        {
            var dateTimeFormat = UnitOfWork.ApplicationSettingRepository.GetDateTimeFormat(applicationId);
            return dateTimeFormat;
        }
        public string GetDateFormat(Guid applicationId)
        {
            var dateFormat = UnitOfWork.ApplicationSettingRepository.GetDateFormat(applicationId);
            return dateFormat;
        }
        public string GetTimeZone(Guid applicationId)
        {
            var timeZone = UnitOfWork.ApplicationSettingRepository.GetTimeZone(applicationId);
            return timeZone;
        }

        #endregion

        #region PAGE-CONFIG

        public IEnumerable<SettingItemDetail> GetPageConfigSettings(Guid applicationId)
        {
            var settings = UnitOfWork.ApplicationSettingRepository.GetPageConfigSettings(applicationId);
            return settings.ToDtos<SettingItem, SettingItemDetail>();
        }

        public SettingItemDetail GetPageConfigSetting(Guid applicationId, string keyName)
        {
            var setting = UnitOfWork.ApplicationSettingRepository.GetPageConfigSetting(applicationId, keyName);
            return setting.ToDto<SettingItem, SettingItemDetail>();
        }

        public int GetPageSizeForDefault(Guid applicationId)
        {
            var paymentMethodId = UnitOfWork.ApplicationSettingRepository.GetPageSizeForDefault(applicationId);
            return paymentMethodId;
        }
        
        #endregion

        #region FILE-CONFIG

        public IEnumerable<SettingItemDetail> GetFileConfigSettings(Guid applicationId)
        {
            var settings = UnitOfWork.ApplicationSettingRepository.GetFileConfigSettings(applicationId);
            return settings.ToDtos<SettingItem, SettingItemDetail>();
        }

        public SettingItemDetail GetFileConfigSetting(Guid applicationId, string keyName)
        {
            var setting = UnitOfWork.ApplicationSettingRepository.GetFileConfigSetting(applicationId, keyName);
            return setting.ToDto<SettingItem, SettingItemDetail>();
        }

        public string[] GetAllowedFileExtensions(Guid applicationId)
        {
            var settings = UnitOfWork.ApplicationSettingRepository.GetAllowedFileExtensions(applicationId);
            return settings;
        }

        public string[] GetAllowedImageExtensions(Guid applicationId)
        {
            var settings = UnitOfWork.ApplicationSettingRepository.GetAllowedImageExtensions(applicationId);
            return settings;
        }

        public string[] GetAllowedDocumentExtensions(Guid applicationId)
        {
            var settings = UnitOfWork.ApplicationSettingRepository.GetAllowedDocumentExtensions(applicationId);
            return settings;
        }
        public string[] GetAllowedVideoExtensions(Guid applicationId)
        {
            var settings = UnitOfWork.ApplicationSettingRepository.GetAllowedVideoExtensions(applicationId);
            return settings;
        }
      
        #endregion

        #region ORDER

        public IEnumerable<SettingItemDetail> GetOrderSettings(Guid applicationId)
        {
            var settings = UnitOfWork.ApplicationSettingRepository.GetOrderSettings(applicationId);
            return settings.ToDtos<SettingItem, SettingItemDetail>();
        }

        public SettingItemDetail GetOrderSetting(Guid applicationId, string keyName)
        {
            var setting = UnitOfWork.ApplicationSettingRepository.GetOrderSetting(applicationId, keyName);
            return setting.ToDto<SettingItem, SettingItemDetail>();
        }

        #endregion

        #region PAYMENT - PAYGATE - PAY METHOD

        public IEnumerable<SettingItemDetail> GetPayGateSettings(Guid applicationId)
        {
            var settings = UnitOfWork.ApplicationSettingRepository.GetPayGateSettings(applicationId);
            return settings.ToDtos<SettingItem, SettingItemDetail>();
        }

        public SettingItemDetail GetPayGateSetting(Guid applicationId, string rateName)
        {
            var setting = UnitOfWork.ApplicationSettingRepository.GetPayGateSetting(applicationId, rateName);
            return setting.ToDto<SettingItem, SettingItemDetail>();
        }
        #endregion

        #region PAYMENT - PAYPAL

        public IEnumerable<ApplicationSettingDetail> GetPaypalSettings(Guid applicationId)
        {
            var settings = UnitOfWork.ApplicationSettingRepository.GetPaypalSettings(applicationId);
            return settings.ToDtos<ApplicationSetting, ApplicationSettingDetail>();
        }

        public PaypalSettingDetail GetActivePaypalSetting(Guid applicationId)
        {
            var paymentSetting = UnitOfWork.ApplicationSettingRepository.GetActivePaypalSetting(applicationId);
            return paymentSetting.ToDto<PaypalSetting, PaypalSettingDetail>();
        }

        public PaypalSettingDetail GetPaypalSetting(Guid applicationId, string mode)
        {
            var paymentSetting = UnitOfWork.ApplicationSettingRepository.GetPaypalSetting(applicationId, mode);
            return paymentSetting.ToDto<PaypalSetting, PaypalSettingDetail>();
        }

        public void UpdatePaypalMode(Guid applicationId, string mode)
        {
            var settings = UnitOfWork.ApplicationSettingRepository.GetPaypalSettings(applicationId).ToList();
            if (!settings.Any()) return;

            string prefixPattern = string.Format("PAYPAL_{0}", mode.ToUpper());

            foreach (var item in settings)
            {
                if (item.SettingName.ToUpper().Contains(prefixPattern))
                {
                    if (item.IsActive != ApplicationSettingStatus.Active)
                    {
                        item.IsActive = ApplicationSettingStatus.Active;
                        UnitOfWork.ApplicationSettingRepository.Update(item);
                    }
                }
                else
                {
                    if (item.IsActive != ApplicationSettingStatus.InActive)
                    {
                        item.IsActive = ApplicationSettingStatus.InActive;
                        UnitOfWork.ApplicationSettingRepository.Update(item);
                    }
                }
            }
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region PAYMENT - STRIPE

        public IEnumerable<BaseSettingDetail> GetActiveStripeSettings(Guid applicationId)
        {
            var settings = UnitOfWork.ApplicationSettingRepository.GetActiveStripeSettings(applicationId);
            return settings.ToDtos<BaseSetting, BaseSettingDetail>();
        }
        public IEnumerable<SettingItemDetail> GetStripeSettings(Guid applicationId, string mode)
        {
            var settings = UnitOfWork.ApplicationSettingRepository.GetStripeSettings(applicationId, mode);
            return settings.ToDtos<SettingItem, SettingItemDetail>();
        }
        public SettingItemDetail GetStripeSetting(Guid applicationId, string mode, string keyName)
        {
            var setting = UnitOfWork.ApplicationSettingRepository.GetStripeSetting(applicationId, mode, keyName);
            return setting.ToDto<SettingItem, SettingItemDetail>();
        }
        public SettingItemDetail GetActiveStripeSetting(Guid applicationId, string keyName)
        {
            var setting = UnitOfWork.ApplicationSettingRepository.GetActiveStripeSetting(applicationId, keyName);
            return setting.ToDto<SettingItem, SettingItemDetail>();
        }

        #endregion

        #region TRANSACTION
        public IEnumerable<SettingItemDetail> GetTransactionSettings(Guid applicationId)
        {
            var settings = UnitOfWork.ApplicationSettingRepository.GetTransactionSettings(applicationId);
            return settings.ToDtos<SettingItem, SettingItemDetail>();
        }

        public SettingItemDetail GetTransactionSetting(Guid applicationId, string keyName)
        {
            var setting = UnitOfWork.ApplicationSettingRepository.GetTwitterSetting(applicationId, keyName);
            return setting.ToDto<SettingItem, SettingItemDetail>();
        }
        public int GetSelectedPaymentMethod(Guid applicationId)
        {
            var paymentMethodId = UnitOfWork.ApplicationSettingRepository.GetSelectedPaymentMethod(applicationId);
            return paymentMethodId;
        }
        #endregion

        #region RATING
        public IEnumerable<SettingItemDetail> GetRateSettings(Guid applicationId)
        {
            var settings = UnitOfWork.ApplicationSettingRepository.GetRateSettings(applicationId);
            return settings.ToDtos<SettingItem, SettingItemDetail>();
        }

        public SettingItemDetail GetRateSetting(Guid applicationId, string rateName)
        {
            var setting = UnitOfWork.ApplicationSettingRepository.GetRateSetting(applicationId, rateName);
            return setting.ToDto<SettingItem, SettingItemDetail>();
        }

        #endregion

        #region SMTP
        public IEnumerable<SettingItemDetail> GetSmtpSettings(Guid applicationId)
        {
            var settings = UnitOfWork.ApplicationSettingRepository.GetSmtpSettings(applicationId);
            return settings.ToDtos<SettingItem, SettingItemDetail>();
        }

        public SettingItemDetail GetSmtpSetting(Guid applicationId, string keyName)
        {
            var setting = UnitOfWork.ApplicationSettingRepository.GetSmtpSetting(applicationId, keyName);
            return setting.ToDto<SettingItem, SettingItemDetail>();
        }

        #endregion

        #region GOOGLE RECAPTCHA
        public IEnumerable<SettingItemDetail> GetRecaptchaSettings(Guid applicationId)
        {
            var settings = UnitOfWork.ApplicationSettingRepository.GetRecaptchaSettings(applicationId);
            return settings.ToDtos<SettingItem, SettingItemDetail>();
        }

        public SettingItemDetail GetRecaptchaSetting(Guid applicationId, string keyName)
        {
            var setting = UnitOfWork.ApplicationSettingRepository.GetRecaptchaSetting(applicationId, keyName);
            return setting.ToDto<SettingItem, SettingItemDetail>();
        }

        #endregion

        #region TWITTER
        public IEnumerable<SettingItemDetail> GetTwitterSettings(Guid applicationId)
        {
            var settings = UnitOfWork.ApplicationSettingRepository.GetTwitterSettings(applicationId);
            return settings.ToDtos<SettingItem, SettingItemDetail>();
        }

        public SettingItemDetail GetTwitterSetting(Guid applicationId, string keyName)
        {
            var setting = UnitOfWork.ApplicationSettingRepository.GetTwitterSetting(applicationId, keyName);
            return setting.ToDto<SettingItem, SettingItemDetail>();
        }

        #endregion

        #region FACEBOOK
        public IEnumerable<SettingItemDetail> GetFacebookSettings(Guid applicationId)
        {
            var settings = UnitOfWork.ApplicationSettingRepository.GetFacebookSettings(applicationId);
            return settings.ToDtos<SettingItem, SettingItemDetail>();
        }

        public SettingItemDetail GetFacebookSetting(Guid applicationId, string keyName)
        {
            var setting = UnitOfWork.ApplicationSettingRepository.GetTwitterSetting(applicationId, keyName);
            return setting.ToDto<SettingItem, SettingItemDetail>();
        }

        #endregion

        #region PRODUCT FILTERED PRICE RANGE

        //public int GetProductFilteredPriceMin(Guid applicationId)
        //{
        //    string setting = UnitOfWork.ApplicationSettingRepository.GetApplicationSettingValue(applicationId, null, SettingKeys.ProductFilteredPriceMin);
        //    var retValue = string.IsNullOrEmpty(setting) ? 5 : Convert.ToInt32(setting);
        //    return retValue;
        //}

        //public int GetProductFilteredPriceMax(Guid applicationId)
        //{
        //    string setting = UnitOfWork.ApplicationSettingRepository.GetApplicationSettingValue(applicationId, null, SettingKeys.ProductFilteredPriceMax);
        //    var retValue = string.IsNullOrEmpty(setting) ? 5 : Convert.ToInt32(setting);
        //    return retValue;
        //}

        public IEnumerable<SettingItemDetail> GetProductFilteredPriceRangeSettings(Guid applicationId)
        {
            var settings = UnitOfWork.ApplicationSettingRepository.GetProductFilteredPriceRangeSettings(applicationId);
            return settings?.ToDtos<SettingItem, SettingItemDetail>();
        }

        public SettingItemDetail GetProductFilteredPriceRangeSetting(Guid applicationId, string keyName)
        {
            var setting = UnitOfWork.ApplicationSettingRepository.GetProductFilteredPriceRangeSetting(applicationId, keyName);
            return setting?.ToDto<SettingItem, SettingItemDetail>();
        }

        #endregion

        #region PRODUCT PRICE RANGE SETTING

        public IEnumerable<SettingItemDetail> GetProductPriceRangeSettings(Guid applicationId)
        {
            var settings = UnitOfWork.ApplicationSettingRepository.GetProductPriceRangeSettings(applicationId);
            return settings?.ToDtos<SettingItem, SettingItemDetail>();
        }

        public SettingItemDetail GetProductPriceRangeSetting(Guid applicationId, string keyName)
        {
            var setting = UnitOfWork.ApplicationSettingRepository.GetProductPriceRangeSetting(applicationId, keyName);
            return setting?.ToDto<SettingItem, SettingItemDetail>();
        }

        #endregion

        #region LANGUAGE
        public IEnumerable<SettingItemDetail> GetLanguageSettings(Guid applicationId)
        {
            var settings = UnitOfWork.ApplicationSettingRepository.GetLanguageSettings(applicationId);
            return settings?.ToDtos<SettingItem, SettingItemDetail>();
        }

        public SettingItemDetail GetLanguageSetting(Guid applicationId, string keyName)
        {
            var setting = UnitOfWork.ApplicationSettingRepository.GetLanguageSetting(applicationId, keyName);
            return setting?.ToDto<SettingItem, SettingItemDetail>();
        }

        public void UpdateDefaultLanguage(Guid applicationId, string languageCode)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ApplicationRepository.FindById(applicationId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundApplication, "Application"));
                throw new ValidationError(violations);
            }
            
            entity.DefaultLanguage = languageCode;
           
            UnitOfWork.ApplicationRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        #endregion

        #region CURRENCY
        public IEnumerable<SettingItemDetail> GetCurrencySettings(Guid applicationId)
        {
            var settings = UnitOfWork.ApplicationSettingRepository.GetCurrencySettings(applicationId);
            return settings.ToDtos<SettingItem, SettingItemDetail>();
        }

        public SettingItemDetail GetCurrencySetting(Guid applicationId, string keyName)
        {
            var setting = UnitOfWork.ApplicationSettingRepository.GetCurrencySetting(applicationId, keyName);
            return setting.ToDto<SettingItem, SettingItemDetail>();
        }

        #endregion

        #region ADDRESS
        public IEnumerable<SettingItemDetail> GetAddressSettings(Guid applicationId)
        {
            var settings = UnitOfWork.ApplicationSettingRepository.GetAddressSettings(applicationId);
            return settings.ToDtos<SettingItem, SettingItemDetail>();
        }

        public SettingItemDetail GetAddressSetting(Guid applicationId, string keyName)
        {
            var setting = UnitOfWork.ApplicationSettingRepository.GetAddressSetting(applicationId, keyName);
            return setting.ToDto<SettingItem, SettingItemDetail>();
        }

        #endregion
        
        #region ERROR

        public void HandleError()
        {
            var exception = HttpContext.Current.Server.GetLastError();
            int statusCode = 404;
            string action = "Index";
            if (exception == null) return;

            if (exception is HttpException)
            {
                //if (httpEx.WebEventCode == WebEventCodes.RuntimeErrorPostTooLarge)
                //{
                //    //handle the error
                //    Response.Write("Too big a file, dude"); //for example
                //}
                var httpEx = exception as HttpException;
                statusCode = httpEx.GetHttpCode();

                switch (httpEx.GetHttpCode())
                {
                    case 0:
                        action = "NetworkError";
                        break;
                    case 302:
                        action = "RequestTypeViolation";
                        break;
                    case 401:
                        action = "Unauthorized";
                        break;
                    case 403:
                        action = "SessionExpired";
                        break;
                    case 404:
                        action = "NotFound";
                        break;
                    case 500:
                        action = "InternalServerError";
                        break;
                    case 503:
                        action = "ServiceUnavailable";
                        break;
                    case 590:
                        action = "TimeOut";
                        break;
                    default:
                        action = "Index";
                        break;
                }
            }
            else if (exception is CryptographicException)
            {
                FormsAuthentication.SignOut();
                HttpContext.Current.Server.ClearError();
            }

            // You've handled the error, so clear it. Leaving the server in an error state can cause unintended side effects as the server continues its attempts to handle the error.
            HttpContext.Current.Server.ClearError();

            // Possible that a partially rendered page has already been written to response buffer before encountering error, so clear it.
            HttpContext.Current.Response.Clear();
            //HttpContext.Current.ClearError();

            HttpContext.Current.Response.StatusCode = statusCode;
            HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();
            HttpContext.Current.Response.TrySkipIisCustomErrors = true;
            HttpContext.Current.Response.Redirect("/Error/" + action, true);
        }

        #endregion

        #region DELIVERY - SHIPPING
        public IEnumerable<SettingItemDetail> GetDeliverySettings(Guid applicationId)
        {
            var settings = UnitOfWork.ApplicationSettingRepository.GetDeliverySettings(applicationId);
            return settings?.ToDtos<SettingItem, SettingItemDetail>();
        }

        public SettingItemDetail GetDeliverySetting(Guid applicationId, string keyName)
        {
            var setting = UnitOfWork.ApplicationSettingRepository.GetDeliverySetting(applicationId, keyName);
            return setting?.ToDto<SettingItem, SettingItemDetail>();
        }

        public void UpdateDeliverySettingStatus(int settingId, string keyName, string keyValue)
        {
            var violations = new List<RuleViolation>();
            var setting = UnitOfWork.ApplicationSettingRepository.FindById(settingId);
            if (setting == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullApplicationSetting, "SettingId", settingId));
                throw new ValidationError(violations);
            }

            var baseSettings = JsonConvert.DeserializeObject<List<BaseSettingDetail>>(setting.SettingValue);

            var baseSetting = baseSettings.FirstOrDefault(x => x.KeyName.ToLower() == keyName.ToLower());
            if (baseSetting == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullMenuEntry, "KeyName", keyName));
                throw new ValidationError(violations);
            }

            baseSetting.KeyValue = keyValue;

            var settingValue = JsonConvert.SerializeObject(baseSettings);
            var applicationEditEntry = new ApplicationSettingEditEntry
            {
                SettingId = setting.SettingId,
                SettingName = setting.SettingName,
                SettingValue = settingValue,
                IsSecured = setting.IsSecured,
                IsActive = setting.IsActive
            };

            UpdateApplicationSetting(applicationEditEntry);
        }

        #endregion

        #region NOTIFICATION
        public IEnumerable<SettingItemDetail> GetNotificationSettings(Guid applicationId)
        {
            var settings = UnitOfWork.ApplicationSettingRepository.GetNotificationSettings(applicationId);
            return settings?.ToDtos<SettingItem, SettingItemDetail>();
        }

        public SettingItemDetail GetNotificationSetting(Guid applicationId, string keyName)
        {
            var setting = UnitOfWork.ApplicationSettingRepository.GetNotificationSetting(applicationId, keyName);
            return setting?.ToDto<SettingItem, SettingItemDetail>();
        }

        public void UpdateNotificationSettingStatus(int settingId, string keyName, string keyValue)
        {
            var violations = new List<RuleViolation>();
            var setting = UnitOfWork.ApplicationSettingRepository.FindById(settingId);
            if (setting == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullApplicationSetting, "SettingId", settingId));
                throw new ValidationError(violations);
            }

            var baseSettings = JsonConvert.DeserializeObject<List<BaseSettingDetail>>(setting.SettingValue);

            var baseSetting = baseSettings.FirstOrDefault(x => x.KeyName.ToLower() == keyName.ToLower());
            if (baseSetting == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullMenuEntry, "KeyName", keyName));
                throw new ValidationError(violations);
            }

            baseSetting.KeyValue = keyValue;

            var settingValue = JsonConvert.SerializeObject(baseSettings);
            var applicationEditEntry = new ApplicationSettingEditEntry
            {
                SettingId = setting.SettingId,
                SettingName = setting.SettingName,
                SettingValue = settingValue,
                IsSecured = setting.IsSecured,
                IsActive = setting.IsActive
            };

            UpdateApplicationSetting(applicationEditEntry);
        }

        #endregion

        #region Dipose

        private bool _disposed = false;
        protected override void Dispose(bool isDisposing)
        {
            if (!this._disposed)
            {
                if (isDisposing)
                {
                    CacheService = null;
                    LanguageService = null;
                    ThemeService = null;
                    VendorService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion

    }
}
