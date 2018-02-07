using System;
using System.Web;
using Eagle.Common.Utilities;
using Eagle.Core.Settings;

namespace Eagle.Core.Configuration
{
    public static class GlobalSettings
    {
        #region COMMON
        public static Guid DefaultApplicationId = new Guid("c25a74cc-2e61-43ae-86c2-ce608c40b65a");
        public static Guid DefaultRoleId = new Guid("c3318a71-d9cc-4c92-a1ae-66e0eefde765");
        public static string DefaultUserId = "76119582-0be5-4400-b9ee-2a94ea27dd99";
        public static int DefaultVendorId = 1;
        public static bool DefaultIsSuperUser = false;
        public static string DefaultPageUrl = "/Admin/Dashboard/Index";
        public static string DefaultDesktopPageUrl = "/Index";
        public static int DefaultCompanyId = 1;
        public static int DefaultFrontCompanyId = 2;

        //DateTime
        public static string DefaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss"; //Save to Db
        public static string DefaultDateFormat = "dd/MM/yyyy";
        public static string DefaultTimeZone = "SE Asia Standard Time";
        public static DateTime MinDateTime = new DateTime(1900, 01, 01, 00, 00, 00);
        public static DateTime MaxDateTime = new DateTime(2079, 06, 06, 23, 59, 00);

        //Cache
        public static int DefaultCacheDuration = 20; //20 minutes
        public static DateTime AbsoluteExpiration = DateTime.UtcNow.AddDays(1);

        public static int DefaultLanguageId = 124; //LanguageId = 41; =>en_US or LanguageId=124 => vi-VN
        public static string DefaultLanguageCode = LanguageSetting.Vietnamese;
        public static int DefaultPageSize = 10;
        public static int DefaultPageSizeByLastest = 3;
        public static int DefaultPageSizeEvent = 1;
        public static int DefaultPageId = 1;
        public static int DefaultCountryId = 232; //viet nam
        public static int DefaultProvinceId = 24;
        public static string DefaultIp = NetworkUtils.GetIP4Address();
        public static string DefaultCurrencyCode = "USD";
        public const string ApplicationName = "EAGLE";
        public const string ResourceName = "EAGLE";

        public static string DefaultIconClass = "glyphicon-picture";
        //public static string DefaultImageFilter = "*.gif,*.png,*.jpg,*.jpeg,*.bmp,*.tiff";
        //public static string DefaultFileFilter = "*.gif,*.png,*.jpg,*.jpeg,*.bmp,*.tiff,*.doc,*.docx,*.ppt,*.pptx,*.csv,*.xls,*.xlsx,*.pdf,*.odt,*.txt,*.zip,*.rar,*.7z";
        //public static string DefaultVideoFilter = "*.flv,*.mpg,*.avi,*.3gp,*.mov,*.mp4";
        //public static string DefaultDocumentFileFilter = "*.doc,*.docx,*.ppt,*.pptx,*.csv,*.xls,*.xlsx,*.pdf,*.odt, *.txt, *.zip, *.rar, *.7z";
        //public static string[] DefaultFileTypes = { ".bmp", ".gif", ".png", ".jpg", ".jpeg", ".tiff", ".swf" };
        //public static string[] DefaultValidImageTypes = { "image/gif","image/jpeg", "image/pjpeg","image/png","image/bmp"};
        //public static string[] ImageExtensions = { ".gif", ".jpeg", ".jpg", ".pjpeg", ".png", ".bmp" };

        public static string NotFoundFileUrl = "/Images/no-image.png";
        public static string NotFoundImageUrl = "/Images/no-image.png";
        public static string NotFoundVideoUrl = "/Images/no-video.png";
        public static string DefaultFileUrl = "/Images/no-image.png";
        public static string DefaultImageUrl = "/Images/no-image.png";
        public static string DefaultImageFolderPath = "/Images";
        public static int DefaultImagelHeight = ImageSettings.ImageWidthVga;
        public static int DefaultImageWidth = ImageSettings.ImageHeightVga;
        public static int DefaultThumbnailHeight = ImageSettings.ImageWidthPalm;
        public static int DefaultThumbnailWidth = ImageSettings.ImageHeightPalm;
        public static int MaxImageContentLength = 1024 * 1024 * 15; //1024 * 1024 * 15 = 15 MB
        public static int MaxVideoContentLength = 1024 * 1024 * 150; //1024 * 1024 * 15 = 150 MB
        public static int MaxDocumentContentLength = 1024 * 1024 * 150; //1024 * 1024 * 15 = 150 MB

        //Transaction
        public static int DefaultPaymentMethod = 1;

        public const string ALLOWED_ORIGIN = "*";
        public const string ACCESS_CONTROL_ALLOW_ORGIN = "Access-Control-Allow-Origin";
        public const string ERR_INVALID_GRANT = "invalid_grant";
        public const string ERR_USERNAME_PASS_INCORRECT = "User name or password is incorrect!";

        #endregion

        #region HOST SETTING CONFIG
        public const string HostCurrency = "HostCurrency";
        public const string HostEmail = "HostEmail";
        public const string HostFee = "HostFee";
        public const string HostPortalId = "HostPortalId";
        public const string HostSpace = "HostSpace";
        public const string HostTitle = "HostTitle";
        public const string HostUrl = "HostURL";
        public const string PageQuota = "PageQuota";
        public const string PasswordExpiry = "PasswordExpiry";
        public const string PasswordExpiryReminder = "PasswordExpiryReminder";
        public const string UseFriendlyUrls = "UseFriendlyUrls";
        public const string UseCustomErrorMessages = "UseCustomErrorMessages";
        public const string UserQuota = "UserQuota";
        public const string UsersOnlineTimeWindow = "UsersOnlineTime";
        public const string WebRequestTimeout = "WebRequestTimeout";
        public const string WhitespaceFilter = "WhitespaceFilter";
        public const string ExensionImage = "ExensionImage";
        public const string ExtensionVideo = "ExtensionVideo";
        public const string ExtensionFileUpload = "ExtensionFileUpload";
        #endregion

        #region UPLOADS - IMAGE - DOCUMENTS- MEDIA

        public static string FolderRoot = "~/";
        public static string ContentFolderRoot = "Content";
        public static string UploadFolderRoot = "Uploads";

        public static string UploadImageFolder = "Images";
        public static string UploadBannerFolder = "Banners";
        public static string UploadCompanyFolder = "Company";
        public static string UploadUserFolder = "Users";
        public static string UploadVendorFolder = "Vendors";
        public static string UploadNewsFolder = "News";
        public static string UploadEventFolder ="Events";
        public static string UploadProductFolder = "Products";

        public static string UploadMediaFolder = "Media";
        public static string UploadAudioFolder = "Audio";
        public static string UploadBackgroundAudioFolder = "BackgroundAudio";
        public static string UploadMusicAudioFolder = "Music";
        public static string UploadVideoFolder = "Video";
        public static string UploadVideoThumbnailFolder = "Thumbnail";
        public static string UploadServicePackFolder = "ServicePack";

        public static string UploadDocumentFolder = "Documents";
       
        public static string UploadImagePath => $"/{UploadFolderRoot}/{UploadImageFolder}";
        public static string BannerUploadImagePath => $"{UploadImagePath}/{UploadBannerFolder}";
        public static string CompanyUploadImagePath => $"{UploadImagePath}/{UploadCompanyFolder}";
        public static string UserUploadImagePath => $"{UploadImagePath}/{UploadUserFolder}";
        public static string VendorUploadImagePath => $"{UploadImagePath}/{UploadVendorFolder}";
        public static string NewsUploadImagePath => $"{UploadImagePath}/{UploadNewsFolder}";
        public static string EventUploadImagePath => $"{UploadImagePath}/{UploadEventFolder}";
        public static string ProductUploadImagePath => $"{UploadImagePath}/{UploadProductFolder}";
        public static string ServicePackUploadImagePath => $"{UploadImagePath}/{UploadServicePackFolder}";

        public static string UploadDocumentPath => $"/{UploadFolderRoot}/{UploadDocumentFolder}";
        public static string CompanyUploadDocumentPath => $"{UploadDocumentPath}/{UploadCompanyFolder}";
        public static string NewsUploadDocumentPath => $"{UploadDocumentPath}/{UploadNewsFolder}";
        public static string EventUploadDocumentPath => $"{UploadDocumentPath}/{UploadEventFolder}";
        public static string ProductUploadDocumentPath => $"{UploadDocumentPath}/{UploadProductFolder}";
        public static string ServicePackUploadDocumentPath => $"{UploadDocumentPath}/{UploadServicePackFolder}";

        public static string MediaUploadPath => $"/{UploadFolderRoot}/{UploadMediaFolder}";
        public static string AudioUploadPath => $"{MediaUploadPath}/{UploadAudioFolder}";
        public static string BackgroundAudioUploadPath => $"{AudioUploadPath}/{ UploadBackgroundAudioFolder}";
        public static string MusicAudioUploadImagePath => $"{AudioUploadPath}/{ UploadMusicAudioFolder}";
        public static string VideoUploadPath => $"{MediaUploadPath}/{UploadVideoFolder}";
        public static string VideoThumbnailUploadPath => $"{VideoUploadPath}/{ UploadVideoThumbnailFolder}";

        #endregion

        #region Theme 

        //Default Theme 
        public static string DefaultThemeName = "Default";
        public static string DefaultThemeSrc = "~/Themes/Default";
        public static string ThemeName
        {
            get
            {
                if (HttpContext.Current.Session[SettingKeys.ThemeName] != null)
                    DefaultThemeName = HttpContext.Current.Session[SettingKeys.ThemeName].ToString();
                return DefaultThemeName;
            }
            set => HttpContext.Current.Session[SettingKeys.ThemeName] = value;
        }


        #endregion      
        
    }
    
    public class AppType
    {
        public const string Automation = "Automation";
        public const string Manual = "Manual";
    }

    public class FileType
    {
        public const string Normal = "Normal";
        public const string Compress = "Compress";
        public const string Decompress = "Decompress";
    }

    public class FtpFileStatusConst
    {
        public const string UnReady = "UNREADY";
        public const string Ready = "READY";
        public const string Processing = "PROCESSING";
        public const string Copied = "COPIED";
        public const string Success = "SUCCESS";
        public const string Error = "ERROR";
    }

}
