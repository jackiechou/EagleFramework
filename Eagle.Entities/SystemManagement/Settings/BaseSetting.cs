using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement.Settings
{
    public class SettingMode
    {
        public string Sandbox = "sandbox";
        public string Live = "live";
    }

    [NotMapped]
    public class BaseSetting : EntityBase
    {
        public string KeyName { get; set; }
        public string KeyValue { get; set; }
    }

    [NotMapped]
    public class SettingItem : ApplicationSetting
    {
        public BaseSetting Setting { get; set; }
    }

    #region Payal Settings

    [NotMapped]
    public class PaypalSetting : ApplicationSetting
    {
        public Dictionary<string, string> Config => new Dictionary<string, string>
        {
            {"model", Mode},
            {"appId", AppId},
            {"clientId", ClientId},
            {"clientSecret", ClientSecret},
            {"connectionTimeout", ConnectionTimeout},
            {"requestRetries", RequestRetries}
        };

        public string AppId { get; set; }
        public string Mode { get; set; }
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string ConnectionTimeout { get; set; }

        public string RequestRetries { get; set; }

    }

    #endregion

    #region TRANSACTION

    public class TransactionConfig
    {
        public static string PaymentMethod = "PaymentMethod";
    }

    #endregion

    #region Rate Settings

    [NotMapped]
    public class Rating : EntityBase
    {
        public string RatingName { get; set; }
        public int RatingValue { get; set; }
    }

    [NotMapped]
    public class RateSetting : ApplicationSetting
    {
        public string RatingName { get; set; }
        public int RatingValue { get; set; }
    }

    #endregion

    #region Notification

    [NotMapped]
    public class SmtpSetting : ApplicationSetting
    {
        public BaseSetting Setting { get; set; }
    }

    #endregion
    
    #region GOOGLE RECAPTCHA

    [NotMapped]
    public class RecaptchaSetting : ApplicationSetting
    {
        public BaseSetting Setting { get; set; }
    }

    #endregion

    public class FileConfig
    {
        public static string FileExtensions = "FileExtensions";
        public static string ImageExtensions = "ImageExtensions";
        public static string VideoExtensions = "VideoExtensions";
        public static string DocumentExtensions = "DocumentExtensions";
        public static string MaxImageContentLength = "MaxImageContentLength";
        public static string MaxVideoContentLength = "MaxVideoContentLength";
        public static string MaxDocumentContentLength = "MaxDocumentContentLength";
    }

    public class PageConfig
    {
        public static string PageSizeForDefault = "PageSizeForDefault";
        public static string PageSizeForNews = "PageSizeForNews";
        public static string PageSizeForEvent = "PageSizeForEvent";
    }

    public class DateTimeConfig
    {
        public static string DateTimeFormat = "DateTimeFormat";
        public static string DateFormat = "DateFormat";
        public static string TimeZone = "TimeZone";
    }
}
