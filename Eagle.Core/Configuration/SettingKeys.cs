#region "Copyright"
/*
5eagles® - http://www.5eagles.com.vn
Copyright (c) 2009-2012 by 5EAGLES
Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
#endregion

namespace Eagle.Core.Configuration
{
    public static class SettingKeys
    {
        #region APPLICATION SETTING KEYS - Mapping Application Setting Table

        public static string CurrencySettingName = "CURRENCY";
        
        #endregion

        #region Session Key Settings ====================================       
        public static string DomainUrl = "DomainUrl";
        public static string DesiredUrl = "DesiredUrl";
        public static string SiteMap = "SiteMap";

        public static string ApplicationId = "ApplicationId";
        public static string ApplicationInfo = "ApplicationInfo";

        public static string MenuTypeCode = "MenuTypeCode";
        public static string MenuList = "MenuList";
        public static string PageList = "PageList";
        public static string ModuleList = "ModuleList";
        public static string FunctionList = "FunctionList";
        public static string RoleList = "RoleList";

        public static string DesktopMenuList = "DesktopMenuList";

        public static string DefaultLanguage = "DefaultLanguage";
        public static string CurrentLanguage = "CurrentLanguage";
        public static string LanguageCode = "LanguageCode";
        public static string LanguageId = "LanguageId";
        public static string IsLanguageSwitched = "IsLanguageSwitched";
        public static string CurrencyCode = "CurrencyCode";


        public static string RoleId = "RoleId";
        public static string RoleCode = "RoleCode";

        public static string UserId = "UserId";
        public static string UserCode = "UserCode";
        public static string UserName = "UserName";
        public static string Password = "Password";
        public static string DisplayName = "DisplayName";
        public static string FullName = "FullName";
        public static string IsSuperUser = "IsSuperUser";
        public static string ScopeTypeId = "ScopeTypeId";
        public static string UserProfile = "UserProfile";
        public static string Issuer = "Issuer";


        public static string EmpId = "EmpId";
        public static string EmpCode = "EmpCode";
        public static string EmpInfo = "EmpInfo";

        public static string AccountInfo = "AccountInfo";
        public static string UserInfo = "UserInfo";

        public static string VendorId = "VendorId";

        #endregion Session Key Settings ==================================

        public static string _ThemeName = "ThemeName";
        public static string _ThemeSrc = "ThemeSrc";
        public static string ThemeName
        {
            get { return _ThemeName; }
            set { _ThemeName = value; }
        }

        public static string ThemeSrc
        {
            get { return _ThemeSrc; }
            set { _ThemeSrc = value; }
        }


        public static string _MasterName = "MasterName";
        public static string _MasterSrc = "MasterSrc";

        public static string MasterName
        {
            get { return _MasterName; }
            set { _MasterName = value; }
        }

        public static string MasterSrc
        {
            get { return _MasterSrc; }
            set { _MasterSrc = value; }
        }

        public static string IconFile = "IconFile";
        public static string IsSecure = "IsSecure";
        public static string _MetaAuthor = "MetaAuthor";
        public static string MetaAuthor
        {
            get { return _MetaAuthor; }
            set { _MetaAuthor = value; }
        }

        public static string _MetaCopyright = "MetaCopyright";
        public static string MetaCopyright
        {
            get { return _MetaCopyright; }
            set { _MetaCopyright = value; }
        }

        public static string _MetaDescription = "MetaDescription";
        public static string MetaDescription
        {
            get { return _MetaDescription; }
            set { _MetaDescription = value; }
        }

        public static string MetaDistribution = "MetaDISTRIBUTION";
        public static string MetaDISTRIBUTION
        {
            get { return MetaDistribution; }
            set { MetaDistribution = value; }
        }

        public static string _MetaGenerator = "MetaGenerator";
        public static string MetaGenerator
        {
            get { return _MetaGenerator; }
            set { _MetaGenerator = value; }
        }

        public static string _MetaKeywords = "MetaKeywords";
        public static string MetaKeywords
        {
            get { return _MetaKeywords; }
            set { _MetaKeywords = value; }
        }

        public static string MetaPageEnter = "MetaPAGE_ENTER";
        public static string MetaPAGE_ENTER
        {
            get { return MetaPageEnter; }
            set { MetaPageEnter = value; }
        }

        public static string MetaRating = "MetaRATING";
        public static string MetaRATING
        {
            get { return MetaRating; }
            set { MetaRating = value; }
        }

        public static string _MetaRefresh = "MetaRefresh";
        public static string MetaRefresh
        {
            get { return _MetaRefresh; }
            set { _MetaRefresh = value; }
        }

        public static string MetaResourceType = "MetaRESOURCE_TYPE";
        public static string MetaRESOURCE_TYPE
        {
            get { return MetaResourceType; }
            set { MetaResourceType = value; }
        }

        public static string MetaRevisitAfter = "MetaREVISIT_AFTER";
        public static string MetaREVISIT_AFTER
        {
            get { return MetaRevisitAfter; }
            set { MetaRevisitAfter = value; }
        }

        public static string _MetaRobots = "MetaRobots";
        public static string MetaRobots
        {
            get { return _MetaRobots; }
            set { _MetaRobots = value; }
        }

        public static string _PageHeadText = "PageHeadText";
        public static string PageHeadText
        {
            get { return _PageHeadText; }
            set { _PageHeadText = value; }
        }

        public static string _PageTitle = "PageTitle";
        public static string PageTitle
        {
            get { return _PageTitle; }
            set { _PageTitle = value; }
        }

        public static string _EagleCss = "EagleCss";
        public static string EagleCss
        {
            get { return _EagleCss; }
            set { _EagleCss = value; }
        }

        public static string _AuthenticatedCacheability = "AuthenticatedCacheability";
        public static string AuthenticatedCacheability
        {
            get { return _AuthenticatedCacheability; }
            set { _AuthenticatedCacheability = value; }
        }

        public static string _AutoAccountUnlockDuration = "AutoAccountUnlockDuration";
        public static string AutoAccountUnlockDuration
        {
            get { return _AutoAccountUnlockDuration; }
            set { _AutoAccountUnlockDuration = value; }
        }

        public static string CheckUpgrade = "CheckUpgrade";
        public static string ContentLocalization = "ContentLocalization";
        public static string ControlPanel = "ControlPanel";
        public static string DefaultAdminContainer = "DefaultAdminContainer";
        public static string DefaultAdminTheme = "DefaultAdminTheme";
        public static string DefaultPortalContainer = "DefaultPortalContainer";
        public static string DefaultPortalTheme = "DefaultPortalTheme";
        public static string DemoPeriod = "DemoPeriod";
        public static string DemoSignup = "DemoSignup";
        public static string DisableUsersOnline = "DisableUsersOnline";
        public static string DisplayBetaNotice = "DisplayBetaNotice";
        public static string EnableFileAutoSync = "EnableFileAutoSync";
        public static string EnableModuleOnLineHelp = "EnableModuleOnLineHelp";
        public static string EnableRequestFilters = "EnableRequestFilters";
        public static string EncryptionKey = "EncryptionKey";
        public static string _FileExtensions = "FileExtensions";
        public static string FileExtensions
        {
            get { return _FileExtensions; }
            set { _FileExtensions = value; }
        }
        public static string Guid = "GUID";
        public static string HelpUrl = "HelpURL";
        public static string HelpURL
        {
            get { return HelpUrl; }
            set { HelpUrl = value; }
        }
        public static string HttpCompression = "HttpCompression";
        public static string HttpCompressionLevel = "HttpCompressionLevel";
        public static string JQueryDebug = "jQueryDebug";
        public static string JQuerySuperUsered = "jQuerySuperUsered";
        public static string JQueryUrl = "jQueryUrl";
        public static string ModuleCaching = "ModuleCaching";
        public static string PageQuota = "PageQuota";
        public static string PageStatePersister = "PageStatePersister";
        public static string PaymentProcessor = "PaymentProcessor";
        public static string PerformanceSetting = "PerformanceSetting";
        public static string ProcessorPassword = "ProcessorPassword";
        public static string ProcessorUserId = "ProcessorUserId";
        public static string ProxyPassword = "ProxyPassword";
        public static string ProxyPort = "ProxyPort";
        public static string ProxyServer = "ProxyServer";
        public static string ProxyUsername = "ProxyUsername";

        public static string _RememberCheckbox = "RememberCheckbox";
        public static string RememberCheckbox
        {
            get { return _RememberCheckbox; }
            set { _RememberCheckbox = value; }
        }

        public static string _SchedulerMode = "SchedulerMode";
        public static string SchedulerMode
        {
            get { return _SchedulerMode; }
            set { _SchedulerMode = value; }
        }

        public static string _SiteLogBuffer = "SiteLogBuffer";
        public static string SiteLogBuffer
        {
            get { return _SiteLogBuffer; }
            set { _SiteLogBuffer = value; }
        }

        public static string _SiteLogHistory = "SiteLogHistory";
        public static string SiteLogHistory
        {
            get { return _SiteLogHistory; }
            set { _SiteLogHistory = value; }
        }

        public static string _SiteLogStorage = "SiteLogStorage";
        public static string SiteLogStorage
        {
            get { return _SiteLogStorage; }
            set { _SiteLogStorage = value; }
        }

        public static string _SkinUpload = "SkinUpload";
        public static string SkinUpload
        {
            get { return _SkinUpload; }
            set { _SkinUpload = value; }
        }

        public static string SmtpAuthentication = "SMTPAuthentication";
        public static string SMTPAuthentication
        {
            get { return SmtpAuthentication; }
            set { SmtpAuthentication = value; }
        }

        public static string SmtpEnableSsl = "SMTPEnableSSL";
        public static string SMTPEnableSSL
        {
            get { return SmtpEnableSsl; }
            set { SmtpEnableSsl = value; }
        }

        public static string SmtpPassword = "SMTPPassword";
        public static string SMTPPassword
        {
            get { return SmtpPassword; }
            set { SmtpPassword = value; }
        }

        public static string SmtpServer = "SMTPServer";
        public static string SMTPServer
        {
            get { return SmtpServer; }
            set { SmtpServer = value; }
        }

        public static string SmtpUsername = "SMTPUsername";
        public static string SMTPUsername
        {
            get { return SmtpUsername; }
            set { SmtpUsername = value; }
        }

        public static string _SuperUserCopyright = "SuperUserCopyright";
        public static string SuperUserCopyright
        {
            get { return _SuperUserCopyright; }
            set { _SuperUserCopyright = value; }
        }

        public static string _SuperUserCurrency = "SuperUserCurrency";
        public static string SuperUserCurrency
        {
            get { return _SuperUserCurrency; }
            set { _SuperUserCurrency = value; }
        }

        public static string _SuperUserEmail = "SuperUserEmail";
        public static string SuperUserEmail
        {
            get { return _SuperUserEmail; }
            set { _SuperUserEmail = value; }
        }

        public static string _SuperUserFee = "SuperUserFee";
        public static string SuperUserFee
        {
            get { return _SuperUserFee; }
            set { _SuperUserFee = value; }
        }

        public static string _SuperUserPortalId = "SuperUserPortalId";
        public static string SuperUserPortalId
        {
            get { return _SuperUserPortalId; }
            set { _SuperUserPortalId = value; }
        }

        public static string _SuperUserSpace = "SuperUserSpace";
        public static string SuperUserSpace
        {
            get { return _SuperUserSpace; }
            set { _SuperUserSpace = value; }
        }

        public static string _SuperUserTitle = "SuperUserTitle";
        public static string SuperUserTitle
        {
            get { return _SuperUserTitle; }
            set { _SuperUserTitle = value; }
        }

        public static string SuperUserUrl = "SuperUserURL";
        public static string SuperUserURL
        {
            get { return SuperUserUrl; }
            set { SuperUserUrl = value; }
        }

        public static string _UseCustomErrorMessages = "UseCustomErrorMessages";
        public static string UseCustomErrorMessages
        {
            get { return _UseCustomErrorMessages; }
            set { _UseCustomErrorMessages = value; }
        }

        public static string _UseFriendlyUrls = "UseFriendlyUrls";
        public static string UseFriendlyUrls
        {
            get { return _UseFriendlyUrls; }
            set { _UseFriendlyUrls = value; }
        }

        public static string _UserQuota = "UserQuota";
        public static string UserQuota
        {
            get { return _UserQuota; }
            set { _UserQuota = value; }
        }

        public static string _UsersOnlineTime = "UsersOnlineTime";
        public static string UsersOnlineTime
        {
            get { return _UsersOnlineTime; }
            set { _UsersOnlineTime = value; }
        }

        public static string _WebRequestTimeout = "WebRequestTimeout";
        public static string WebRequestTimeout
        {
            get { return _WebRequestTimeout; }
            set { _WebRequestTimeout = value; }
        }

        public static string _WhitespaceFilter = "WhitespaceFilter";
        public static string WhitespaceFilter
        {
            get { return _WhitespaceFilter; }
            set { _WhitespaceFilter = value; }
        }

        public static string _PortalAdminEmail = "PortalAdminEmail";
        public static string PortalAdminEmail
        {
            get { return _PortalAdminEmail; }
            set { _PortalAdminEmail = value; }
        }

        public static string _PortalLogoTemplate = "PortalLogoTemplate";
        public static string PortalLogoTemplate
        {
            get { return _PortalLogoTemplate; }
            set { _PortalLogoTemplate = value; }
        }

        public static string _PortalCopyright = "PortalCopyright";
        public static string PortalCopyright
        {
            get { return _PortalCopyright; }
            set { _PortalCopyright = value; }
        }

        public static string _PortalSearchEngine = "PortalSearchEngine";
        public static string PortalSearchEngine
        {
            get { return _PortalSearchEngine; }
            set { _PortalSearchEngine = value; }
        }

        public static string _PortalCssTemplate = "PortalCssTemplate";
        public static string PortalCssTemplate
        {
            get { return _PortalCssTemplate; }
            set { _PortalCssTemplate = value; }
        }

        public static string _PortalUserRegistration = "PortalUserRegistration";
        public static string PortalUserRegistration
        {
            get { return _PortalUserRegistration; }
            set { _PortalUserRegistration = value; }
        }

        public static string _PortalLoginpage = "PortalLoginpage";
        public static string PortalLoginpage
        {
            get { return _PortalLoginpage; }
            set { _PortalLoginpage = value; }
        }

        public static string _PortalUserProfilePage = "PortalUserProfilePage";
        public static string PortalUserProfilePage
        {
            get { return _PortalUserProfilePage; }
            set { _PortalUserProfilePage = value; }
        }

        public static string _PortalDefaultPage = "PortalDefaultPage";
        public static string PortalDefaultPage
        {
            get { return _PortalDefaultPage; }
            set { _PortalDefaultPage = value; }
        }

        public static string _PortalCurrency = "PortalCurrency";
        public static string PortalCurrency
        {
            get { return _PortalCurrency; }
            set { _PortalCurrency = value; }
        }

        public static string _PortalPaymentProcessor = "PortalPaymentProcessor";
        public static string PortalPaymentProcessor
        {
            get { return _PortalPaymentProcessor; }
            set { _PortalPaymentProcessor = value; }
        }

        public static string PortalProcessorUserId = "PortalProcessorUserId";
        public static string PortalProcessorPassword = "PortalProcessorPassword";

        public static string _PortalDefaultLanguage = "PortalDefaultLanguage";
        public static string PortalDefaultLanguage
        {
            get { return _PortalDefaultLanguage; }
            set { _PortalDefaultLanguage = value; }
        }

        public static string _PortalTimeZone = "PortalTimeZone";
        public static string PortalTimeZone
        {
            get { return _PortalTimeZone; }
            set { _PortalTimeZone = value; }
        }

        public static string _PortalRegistrationPage = "PortalRegistrationPage";
        public static string PortalRegistrationPage
        {
            get { return _PortalRegistrationPage; }
            set { _PortalRegistrationPage = value; }
        }

        public static string PortalAnalyticsId = "PortalAnalyticsID";
        public static string PortalAnalyticsUrlParameters = "PortalAnalyticsURLParameters";


        /* New Keyes */
        public static string _PortalShowProfileLink = "PortalShowProfileLink";
        public static string PortalShowProfileLink
        {
            get { return _PortalShowProfileLink; }
            set { _PortalShowProfileLink = value; }
        }
        public static string _PortalShowSubscribe = "PortalShowSubscribe";
        public static string PortalShowSubscribe
        {
            get { return _PortalShowSubscribe; }
            set { _PortalShowSubscribe = value; }
        }

        public static string _PortalShowLogo = "PortalShowLogo";
        public static string PortalShowLogo
        {
            get { return _PortalShowLogo; }
            set { _PortalShowLogo = value; }
        }

        public static string _PortalShowLoginStatus = "PortalShowLoginStatus";
        public static string PortalShowLoginStatus
        {
            get { return _PortalShowLoginStatus; }
            set { _PortalShowLoginStatus = value; }
        }

        public static string _PortalShowFooterLink = "PortalShowFooterLink";
        public static string PortalShowFooterLink
        {
            get { return _PortalShowFooterLink; }
            set { _PortalShowFooterLink = value; }
        }

        public static string _PortalShowFooter = "PortalShowFooter";
        public static string PortalShowFooter
        {
            get { return _PortalShowFooter; }
            set { _PortalShowFooter = value; }
        }

        public static string _PortalShowBreadCrum = "PortalShowBreadCrum";
        public static string PortalShowBreadCrum
        {
            get { return _PortalShowBreadCrum; }
            set { _PortalShowBreadCrum = value; }
        }

        public static string _PortalShowCopyRight = "PortalShowCopyRight";
        public static string PortalShowCopyRight
        {
            get { return _PortalShowCopyRight; }
            set { _PortalShowCopyRight = value; }
        }

        public static string PortalGoogleAdSenseId = "PortalGoogleAdSenseID";
        public static string PortalGoogleAdSenseID
        {
            get { return PortalGoogleAdSenseId; }
            set { PortalGoogleAdSenseId = value; }
        }
        public static string PortalGoogleAdsenseChannelId = "PortalGoogleAdsenseChannelID";//Not Used
        public static string PortalGoogleAdsenseChannelID
        {
            get { return PortalGoogleAdsenseChannelId; }
            set { PortalGoogleAdsenseChannelId = value; }
        }

        public static string _PortalUserActivation = "PortalUserActivation";
        public static string PortalUserActivation
        {
            get { return _PortalUserActivation; }
            set { _PortalUserActivation = value; }
        }

        public static string _PortalForgotPassword = "PortalForgotPassword";
        public static string PortalForgotPassword
        {
            get { return _PortalForgotPassword; }
            set { _PortalForgotPassword = value; }
        }

        public static string _PortalPageNotAccessible = "PortalPageNotAccessible";
        public static string PortalPageNotAccessible
        {
            get { return _PortalPageNotAccessible; }
            set { _PortalPageNotAccessible = value; }
        }

        public static string _PortalPageNotFound = "PortalPageNotFound";
        public static string PortalPageNotFound
        {
            get { return _PortalPageNotFound; }
            set { _PortalPageNotFound = value; }
        }

        public static string _PortalPasswordRecovery = "PortalPasswordRecovery";
        public static string PortalPasswordRecovery
        {
            get { return _PortalPasswordRecovery; }
            set { _PortalPasswordRecovery = value; }
        }

        //New
        public static string _PortalUserProfileMaxImageSize = "PortalUserProfileMaxImageSize";
        public static string PortalUserProfileMaxImageSize
        {
            get { return _PortalUserProfileMaxImageSize; }
            set { _PortalUserProfileMaxImageSize = value; }
        }

        public static string _PortalUserProfileMediumImageSize = "PortalUserProfileMediumImageSize";
        public static string PortalUserProfileMediumImageSize
        {
            get { return _PortalUserProfileMediumImageSize; }
            set { _PortalUserProfileMediumImageSize = value; }
        }

        public static string _PortalUserProfileSmallImageSize = "PortalUserProfileSmallImageSize";
        public static string PortalUserProfileSmallImageSize
        {
            get { return _PortalUserProfileSmallImageSize; }
            set { _PortalUserProfileSmallImageSize = value; }
        }

        public static string _SiteAdminEmailAddress = "SiteAdminEmailAddress";
        public static string SiteAdminEmailAddress
        {
            get { return _SiteAdminEmailAddress; }
            set { _SiteAdminEmailAddress = value; }
        }

        public static string _IsPortalMenuIsImage = "IsPortalMenuIsImage";
        public static string IsPortalMenuIsImage
        {
            get { return _IsPortalMenuIsImage; }
            set { _IsPortalMenuIsImage = value; }
        }

        public static string _PortalMenuImageExtension = "PortalMenuImageExtension";
        public static string PortalMenuImageExtension
        {
            get { return _PortalMenuImageExtension; }
            set { _PortalMenuImageExtension = value; }
        }

        public static string _InsertSessionTrackingPages = "InsertSessionTrackingPages";
        public static string InsertSessionTrackingPages
        {
            get { return _InsertSessionTrackingPages; }
            set { _InsertSessionTrackingPages = value; }
        }

        //CssJs Optimization Keys
        public static string _OptimizeCss = "OptimizeCss";
        public static string OptimizeCss
        {
            get { return _OptimizeCss; }
            set { _OptimizeCss = value; }
        }

        public static string _OptimizeJs = "OptimizeJs";
        public static string OptimizeJs
        {
            get { return _OptimizeJs; }
            set { _OptimizeJs = value; }
        }

        public static string _EnableLiveFeeds = "EnableLiveFeeds";
        public static string EnableLiveFeeds
        {
            get { return _EnableLiveFeeds; }
            set { _EnableLiveFeeds = value; }
        }

        public static string _ShowSideBar = "ShowSideBar";
        public static string ShowSideBar
        {
            get { return _ShowSideBar; }
            set { _ShowSideBar = value; }
        }

        public static string _SettingPageExtension = "PageExtension";
        public static string SettingPageExtension
        {
            get { return _SettingPageExtension; }
            set { _SettingPageExtension = value; }
        }

        private static string _pageExtension = string.Empty;
        public static string PageExtension
        {
            get { return _pageExtension; }
            set { _pageExtension = value; }
        }


        //Added by Bj for OpenID consumer key and Secret key


        public static string ShowOpenId = "ShowOpenID";
        public static string ShowOpenID
        {
            get { return ShowOpenId; }
            set { ShowOpenId = value; }
        }

        public static string _FaceBookConsumerKey = "FaceBookConsumerKey";
        public static string FaceBookConsumerKey
        {
            get { return _FaceBookConsumerKey; }
            set { _FaceBookConsumerKey = value; }
        }

        public static string _FaceBookSecretkey = "FaceBookSecretkey";
        public static string FaceBookSecretkey
        {
            get { return _FaceBookSecretkey; }
            set { _FaceBookSecretkey = value; }
        }

        public static string _LinkedInConsumerKey = "LinkedInConsumerKey";
        public static string LinkedInConsumerKey
        {
            get { return _LinkedInConsumerKey; }
            set { _LinkedInConsumerKey = value; }
        }

        public static string _LinkedInSecretKey = "LinkedInSecretKey";
        public static string LinkedInSecretKey
        {
            get { return _LinkedInSecretKey; }
            set { _LinkedInSecretKey = value; }
        }

        //Message Setting
        public static string _MessageTemplate = "MessageTemplate";
        public static string MessageTemplate
        {
            get { return _MessageTemplate; }
            set { _MessageTemplate = value; }
        }

        //Enable Heavy Cache
        private static bool _frontMenu = true;
        public static bool FrontMenu
        {
            get { return _frontMenu; }
            set { _frontMenu = value; }
        }
        private static bool _sideMenu = true;
        public static bool SideMenu
        {
            get { return _sideMenu; }
            set { _sideMenu = value; }
        }
        private static bool _footerMenu = true;
        public static bool FooterMenu
        {
            get { return _footerMenu; }
            set { _footerMenu = value; }
        }

        //Scheduler
        public static string _Scheduler = "Scheduler";
        public static string Scheduler
        {
            get { return _Scheduler; }
            set { _Scheduler = value; }
        }

    }

    public class GoogleReCaptcha
    {
        public const string SiteKey = "SiteKey";
        public const string SecretKey = "SecretKey";
    }

    public class Smtp
    {
        public const string MailServerProviderId = "MailServerProviderId";
        public const string NotificationSenderId = "NotificationSenderId";
        public const string Authentication = "Authentication";
        public const string EnableSsl = "EnableSsl";
    }
    public class OrderSetting
    {
        public const string DueDateRange = "DueDateRange";
        public const string ShipDateRange = "ShipDateRange";
    }

    public class PaypalType
    {
        public const string Live = "PAYPAL_LIVE";
        public const string SandBox = "PAYPAL_SANDBOX";
    }
}
