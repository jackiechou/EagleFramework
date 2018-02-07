using System;
using System.Collections.Generic;
using Eagle.Entities.SystemManagement;
using Eagle.Entities.SystemManagement.Settings;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IApplicationSettingRepository : IRepositoryBase<ApplicationSetting>
    {
        #region Base Settings

        IEnumerable<ApplicationSetting> GetApplicationSettings(Guid applicationId);
        Dictionary<string, string> GetDictionaryApplicationSettings(Guid applicationId, bool? isSecured);
        string GetApplicationSettingValue(Guid applicationId, bool? isSecured, string settingName);

        bool HasDataExisted(string settingName);
        string GetApplicationSetting(Guid applicationId, string settingName, string defaultValue);
        string GetDefaultApplicationSetting(Guid applicationId, string settingName, string defaultValue);
        bool HasDataExisted(Guid applicationId, string settingName);

        #endregion

        #region DATE-TIME-CONFIG

        List<SettingItem> GetDateTimeConfigSettings(Guid applicationId);
        SettingItem GetDateTimeConfigSetting(Guid applicationId, string keyName);
        string GetDateTimeFormat(Guid applicationId);
        string GetDateFormat(Guid applicationId);
        string GetTimeZone(Guid applicationId);

        #endregion

        #region PAGE-CONFIG

        List<SettingItem> GetPageConfigSettings(Guid applicationId);

        SettingItem GetPageConfigSetting(Guid applicationId, string keyName);
        
        int GetPageSizeForDefault(Guid applicationId);

        #endregion
        
        #region FILE CONFIG

        List<SettingItem> GetFileConfigSettings(Guid applicationId);
        SettingItem GetFileConfigSetting(Guid applicationId, string keyName);
        string[] GetAllowedFileExtensions(Guid applicationId);
        string[] GetAllowedDocumentExtensions(Guid applicationId);
        string[] GetAllowedVideoExtensions(Guid applicationId);
        string[] GetAllowedImageExtensions(Guid applicationId);
        int GetAllowedMaxImageContentLength(Guid applicationId);
        int GetAllowedMaxVideoContentLength(Guid applicationId);
        int GetAllowedMaxDocumentContentLength(Guid applicationId);

        #endregion

        #region REPORT
        string GetDefaultReportPageHeaderText(Guid applicationId);
        string GetDefaultReportPageFooterText(Guid applicationId);
        string GetDefaultReportHeaderText(Guid applicationId);
        string GetDefaultReportLogo(Guid applicationId);
        #endregion

        #region ORDER

        List<SettingItem> GetOrderSettings(Guid applicationId);
        SettingItem GetOrderSetting(Guid applicationId, string keyName);

        #endregion

        #region PAYMENT - PAYGATE

        List<SettingItem> GetPayGateSettings(Guid applicationId);

        SettingItem GetPayGateSetting(Guid applicationId, string keyName);

        #endregion

        #region PAYMENT - PAYPAL
        IEnumerable<ApplicationSetting> GetPaypalSettings(Guid applicationId);
        PaypalSetting GetActivePaypalSetting(Guid applicationId);
        PaypalSetting GetPaypalSetting(Guid applicationId, string mode);

        #endregion

        #region PAYMENT - STRIPE

        IEnumerable<BaseSetting> GetActiveStripeSettings(Guid applicationId);
        List<SettingItem> GetStripeSettings(Guid applicationId, string mode);
        SettingItem GetStripeSetting(Guid applicationId, string mode, string keyName);
        SettingItem GetActiveStripeSetting(Guid applicationId, string keyName);

        #endregion

        #region TRANSACTION

        List<SettingItem> GetTransactionSettings(Guid applicationId);

        SettingItem GetTransactionSetting(Guid applicationId, string keyName);
        int GetSelectedPaymentMethod(Guid applicationId);

        #endregion

        #region Rate Settings

        List<SettingItem> GetRateSettings(Guid applicationId);

        SettingItem GetRateSetting(Guid applicationId, string keyName);

        #endregion

        #region SMTP

        List<SettingItem> GetSmtpSettings(Guid applicationId);
        SettingItem GetSmtpSetting(Guid applicationId, string keyName);

        #endregion

        #region GOOGLE RECAPTCHA

        List<SettingItem> GetRecaptchaSettings(Guid applicationId);
        SettingItem GetRecaptchaSetting(Guid applicationId, string keyName);

        #endregion

        #region TWITTER

        List<SettingItem> GetTwitterSettings(Guid applicationId);

        SettingItem GetTwitterSetting(Guid applicationId, string keyName);

        #endregion

        #region FACEBOOK

        List<SettingItem> GetFacebookSettings(Guid applicationId);

        SettingItem GetFacebookSetting(Guid applicationId, string keyName);

        #endregion

        #region PRODUCT FILTERED PRICE RANGE SETTING

        List<SettingItem> GetProductFilteredPriceRangeSettings(Guid applicationId);
        SettingItem GetProductFilteredPriceRangeSetting(Guid applicationId, string keyName);

        #endregion

        #region PRODUCT PRICE RANGE SETTING

        List<SettingItem> GetProductPriceRangeSettings(Guid applicationId);
        SettingItem GetProductPriceRangeSetting(Guid applicationId, string keyName);

        #endregion

        #region LANGUAGE

        List<SettingItem> GetLanguageSettings(Guid applicationId);

        SettingItem GetLanguageSetting(Guid applicationId, string keyName);

        #endregion

        #region CURRENCY

        List<SettingItem> GetCurrencySettings(Guid applicationId);

        SettingItem GetCurrencySetting(Guid applicationId, string keyName);

        #endregion

        #region ADDRESS

        List<SettingItem> GetAddressSettings(Guid applicationId);

        SettingItem GetAddressSetting(Guid applicationId, string keyName);

        #endregion

        #region DELIVERY - SHIPPING

        List<SettingItem> GetDeliverySettings(Guid applicationId);

        SettingItem GetDeliverySetting(Guid applicationId, string keyName);

        #endregion

        #region NOTIFICATION

        List<SettingItem> GetNotificationSettings(Guid applicationId);

        SettingItem GetNotificationSetting(Guid applicationId, string keyName);

        #endregion
    }
}
