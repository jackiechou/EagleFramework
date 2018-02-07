using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Common.Utilities;
using Eagle.Core.Configuration;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.Entities.SystemManagement.Settings;
using Eagle.EntityFramework;
using Newtonsoft.Json;

namespace Eagle.Repositories.SystemManagement
{
    public class ApplicationSettingRepository : RepositoryBase<ApplicationSetting>, IApplicationSettingRepository
    {
        public ApplicationSettingRepository(IDataContext dataContext) : base(dataContext) { }

        #region Base Settings
        public IEnumerable<ApplicationSetting> GetApplicationSettings(Guid applicationId)
        {
            return DataContext.Get<ApplicationSetting>().Where(x => x.ApplicationId == applicationId).AsEnumerable();
        }

        public Dictionary<string, string> GetDictionaryApplicationSettings(Guid applicationId, bool? isSecured)
        {
            var dicSettings = new Dictionary<string, string>();
            var query = (from x in DataContext.Get<ApplicationSetting>()
                         where x.ApplicationId == applicationId
                         select x);

            if (isSecured != null)
            {
                query = query.Where(x => x.IsSecured == isSecured);
            }

            var lst = query.ToList();
            if (lst.Any())
            {
                foreach (var item in lst)
                {
                    dicSettings.Add(item.SettingName, item.SettingValue);
                }
            }
            return dicSettings;
        }

        public string GetApplicationSettingValue(Guid applicationId, bool? isSecured, string settingName)
        {
            GetDictionaryApplicationSettings(applicationId, isSecured).TryGetValue(settingName, out var settingValue);
            return settingValue;
        }

        public bool HasDataExisted(string settingName)
        {
            var result = DataContext.Get<ApplicationSetting>().FirstOrDefault(x => x.SettingName == settingName);
            return (result != null);
        }

        public string GetApplicationSetting(Guid applicationId, string settingName, string defaultValue)
        {
            string setting = DataContext.Get<ApplicationSetting>().Where(x => x.ApplicationId == applicationId && x.SettingName == settingName).Select(x => x.SettingValue).FirstOrDefault();
            return string.IsNullOrEmpty(setting) ? defaultValue : setting;
        }

        public string GetDefaultApplicationSetting(Guid applicationId, string settingName, string defaultValue)
        {
            string setting = DataContext.Get<ApplicationSetting>().Where(x => x.ApplicationId == applicationId && x.SettingName == settingName).Select(x => x.SettingValue).FirstOrDefault();
            return string.IsNullOrEmpty(setting) ? defaultValue : setting;
        }

        public bool HasDataExisted(Guid applicationId, string settingName)
        {
            var query = DataContext.Get<ApplicationSetting>().FirstOrDefault(p => p.ApplicationId == applicationId && p.SettingName == settingName);
            return (query != null);
        }

        #endregion

        #region DATE-TIME-CONFIG
        public List<SettingItem> GetDateTimeConfigSettings(Guid applicationId)
        {
            string prefixPattern = "DATE_TIME_CONFIG";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.ToUpper() == prefixPattern);
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return null;

            var settings = new List<SettingItem>();
            foreach (var item in baseSettings)
            {
                var setting = new SettingItem
                {
                    ApplicationId = aplicationSetting.ApplicationId,
                    SettingId = aplicationSetting.SettingId,
                    SettingName = aplicationSetting.SettingName,
                    SettingValue = aplicationSetting.SettingValue,
                    IsSecured = aplicationSetting.IsSecured,
                    IsActive = aplicationSetting.IsActive,
                    Setting = item
                };
                settings.Add(setting);
            }

            return settings;
        }

        public SettingItem GetDateTimeConfigSetting(Guid applicationId, string keyName)
        {
            string prefixPattern = "DATE_TIME_CONFIG";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return null;

            var baseSetting = baseSettings.FirstOrDefault(x => x.KeyName.ToLower() == keyName.ToLower());
            if (baseSetting == null) return null;

            var setting = new SettingItem
            {
                ApplicationId = aplicationSetting.ApplicationId,
                SettingId = aplicationSetting.SettingId,
                SettingName = aplicationSetting.SettingName,
                SettingValue = aplicationSetting.SettingValue,
                IsSecured = aplicationSetting.IsSecured,
                IsActive = aplicationSetting.IsActive,
                Setting = baseSetting
            };

            return setting;
        }

        public string GetDateTimeFormat(Guid applicationId)
        {
            string prefixPattern = "DATE_TIME_CONFIG";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return GlobalSettings.DefaultDateTimeFormat;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return GlobalSettings.DefaultDateTimeFormat;

            var baseSetting = baseSettings.FirstOrDefault(x => string.Equals(x.KeyName, DateTimeConfig.DateTimeFormat, StringComparison.CurrentCultureIgnoreCase));
            if (baseSetting == null) return GlobalSettings.DefaultDateTimeFormat;

            string dateTimeFormat = baseSetting.KeyValue;
            return dateTimeFormat;
        }

        public string GetDateFormat(Guid applicationId)
        {
            string prefixPattern = "DATE_TIME_CONFIG";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return GlobalSettings.DefaultDateFormat;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return GlobalSettings.DefaultDateFormat;

            var baseSetting = baseSettings.FirstOrDefault(x => string.Equals(x.KeyName, DateTimeConfig.DateTimeFormat, StringComparison.CurrentCultureIgnoreCase));
            if (baseSetting == null) return GlobalSettings.DefaultDateFormat;

            string dateFormat = baseSetting.KeyValue;
            return dateFormat;
        }

        public string GetTimeZone(Guid applicationId)
        {
            string prefixPattern = "DATE_TIME_CONFIG";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return GlobalSettings.DefaultTimeZone;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return GlobalSettings.DefaultTimeZone;

            var baseSetting = baseSettings.FirstOrDefault(x => string.Equals(x.KeyName, DateTimeConfig.TimeZone, StringComparison.CurrentCultureIgnoreCase));
            if (baseSetting == null) return GlobalSettings.DefaultTimeZone;

            string timeZone = baseSetting.KeyValue;
            return timeZone;
        }

        #endregion
        
        #region PAGE-CONFIG
        public List<SettingItem> GetPageConfigSettings(Guid applicationId)
        {
            string prefixPattern = "PAGE_CONFIG";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.ToUpper() == prefixPattern);
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return null;

            var settings = new List<SettingItem>();
            foreach (var item in baseSettings)
            {
                var setting = new SettingItem
                {
                    ApplicationId = aplicationSetting.ApplicationId,
                    SettingId = aplicationSetting.SettingId,
                    SettingName = aplicationSetting.SettingName,
                    SettingValue = aplicationSetting.SettingValue,
                    IsSecured = aplicationSetting.IsSecured,
                    IsActive = aplicationSetting.IsActive,
                    Setting = item
                };
                settings.Add(setting);
            }

            return settings;
        }

        public SettingItem GetPageConfigSetting(Guid applicationId, string keyName)
        {
            string prefixPattern = "PAGE_CONFIG";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return null;

            var baseSetting = baseSettings.FirstOrDefault(x => x.KeyName.ToLower() == keyName.ToLower());
            if (baseSetting == null) return null;

            var setting = new SettingItem
            {
                ApplicationId = aplicationSetting.ApplicationId,
                SettingId = aplicationSetting.SettingId,
                SettingName = aplicationSetting.SettingName,
                SettingValue = aplicationSetting.SettingValue,
                IsSecured = aplicationSetting.IsSecured,
                IsActive = aplicationSetting.IsActive,
                Setting = baseSetting
            };

            return setting;
        }

        public int GetPageSizeForDefault(Guid applicationId)
        {
            string prefixPattern = "PAGE_CONFIG";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return GlobalSettings.MaxImageContentLength;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return GlobalSettings.MaxImageContentLength;

            var baseSetting = baseSettings.FirstOrDefault(x => string.Equals(x.KeyName, PageConfig.PageSizeForDefault, StringComparison.CurrentCultureIgnoreCase));
            if (baseSetting == null) return GlobalSettings.MaxImageContentLength;

            int maxImageContentLength = Convert.ToInt32(baseSetting.KeyValue);
            return maxImageContentLength;
        }
        
        #endregion

        #region FILE-CONFIG

        public List<SettingItem> GetFileConfigSettings(Guid applicationId)
        {
            string prefixPattern = "FILE_CONFIG";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.ToUpper() == prefixPattern);
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return null;

            var settings = new List<SettingItem>();
            foreach (var item in baseSettings)
            {
                var setting = new SettingItem
                {
                    ApplicationId = aplicationSetting.ApplicationId,
                    SettingId = aplicationSetting.SettingId,
                    SettingName = aplicationSetting.SettingName,
                    SettingValue = aplicationSetting.SettingValue,
                    IsSecured = aplicationSetting.IsSecured,
                    IsActive = aplicationSetting.IsActive,
                    Setting = item
                };
                settings.Add(setting);
            }

            return settings;
        }

        public SettingItem GetFileConfigSetting(Guid applicationId, string keyName)
        {
            string prefixPattern = "FILE_CONFIG";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return null;

            var baseSetting = baseSettings.FirstOrDefault(x => x.KeyName.ToLower() == keyName.ToLower());
            if (baseSetting == null) return null;

            var setting = new SettingItem
            {
                ApplicationId = aplicationSetting.ApplicationId,
                SettingId = aplicationSetting.SettingId,
                SettingName = aplicationSetting.SettingName,
                SettingValue = aplicationSetting.SettingValue,
                IsSecured = aplicationSetting.IsSecured,
                IsActive = aplicationSetting.IsActive,
                Setting = baseSetting
            };

            return setting;
        }

        public string[] GetAllowedFileExtensions(Guid applicationId)
        {
            string prefixPattern = "FILE_CONFIG";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return null;

            var baseSetting = baseSettings.FirstOrDefault(x => string.Equals(x.KeyName, FileConfig.FileExtensions, StringComparison.CurrentCultureIgnoreCase));
            if (baseSetting == null) return null;

            string[] extensions = baseSetting.KeyValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return extensions;
        }

        public string[] GetAllowedDocumentExtensions(Guid applicationId)
        {
            string prefixPattern = "FILE_CONFIG";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return null;

            var baseSetting = baseSettings.FirstOrDefault(x => string.Equals(x.KeyName, FileConfig.DocumentExtensions, StringComparison.CurrentCultureIgnoreCase));
            if (baseSetting == null) return null;

            string[] extensions = baseSetting.KeyValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return extensions;
        }

        public string[] GetAllowedVideoExtensions(Guid applicationId)
        {
            string prefixPattern = "FILE_CONFIG";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return null;

            var baseSetting = baseSettings.FirstOrDefault(x => string.Equals(x.KeyName, FileConfig.VideoExtensions, StringComparison.CurrentCultureIgnoreCase));
            if (baseSetting == null) return null;

            string[] extensions = baseSetting.KeyValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return extensions;
        }
        
        public string[] GetAllowedImageExtensions(Guid applicationId)
        {
            string prefixPattern = "FILE_CONFIG";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return null;

            var baseSetting = baseSettings.FirstOrDefault(x => string.Equals(x.KeyName, FileConfig.ImageExtensions, StringComparison.CurrentCultureIgnoreCase));
            if (baseSetting == null) return null;

            string[] extensions = baseSetting.KeyValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return extensions;
        }

        public int GetAllowedMaxImageContentLength(Guid applicationId)
        {
            string prefixPattern = "FILE_CONFIG";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return GlobalSettings.MaxImageContentLength;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return GlobalSettings.MaxImageContentLength;

            var baseSetting = baseSettings.FirstOrDefault(x => string.Equals(x.KeyName, FileConfig.MaxImageContentLength, StringComparison.CurrentCultureIgnoreCase));
            if (baseSetting == null) return GlobalSettings.MaxImageContentLength;

            int  maxImageContentLength = Convert.ToInt32(baseSetting.KeyValue);
            return maxImageContentLength;
        }

        public int GetAllowedMaxVideoContentLength(Guid applicationId)
        {
            string prefixPattern = "FILE_CONFIG";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return GlobalSettings.MaxVideoContentLength;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return GlobalSettings.MaxVideoContentLength;

            var baseSetting = baseSettings.FirstOrDefault(x => string.Equals(x.KeyName, FileConfig.MaxVideoContentLength, StringComparison.CurrentCultureIgnoreCase));
            if (baseSetting == null) return GlobalSettings.MaxVideoContentLength;

            int maxImageContentLength = Convert.ToInt32(baseSetting.KeyValue);
            return maxImageContentLength;
        }

        public int GetAllowedMaxDocumentContentLength(Guid applicationId)
        {
            string prefixPattern = "FILE_CONFIG";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return GlobalSettings.MaxDocumentContentLength;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return GlobalSettings.MaxDocumentContentLength;

            var baseSetting = baseSettings.FirstOrDefault(x => string.Equals(x.KeyName, FileConfig.MaxDocumentContentLength, StringComparison.CurrentCultureIgnoreCase));
            if (baseSetting == null) return GlobalSettings.MaxDocumentContentLength;

            int maxImageContentLength = Convert.ToInt32(baseSetting.KeyValue);
            return maxImageContentLength;
        }
       
        #endregion

        #region REPORT

        public string GetDefaultReportPageHeaderText(Guid applicationId)
        {
            return GetApplicationSetting(applicationId, "ReportPageHeaderText", Null.NullString);
        }

        public string GetDefaultReportPageFooterText(Guid applicationId)
        {
            return GetApplicationSetting(applicationId, "ReportPageFooterText", Null.NullString);
        }

        public string GetDefaultReportHeaderText(Guid applicationId)
        {
            return GetApplicationSetting(applicationId, "ReportHeaderText", Null.NullString);
        }

        public string GetDefaultReportLogo(Guid applicationId)
        {
            return GetApplicationSetting(applicationId, "ReportLogo", Null.NullString);
        }

        #endregion

        #region ORDER

        public List<SettingItem> GetOrderSettings(Guid applicationId)
        {
            string prefixPattern = "ORDER";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.ToUpper() == prefixPattern);
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return null;

            var settings = new List<SettingItem>();
            foreach (var item in baseSettings)
            {
                var setting = new SettingItem
                {
                    ApplicationId = aplicationSetting.ApplicationId,
                    SettingId = aplicationSetting.SettingId,
                    SettingName = aplicationSetting.SettingName,
                    SettingValue = aplicationSetting.SettingValue,
                    IsSecured = aplicationSetting.IsSecured,
                    IsActive = aplicationSetting.IsActive,
                    Setting = item
                };
                settings.Add(setting);
            }

            return settings;
        }

        public SettingItem GetOrderSetting(Guid applicationId, string keyName)
        {
            string prefixPattern = "ORDER";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return null;

            var baseSetting = baseSettings.FirstOrDefault(x => x.KeyName.ToLower() == keyName.ToLower());
            if (baseSetting == null) return null;

            var setting = new SettingItem
            {
                ApplicationId = aplicationSetting.ApplicationId,
                SettingId = aplicationSetting.SettingId,
                SettingName = aplicationSetting.SettingName,
                SettingValue = aplicationSetting.SettingValue,
                IsSecured = aplicationSetting.IsSecured,
                IsActive = aplicationSetting.IsActive,
                Setting = baseSetting
            };

            return setting;
        }

        #endregion

        #region PAYMENT - PAYGATE
        public IEnumerable<BaseSetting> GetPayGateBaseSettings(Guid applicationId)
        {
            string prefixPattern = "PAYGATE";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.IsActive == ApplicationSettingStatus.Active && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return null;

            var settings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(aplicationSetting.SettingValue).ToList();
            return settings;
        }

        /// <summary>
        /// Get PayGate Settings
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public List<SettingItem> GetPayGateSettings(Guid applicationId)
        {
            string prefixPattern = "PAYGATE";
            var setting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.ToUpper() == prefixPattern);
            if (setting == null) return null;

            var lst = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(setting.SettingValue).ToList();
            if (!lst.Any()) return null;

            var settingItems = new List<SettingItem>();
            foreach (var item in lst)
            {
                var settingItem = new SettingItem
                {
                    ApplicationId = setting.ApplicationId,
                    SettingId = setting.SettingId,
                    SettingName = setting.SettingName,
                    SettingValue = setting.SettingValue,
                    IsSecured = setting.IsSecured,
                    IsActive = setting.IsActive,
                    Setting = item
                };
                settingItems.Add(settingItem);
            }

            return settingItems;
        }

        public SettingItem GetPayGateSetting(Guid applicationId, string keyName)
        {
            string prefixPattern = "PAYGATE";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return null;

            var settings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(aplicationSetting.SettingValue).ToList();
            if (!settings.Any()) return null;

            var setting = settings.FirstOrDefault(x => x.KeyName.ToLower() == keyName.ToLower());
            if (setting == null) return null;

            var settingItem = new SettingItem
            {
                ApplicationId = aplicationSetting.ApplicationId,
                SettingId = aplicationSetting.SettingId,
                SettingName = aplicationSetting.SettingName,
                SettingValue = aplicationSetting.SettingValue,
                IsSecured = aplicationSetting.IsSecured,
                IsActive = aplicationSetting.IsActive,
                Setting = setting
            };

            return settingItem;
        }

        #endregion

        #region PAYMENT - PAPAL

        /// <summary>
        /// Get Paypal Settings
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public IEnumerable<ApplicationSetting> GetPaypalSettings(Guid applicationId)
        {
            string prefixPattern = "PAYPAL_";
            var settings = DataContext
                .Get<ApplicationSetting>().Where(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            return settings.AsEnumerable();
        }

        /// <summary>
        /// Paypal Informaiton
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public PaypalSetting GetActivePaypalSetting(Guid applicationId)
        {
            string prefixPattern = "PAYPAL_";
            var setting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.IsActive == ApplicationSettingStatus.Active && x.SettingName.Contains(prefixPattern));
            if (setting == null) return null;

            string settingValue = setting.SettingValue;

            var paymentSetting = JsonConvert.DeserializeObject<PaypalSetting>(settingValue);
            paymentSetting.ApplicationId = setting.ApplicationId;
            paymentSetting.SettingId = setting.SettingId;
            paymentSetting.SettingName = setting.SettingName;
            paymentSetting.SettingValue = settingValue;
            paymentSetting.IsSecured = setting.IsSecured;
            paymentSetting.IsActive = setting.IsActive;

            return paymentSetting;
        }

        /// <summary>
        /// Paypal Informaiton
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public PaypalSetting GetPaypalSetting(Guid applicationId, string mode)
        {
            string modeName = !string.IsNullOrEmpty(mode) ? mode.ToUpper() : string.Empty;
            string prefixPattern = "PAYPAL_" + modeName;
            var setting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (setting == null) return null;

            string settingValue = setting.SettingValue;

            var paymentSetting = JsonConvert.DeserializeObject<PaypalSetting>(settingValue);
            paymentSetting.ApplicationId = setting.ApplicationId;
            paymentSetting.SettingId = setting.SettingId;
            paymentSetting.SettingName = setting.SettingName;
            paymentSetting.IsSecured = setting.IsSecured;
            paymentSetting.IsActive = setting.IsActive;

            return paymentSetting;
        }

        #endregion

        #region PAYMENT - STRIPE

        public IEnumerable<BaseSetting> GetActiveStripeSettings(Guid applicationId)
        {
            string prefixPattern = "STRIPE_";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.IsActive == ApplicationSettingStatus.Active && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return null;

            var settings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(aplicationSetting.SettingValue).ToList();
            return settings;
        }
        
        public List<SettingItem> GetStripeSettings(Guid applicationId, string mode)
        { 
            string prefixPattern = $"STRIPE_{mode.ToUpper()}";
            var setting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.ToUpper() == prefixPattern);
            if (setting == null) return null;

            var lst = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(setting.SettingValue).ToList();
            if (!lst.Any()) return null;

            var settingItems = new List<SettingItem>();
            foreach (var item in lst)
            {
                var settingItem = new SettingItem
                {
                    ApplicationId = setting.ApplicationId,
                    SettingId = setting.SettingId,
                    SettingName = setting.SettingName,
                    SettingValue = setting.SettingValue,
                    IsSecured = setting.IsSecured,
                    IsActive = setting.IsActive,
                    Setting = item
                };
                settingItems.Add(settingItem);
            }

            return settingItems;
        }

        public SettingItem GetStripeSetting(Guid applicationId, string mode, string keyName)
        {
            string prefixPattern = $"STRIPE_{mode.ToUpper()}";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return null;

            var settings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(aplicationSetting.SettingValue).ToList();
            if (!settings.Any()) return null;

            var setting = settings.FirstOrDefault(x => x.KeyName.ToLower() == keyName.ToLower());
            if (setting == null) return null;

            var settingItem = new SettingItem
            {
                ApplicationId = aplicationSetting.ApplicationId,
                SettingId = aplicationSetting.SettingId,
                SettingName = aplicationSetting.SettingName,
                SettingValue = aplicationSetting.SettingValue,
                IsSecured = aplicationSetting.IsSecured,
                IsActive = aplicationSetting.IsActive,
                Setting = setting
            };

            return settingItem;
        }
        
        public SettingItem GetActiveStripeSetting(Guid applicationId, string keyName)
        {
            string prefixPattern = "STRIPE_";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.IsActive == ApplicationSettingStatus.Active && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return null;

            var settings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(aplicationSetting.SettingValue).ToList();
            if (!settings.Any()) return null;

            var setting = settings.FirstOrDefault(x => x.KeyName.ToLower() == keyName.ToLower());
            if (setting == null) return null;

            var settingItem = new SettingItem
            {
                ApplicationId = aplicationSetting.ApplicationId,
                SettingId = aplicationSetting.SettingId,
                SettingName = aplicationSetting.SettingName,
                SettingValue = aplicationSetting.SettingValue,
                IsSecured = aplicationSetting.IsSecured,
                IsActive = aplicationSetting.IsActive,
                Setting = setting
            };

            return settingItem;
        }

        #endregion

        #region TRANSACTION

        public List<SettingItem> GetTransactionSettings(Guid applicationId)
        {
            string prefixPattern = "TRANSACTION";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.ToUpper() == prefixPattern);
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return null;

            var settings = new List<SettingItem>();
            foreach (var item in baseSettings)
            {
                var setting = new SettingItem
                {
                    ApplicationId = aplicationSetting.ApplicationId,
                    SettingId = aplicationSetting.SettingId,
                    SettingName = aplicationSetting.SettingName,
                    SettingValue = aplicationSetting.SettingValue,
                    IsSecured = aplicationSetting.IsSecured,
                    IsActive = aplicationSetting.IsActive,
                    Setting = item
                };
                settings.Add(setting);
            }

            return settings;
        }

        public SettingItem GetTransactionSetting(Guid applicationId, string keyName)
        {
            string prefixPattern = "TRANSACTION";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return null;

            var baseSetting = baseSettings.FirstOrDefault(x => x.KeyName.ToLower() == keyName.ToLower());
            if (baseSetting == null) return null;

            var setting = new SettingItem
            {
                ApplicationId = aplicationSetting.ApplicationId,
                SettingId = aplicationSetting.SettingId,
                SettingName = aplicationSetting.SettingName,
                SettingValue = aplicationSetting.SettingValue,
                IsSecured = aplicationSetting.IsSecured,
                IsActive = aplicationSetting.IsActive,
                Setting = baseSetting
            };

            return setting;
        }

        public int GetSelectedPaymentMethod(Guid applicationId)
        {
            string prefixPattern = "TRANSACTION";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return GlobalSettings.DefaultPaymentMethod;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return GlobalSettings.DefaultPaymentMethod;

            var baseSetting = baseSettings.FirstOrDefault(x => string.Equals(x.KeyName, TransactionConfig.PaymentMethod, StringComparison.CurrentCultureIgnoreCase));
            if (baseSetting == null) return GlobalSettings.DefaultPaymentMethod;

            int paymentMethodId = Convert.ToInt32(baseSetting.KeyValue);
            return paymentMethodId;
        }

        #endregion

        #region Rate Settings

        /// <summary>
        /// Get Rate Settings
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public List<SettingItem> GetRateSettings(Guid applicationId)
        {
            string prefixPattern = "RATING";
            var setting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.ToUpper() == prefixPattern);
            if (setting == null) return null;
            
            var lst = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(setting.SettingValue).ToList();
            if (!lst.Any()) return null;

            var settingItems = new List<SettingItem>();
            foreach (var item in lst)
            {
                var settingItem = new SettingItem
                {
                    ApplicationId = setting.ApplicationId,
                    SettingId = setting.SettingId,
                    SettingName = setting.SettingName,
                    SettingValue = setting.SettingValue,
                    IsSecured = setting.IsSecured,
                    IsActive = setting.IsActive,
                    Setting = item
                };
                settingItems.Add(settingItem);
            }
           
            return settingItems;
        }

        public SettingItem GetRateSetting(Guid applicationId, string keyName)
        {
            string prefixPattern = "RATING";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return null;
            
            var settings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(aplicationSetting.SettingValue).ToList();
            if (!settings.Any()) return null;

            var setting = settings.FirstOrDefault(x => x.KeyName.ToLower() == keyName.ToLower());
            if (setting == null) return null;

            var settingItem = new SettingItem
            {
                ApplicationId = aplicationSetting.ApplicationId,
                SettingId = aplicationSetting.SettingId,
                SettingName = aplicationSetting.SettingName,
                SettingValue = aplicationSetting.SettingValue,
                IsSecured = aplicationSetting.IsSecured,
                IsActive = aplicationSetting.IsActive,
                Setting = setting
            };

            return settingItem;
        }

        #endregion

        #region SMTP

        public List<SettingItem> GetSmtpSettings(Guid applicationId)
        {
            string prefixPattern = "SMTP";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.ToUpper() == prefixPattern);
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var emailSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!emailSettings.Any()) return null;

            var settingItems = new List<SettingItem>();
            foreach (var item in emailSettings)
            {
                var settingItem = new SettingItem
                {
                    ApplicationId = aplicationSetting.ApplicationId,
                    SettingId = aplicationSetting.SettingId,
                    SettingName = aplicationSetting.SettingName,
                    SettingValue = aplicationSetting.SettingValue,
                    IsSecured = aplicationSetting.IsSecured,
                    IsActive = aplicationSetting.IsActive,
                    Setting = item
                };
                settingItems.Add(settingItem);
            }

            return settingItems;
        }

        public SettingItem GetSmtpSetting(Guid applicationId, string keyName)
        {
            string prefixPattern = "SMTP";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var settings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!settings.Any()) return null;

            var setting = settings.FirstOrDefault(x => x.KeyName.ToLower() == keyName.ToLower());
            if (setting == null) return null;

            var smtpSetting = new SettingItem
            {
                ApplicationId = aplicationSetting.ApplicationId,
                SettingId = aplicationSetting.SettingId,
                SettingName = aplicationSetting.SettingName,
                SettingValue = aplicationSetting.SettingValue,
                IsSecured = aplicationSetting.IsSecured,
                IsActive = aplicationSetting.IsActive,
                Setting = setting
            };

            return smtpSetting;
        }

        #endregion

        #region GOOGLE RECAPTCHA

        public List<SettingItem> GetRecaptchaSettings(Guid applicationId)
        {
            string prefixPattern = "GOOGLE_RECAPTCHA";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.ToUpper() == prefixPattern);
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return null;

            var settings = new List<SettingItem>();
            foreach (var item in baseSettings)
            {
                var setting = new SettingItem
                {
                    ApplicationId = aplicationSetting.ApplicationId,
                    SettingId = aplicationSetting.SettingId,
                    SettingName = aplicationSetting.SettingName,
                    SettingValue = aplicationSetting.SettingValue,
                    IsSecured = aplicationSetting.IsSecured,
                    IsActive = aplicationSetting.IsActive,
                    Setting = item
                };
                settings.Add(setting);
            }

            return settings;
        }

        public SettingItem GetRecaptchaSetting(Guid applicationId, string keyName)
        {
            string prefixPattern = "GOOGLE_RECAPTCHA";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return null;

            var baseSetting = baseSettings.FirstOrDefault(x => x.KeyName.ToLower() == keyName.ToLower());
            if (baseSetting == null) return null;

            var setting = new SettingItem
            {
                ApplicationId = aplicationSetting.ApplicationId,
                SettingId = aplicationSetting.SettingId,
                SettingName = aplicationSetting.SettingName,
                SettingValue = aplicationSetting.SettingValue,
                IsSecured = aplicationSetting.IsSecured,
                IsActive = aplicationSetting.IsActive,
                Setting = baseSetting
            };

            return setting;
        }

        #endregion

        #region TWITTER

        public List<SettingItem> GetTwitterSettings(Guid applicationId)
        {
            string prefixPattern = "TWITTER";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.ToUpper() == prefixPattern);
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return null;

            var settings = new List<SettingItem>();
            foreach (var item in baseSettings)
            {
                var setting = new SettingItem
                {
                    ApplicationId = aplicationSetting.ApplicationId,
                    SettingId = aplicationSetting.SettingId,
                    SettingName = aplicationSetting.SettingName,
                    SettingValue = aplicationSetting.SettingValue,
                    IsSecured = aplicationSetting.IsSecured,
                    IsActive = aplicationSetting.IsActive,
                    Setting = item
                };
                settings.Add(setting);
            }

            return settings;
        }

        public SettingItem GetTwitterSetting(Guid applicationId, string keyName)
        {
            string prefixPattern = "TWITTER";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return null;

            var baseSetting = baseSettings.FirstOrDefault(x => x.KeyName.ToLower() == keyName.ToLower());
            if (baseSetting == null) return null;

            var setting = new SettingItem
            {
                ApplicationId = aplicationSetting.ApplicationId,
                SettingId = aplicationSetting.SettingId,
                SettingName = aplicationSetting.SettingName,
                SettingValue = aplicationSetting.SettingValue,
                IsSecured = aplicationSetting.IsSecured,
                IsActive = aplicationSetting.IsActive,
                Setting = baseSetting
            };

            return setting;
        }

        #endregion

        #region FACEBOOK

        public List<SettingItem> GetFacebookSettings(Guid applicationId)
        {
            string prefixPattern = "FACEBOOK";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.ToUpper() == prefixPattern);
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return null;

            var settings = new List<SettingItem>();
            foreach (var item in baseSettings)
            {
                var setting = new SettingItem
                {
                    ApplicationId = aplicationSetting.ApplicationId,
                    SettingId = aplicationSetting.SettingId,
                    SettingName = aplicationSetting.SettingName,
                    SettingValue = aplicationSetting.SettingValue,
                    IsSecured = aplicationSetting.IsSecured,
                    IsActive = aplicationSetting.IsActive,
                    Setting = item
                };
                settings.Add(setting);
            }

            return settings;
        }

        public SettingItem GetFacebookSetting(Guid applicationId, string keyName)
        {
            string prefixPattern = "FACEBOOK";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return null;

            var baseSetting = baseSettings.FirstOrDefault(x => x.KeyName.ToLower() == keyName.ToLower());
            if (baseSetting == null) return null;

            var setting = new SettingItem
            {
                ApplicationId = aplicationSetting.ApplicationId,
                SettingId = aplicationSetting.SettingId,
                SettingName = aplicationSetting.SettingName,
                SettingValue = aplicationSetting.SettingValue,
                IsSecured = aplicationSetting.IsSecured,
                IsActive = aplicationSetting.IsActive,
                Setting = baseSetting
            };

            return setting;
        }

        #endregion

        #region PRODUCT FILTERED PRICE RANGE SETTING
        public List<SettingItem> GetProductFilteredPriceRangeSettings(Guid applicationId)
        {
            string prefixPattern = "PRODUCT_FILTERED_PRICE_RANGE";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.ToUpper() == prefixPattern);
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return null;

            var settings = new List<SettingItem>();
            foreach (var item in baseSettings)
            {
                var setting = new SettingItem
                {
                    ApplicationId = aplicationSetting.ApplicationId,
                    SettingId = aplicationSetting.SettingId,
                    SettingName = aplicationSetting.SettingName,
                    SettingValue = aplicationSetting.SettingValue,
                    IsSecured = aplicationSetting.IsSecured,
                    IsActive = aplicationSetting.IsActive,
                    Setting = item
                };
                settings.Add(setting);
            }

            return settings;
        }

        public SettingItem GetProductFilteredPriceRangeSetting(Guid applicationId, string keyName)
        {
            string prefixPattern = "PRODUCT_FILTERED_PRICE_RANGE";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return null;

            var setting = baseSettings.FirstOrDefault(x => x.KeyName.ToLower() == keyName.ToLower());
            if (setting == null) return null;

            var itemSetting = new SettingItem
            {
                ApplicationId = aplicationSetting.ApplicationId,
                SettingId = aplicationSetting.SettingId,
                SettingName = aplicationSetting.SettingName,
                SettingValue = aplicationSetting.SettingValue,
                IsSecured = aplicationSetting.IsSecured,
                IsActive = aplicationSetting.IsActive,
                Setting = setting
            };

            return itemSetting;
        }
        #endregion

        #region PRODUCT PRICE RANGE SETTING
        public List<SettingItem> GetProductPriceRangeSettings(Guid applicationId)
        {
            string prefixPattern = "PRODUCT_PRICE_RANGE";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.ToUpper() == prefixPattern);
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return null;

            var settings = new List<SettingItem>();
            foreach (var item in baseSettings)
            {
                var setting = new SettingItem
                {
                    ApplicationId = aplicationSetting.ApplicationId,
                    SettingId = aplicationSetting.SettingId,
                    SettingName = aplicationSetting.SettingName,
                    SettingValue = aplicationSetting.SettingValue,
                    IsSecured = aplicationSetting.IsSecured,
                    IsActive = aplicationSetting.IsActive,
                    Setting = item
                };
                settings.Add(setting);
            }

            return settings;
        }

        public SettingItem GetProductPriceRangeSetting(Guid applicationId, string keyName)
        {
           string prefixPattern = "PRODUCT_PRICE_RANGE";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var baseSettings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!baseSettings.Any()) return null;

            var setting = baseSettings.FirstOrDefault(x => x.KeyName.ToLower() == keyName.ToLower());
            if (setting == null) return null;

            var itemSetting = new SettingItem
            {
                ApplicationId = aplicationSetting.ApplicationId,
                SettingId = aplicationSetting.SettingId,
                SettingName = aplicationSetting.SettingName,
                SettingValue = aplicationSetting.SettingValue,
                IsSecured = aplicationSetting.IsSecured,
                IsActive = aplicationSetting.IsActive,
                Setting = setting
            };

            return itemSetting;
        }
        #endregion

        #region LANGUAGE

        public List<SettingItem> GetLanguageSettings(Guid applicationId)
        {
            string prefixPattern = "LANGUAGE";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.ToUpper() == prefixPattern);
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var settingLst = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!settingLst.Any()) return null;

            var settings = new List<SettingItem>();
            foreach (var item in settingLst)
            {
                var currentSetting = new SettingItem
                {
                    ApplicationId = aplicationSetting.ApplicationId,
                    SettingId = aplicationSetting.SettingId,
                    SettingName = aplicationSetting.SettingName,
                    SettingValue = aplicationSetting.SettingValue,
                    IsSecured = aplicationSetting.IsSecured,
                    IsActive = aplicationSetting.IsActive,
                    Setting = item
                };
                settings.Add(currentSetting);
            }

            return settings;
        }

        public SettingItem GetLanguageSetting(Guid applicationId, string keyName)
        {
            string prefixPattern = "LANGUAGE";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var settings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!settings.Any()) return null;

            var setting = settings.FirstOrDefault(x => x.KeyName.ToLower() == keyName.ToLower());
            if (setting == null) return null;

            var currentSetting = new SettingItem
            {
                ApplicationId = aplicationSetting.ApplicationId,
                SettingId = aplicationSetting.SettingId,
                SettingName = aplicationSetting.SettingName,
                SettingValue = aplicationSetting.SettingValue,
                IsSecured = aplicationSetting.IsSecured,
                IsActive = aplicationSetting.IsActive,
                Setting = setting
            };

            return currentSetting;
        }

        #endregion
        
        #region CURRENCY

        public List<SettingItem> GetCurrencySettings(Guid applicationId)
        {
            string prefixPattern = "CURRENCY";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.ToUpper() == prefixPattern);
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var settingLst = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!settingLst.Any()) return null;

            var settings = new List<SettingItem>();
            foreach (var item in settingLst)
            {
                var currentSetting = new SettingItem
                {
                    ApplicationId = aplicationSetting.ApplicationId,
                    SettingId = aplicationSetting.SettingId,
                    SettingName = aplicationSetting.SettingName,
                    SettingValue = aplicationSetting.SettingValue,
                    IsSecured = aplicationSetting.IsSecured,
                    IsActive = aplicationSetting.IsActive,
                    Setting = item
                };
                settings.Add(currentSetting);
            }

            return settings;
        }

        public SettingItem GetCurrencySetting(Guid applicationId, string keyName)
        {
            string prefixPattern = "CURRENCY";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var settings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!settings.Any()) return null;

            var setting = settings.FirstOrDefault(x => x.KeyName.ToLower() == keyName.ToLower());
            if (setting == null) return null;

            var currentSetting = new SettingItem
            {
                ApplicationId = aplicationSetting.ApplicationId,
                SettingId = aplicationSetting.SettingId,
                SettingName = aplicationSetting.SettingName,
                SettingValue = aplicationSetting.SettingValue,
                IsSecured = aplicationSetting.IsSecured,
                IsActive = aplicationSetting.IsActive,
                Setting = setting
            };

            return currentSetting;
        }

        #endregion

        #region ADDRESS

        public List<SettingItem> GetAddressSettings(Guid applicationId)
        {
            string prefixPattern = "ADDRESS";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.ToUpper() == prefixPattern);
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var settingLst = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!settingLst.Any()) return null;

            var settings = new List<SettingItem>();
            foreach (var item in settingLst)
            {
                var currentSetting = new SettingItem
                {
                    ApplicationId = aplicationSetting.ApplicationId,
                    SettingId = aplicationSetting.SettingId,
                    SettingName = aplicationSetting.SettingName,
                    SettingValue = aplicationSetting.SettingValue,
                    IsSecured = aplicationSetting.IsSecured,
                    IsActive = aplicationSetting.IsActive,
                    Setting = item
                };
                settings.Add(currentSetting);
            }

            return settings;
        }

        public SettingItem GetAddressSetting(Guid applicationId, string keyName)
        {
            string prefixPattern = "ADDRESS";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var settings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!settings.Any()) return null;

            var setting = settings.FirstOrDefault(x => x.KeyName.ToLower() == keyName.ToLower());
            if (setting == null) return null;

            var currentSetting = new SettingItem
            {
                ApplicationId = aplicationSetting.ApplicationId,
                SettingId = aplicationSetting.SettingId,
                SettingName = aplicationSetting.SettingName,
                SettingValue = aplicationSetting.SettingValue,
                IsSecured = aplicationSetting.IsSecured,
                IsActive = aplicationSetting.IsActive,
                Setting = setting
            };

            return currentSetting;
        }

        #endregion

        #region DELIVERY - SHIPPING

        public List<SettingItem> GetDeliverySettings(Guid applicationId)
        {
            string prefixPattern = "DELIVERY";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.ToUpper() == prefixPattern);
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var settingLst = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!settingLst.Any()) return null;

            var settings = new List<SettingItem>();
            foreach (var item in settingLst)
            {
                var currentSetting = new SettingItem
                {
                    ApplicationId = aplicationSetting.ApplicationId,
                    SettingId = aplicationSetting.SettingId,
                    SettingName = aplicationSetting.SettingName,
                    SettingValue = aplicationSetting.SettingValue,
                    IsSecured = aplicationSetting.IsSecured,
                    IsActive = aplicationSetting.IsActive,
                    Setting = item
                };
                settings.Add(currentSetting);
            }

            return settings;
        }

        public SettingItem GetDeliverySetting(Guid applicationId, string keyName)
        {
            string prefixPattern = "DELIVERY";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var settings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!settings.Any()) return null;

            var setting = settings.FirstOrDefault(x => x.KeyName.ToLower() == keyName.ToLower());
            if (setting == null) return null;

            var currentSetting = new SettingItem
            {
                ApplicationId = aplicationSetting.ApplicationId,
                SettingId = aplicationSetting.SettingId,
                SettingName = aplicationSetting.SettingName,
                SettingValue = aplicationSetting.SettingValue,
                IsSecured = aplicationSetting.IsSecured,
                IsActive = aplicationSetting.IsActive,
                Setting = setting
            };

            return currentSetting;
        }

        #endregion

        #region NOTIFICATION

        public List<SettingItem> GetNotificationSettings(Guid applicationId)
        {
            string prefixPattern = "NOTIFICATION";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.ToUpper() == prefixPattern);
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var settingLst = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!settingLst.Any()) return null;

            var settings = new List<SettingItem>();
            foreach (var item in settingLst)
            {
                var currentSetting = new SettingItem
                {
                    ApplicationId = aplicationSetting.ApplicationId,
                    SettingId = aplicationSetting.SettingId,
                    SettingName = aplicationSetting.SettingName,
                    SettingValue = aplicationSetting.SettingValue,
                    IsSecured = aplicationSetting.IsSecured,
                    IsActive = aplicationSetting.IsActive,
                    Setting = item
                };
                settings.Add(currentSetting);
            }

            return settings;
        }

        public SettingItem GetNotificationSetting(Guid applicationId, string keyName)
        {
            string prefixPattern = "NOTIFICATION";
            var aplicationSetting = DataContext
                .Get<ApplicationSetting>().FirstOrDefault(x => x.ApplicationId == applicationId && x.SettingName.Contains(prefixPattern));
            if (aplicationSetting == null) return null;

            string settingValue = aplicationSetting.SettingValue;

            var settings = JsonConvert.DeserializeObject<IEnumerable<BaseSetting>>(settingValue).ToList();
            if (!settings.Any()) return null;

            var setting = settings.FirstOrDefault(x => x.KeyName.ToLower() == keyName.ToLower());
            if (setting == null) return null;

            var currentSetting = new SettingItem
            {
                ApplicationId = aplicationSetting.ApplicationId,
                SettingId = aplicationSetting.SettingId,
                SettingName = aplicationSetting.SettingName,
                SettingValue = aplicationSetting.SettingValue,
                IsSecured = aplicationSetting.IsSecured,
                IsActive = aplicationSetting.IsActive,
                Setting = setting
            };

            return currentSetting;
        }

        #endregion
    }
}
