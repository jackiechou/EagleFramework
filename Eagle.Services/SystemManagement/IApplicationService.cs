using System;
using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.Dtos.SystemManagement.Settings;

namespace Eagle.Services.SystemManagement
{
    public interface IApplicationService: IBaseService
    {
        #region Configuration Application Setting

        void SetUpAppConfig(Guid applicationId);
        void SetUpCulture(Guid applicationId);
        int GetCurrentVendorId();
        string GetCurrentCurrencyCode();
        string GetCurrentLanguageCode(Guid applicationId);
        #endregion

        #region Application

        IEnumerable<ApplicationDetail> GetApplications(ref int? recordCount, string orderBy = null,
            int? page = null, int? pageSize = null);
        ApplicationDetail GetApplicationInfo(Guid applicationId);
        ApplicationDetail GetApplicationDetail(Guid id);
        string GetDefaultLanguage(Guid applicationId);
        bool HasApplicationExisted(string applicationName);
        void InsertApplication(ApplicationEntry entry);
        void UpdateApplication(Guid id, ApplicationEntry entry);
        #endregion

        #region Application Setting

        IEnumerable<ApplicationSettingDetail> GetApplicationSettings(Guid applicationId);
        string GetDefaultApplicationSetting(Guid applicationId, string settingName);
        string GetDefaultApplicationSetting(Guid applicationId, string settingName, string defaultValue);
        ApplicationSettingDetail GetApplicationSettingDetail(int id);
        void InsertApplicationSetting(Guid applicationId, ApplicationSettingEntry entry);
        void EditApplicationSetting(SettingItemEditEntry entry);
        void UpdateApplicationSetting(ApplicationSettingEditEntry entry);
        void UpdateApplicationSettingSecure(int id, bool isSecured);
        void UpdateApplicationSettingStatus(int id, ApplicationSettingStatus status);
        void DeleteApplicationSetting(int id);

        bool GetApplicationSettingDataAsBoolean(Guid applicationId, string key, bool defaultValue);
        double GetApplicationSettingDataAsDouble(Guid applicationId, string key, double defaultValue);
        int GetApplicationSettingAsInteger(Guid applicationId, string key, int defaultValue);
        string GetApplicationSettingDataAsString(Guid applicationId, string key, string defaultValue);

        #endregion

        #region DATE-TIME-CONFIG

        IEnumerable<SettingItemDetail> GetDateTimeConfigSettings(Guid applicationId);
        SettingItemDetail GetDateTimeConfigSetting(Guid applicationId, string keyName);
        string GetDateTimeFormat(Guid applicationId);
        string GetDateFormat(Guid applicationId);
        string GetTimeZone(Guid applicationId);

        #endregion

        #region PAGE-CONFIG

        IEnumerable<SettingItemDetail> GetPageConfigSettings(Guid applicationId);
        SettingItemDetail GetPageConfigSetting(Guid applicationId, string keyName);
        int GetPageSizeForDefault(Guid applicationId);
        #endregion

        #region FILE-CONFIG

        IEnumerable<SettingItemDetail> GetFileConfigSettings(Guid applicationId);
        SettingItemDetail GetFileConfigSetting(Guid applicationId, string keyName);
        string[] GetAllowedFileExtensions(Guid applicationId);
        string[] GetAllowedImageExtensions(Guid applicationId);
        string[] GetAllowedDocumentExtensions(Guid applicationId);
        string[] GetAllowedVideoExtensions(Guid applicationId);

        #endregion

        #region ORDER

        IEnumerable<SettingItemDetail> GetOrderSettings(Guid applicationId);
        SettingItemDetail GetOrderSetting(Guid applicationId, string keyName);

        #endregion

        #region PAYMENT - PAYGATE

        IEnumerable<SettingItemDetail> GetPayGateSettings(Guid applicationId);

        SettingItemDetail GetPayGateSetting(Guid applicationId, string rateName);

        #endregion

        #region PAYMENT - PAYPAL

        IEnumerable<ApplicationSettingDetail> GetPaypalSettings(Guid applicationId);
        PaypalSettingDetail GetActivePaypalSetting(Guid applicationId);
        PaypalSettingDetail GetPaypalSetting(Guid applicationId, string mode);
        void UpdatePaypalMode(Guid applicationId, string mode);

        #endregion

        #region PAYMENT - STRIPE
        IEnumerable<BaseSettingDetail> GetActiveStripeSettings(Guid applicationId);
        IEnumerable<SettingItemDetail> GetStripeSettings(Guid applicationId, string mode);
        SettingItemDetail GetStripeSetting(Guid applicationId, string mode, string keyName);
        SettingItemDetail GetActiveStripeSetting(Guid applicationId, string keyName);

        #endregion

        #region TRANSACTION

        IEnumerable<SettingItemDetail> GetTransactionSettings(Guid applicationId);

        SettingItemDetail GetTransactionSetting(Guid applicationId, string keyName);
        int GetSelectedPaymentMethod(Guid applicationId);
        #endregion

        #region RATING

        IEnumerable<SettingItemDetail> GetRateSettings(Guid applicationId);
        SettingItemDetail GetRateSetting(Guid applicationId, string rateName);

        #endregion

        #region SMTP

        IEnumerable<SettingItemDetail> GetSmtpSettings(Guid applicationId);
        SettingItemDetail GetSmtpSetting(Guid applicationId, string keyName);

        #endregion

        #region GOOGLE RECAPTCHA

        IEnumerable<SettingItemDetail> GetRecaptchaSettings(Guid applicationId);

        SettingItemDetail GetRecaptchaSetting(Guid applicationId, string keyName);

        #endregion
        
        #region TWITTER

        IEnumerable<SettingItemDetail> GetTwitterSettings(Guid applicationId);

        SettingItemDetail GetTwitterSetting(Guid applicationId, string keyName);

        #endregion

        #region FACEBOOK

        IEnumerable<SettingItemDetail> GetFacebookSettings(Guid applicationId);

        SettingItemDetail GetFacebookSetting(Guid applicationId, string keyName);

        #endregion

        #region LANGUAGE

        IEnumerable<SettingItemDetail> GetLanguageSettings(Guid applicationId);
        SettingItemDetail GetLanguageSetting(Guid applicationId, string keyName);
        void UpdateDefaultLanguage(Guid applicationId, string languageCode);

        #endregion

        #region CURRENCY

        IEnumerable<SettingItemDetail> GetCurrencySettings(Guid applicationId);

        SettingItemDetail GetCurrencySetting(Guid applicationId, string keyName);

        #endregion

        #region PRODUCT FILTERED PRICE RANGE
        //int GetProductFilteredPriceMin(Guid applicationId);
        //int GetProductFilteredPriceMax(Guid applicationId);
        IEnumerable<SettingItemDetail> GetProductFilteredPriceRangeSettings(Guid applicationId);

        SettingItemDetail GetProductFilteredPriceRangeSetting(Guid applicationId, string keyName);
        #endregion

        #region PRODUCT PRICE RANGE SETTING

        IEnumerable<SettingItemDetail> GetProductPriceRangeSettings(Guid applicationId);

        SettingItemDetail GetProductPriceRangeSetting(Guid applicationId, string keyName);

        #endregion

        #region ADDRESS

        IEnumerable<SettingItemDetail> GetAddressSettings(Guid applicationId);

        SettingItemDetail GetAddressSetting(Guid applicationId, string keyName);

        #endregion

        #region ERROR

        void HandleError();

        #endregion

        #region DELIVERY - SHIPPING

        IEnumerable<SettingItemDetail> GetDeliverySettings(Guid applicationId);
        SettingItemDetail GetDeliverySetting(Guid applicationId, string keyName);
        void UpdateDeliverySettingStatus(int settingId, string keyName, string keyValue);

        #endregion

        #region NOTIFICATION

        IEnumerable<SettingItemDetail> GetNotificationSettings(Guid applicationId);
        SettingItemDetail GetNotificationSetting(Guid applicationId, string keyName);
        void UpdateNotificationSettingStatus(int settingId, string keyName, string keyValue);
    

        #endregion
    }
}
