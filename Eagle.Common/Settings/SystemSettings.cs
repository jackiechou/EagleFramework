#region "References"
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
#endregion

namespace Eagle.Common.Settings
{
    public static class SystemSettings
    {
        #region "Declaration"
        public static string GlbDefaultPane = "ContentPane";
        public static string DbName = ConfigurationManager.AppSettings["DatabaseName"].ToString();
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public static int ConnectionTimeout = 216000;
        public static int CommandTimeout = 900000;
        public static int SessionTimeout = 216000;
        public static string Register = "Register";
        public static string Login = "Login";
        public static string Logout = "Logout";
        public static string Administration = "Administration";
        public static string AnonymousUsername = "anonymous user";
        public static string RegisterUserRolename = "Registered User";
        public static string AnonymousRoleid = string.Empty;
        public static string DataBaseOwner = GetDataBaseOwner();
        public static string ObjectQualifer = GetObjectQualifer();
        public static string[] FileTypes = { ".bmp", ".gif", ".png", ".jpg", ".jpeg", ".tiff", ".swf" };
        public static string[] SystemRoles = { "registered user", "anonymous user", "site admin", "super user" };
        public static string[] SystemUsers = { "anonymoususer", "siteadmin", "Admin" };
        public static string[] SystemSuperRoles = { "site admin", "super user" };
        public static string[] SuperRole = {"super user" };
        public static string  Siteadmin = "site admin";
        public static string[] SystemApplicationRoles = { "super user" };
        public static string[] SystemUserNotallowHtmlcomment = { "anonymoususer" };
        public static string[] SystemRolesAllowHtmlcomment = { "registered user", "site admin", "super user" };
        public static int[] SystemMessageTemplates = {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16};
        public static Int32 AccountActivationEmail = 1;
        public static Int32 PasswordChangeRequestEmail = 2;
        public static Int32 ActivationSuccessfulEmail = 3;
        public static Int32 PasswordRecoveredSuccessfulEmail = 4;
        public static Int32 UserRegistrationHelp = 5;
        public static Int32 UserResisterSucessfulInformationNone = 6;
        public static Int32 UserResisterSucessfulInformationPrivate = 7;
        public static Int32 UserResisterSucessfulInformationVerified = 8;
        public static Int32 UserResisterSucessfulInformationPublic = 9;
        public static Int32 ActivationSuccessfulInformation = 10;
        public static Int32 ActivationFailInformation = 11;
        public static Int32 PasswordForgotHelp = 12;
        public static Int32 PasswordRecoveredSucessfulInformation = 13;
        public static Int32 PasswordRecoverdFailInformation = 14;
        public static Int32 PasswordForgotUsernamePasswordMatch = 15;
        public static Int32 PasswordRecoveredHelp = 16;
        public static Int32 OrderPlaced = 17;
        public static Int32 OrderStatusChanged = 18;
        public static Int32 SharedWishedList = 19;
        public static Int32 ReferAFriendEmail = 20;
        public static Int32 OutOfStockNotification = 21;
        public static string GlbConfigFolder = "\\Config\\";
        public static string GlbVersionConfigFolder = "\\Config\\VersionConfig\\";
        public static string GlbConnStringConfigFolder = "\\Config\\ConnStringConfig\\";
        public static string[] IncompressibleExtensions = { ".gif", ".jpg", ".png", ".axd", ".asmx", ".css", ".js", "Fconnector.aspx", ".html", ".htm", "connector.aspx?", "fckstyles.xml" };
        public static string[] AllowedExtensions = { ".gif", ".jpg", ".png" };
        public static string[] AllowedFiles = { ".gif", ".jpg", ".png", ".htm", ".xml", ".html", ".cs", ".ascx", ".js", ".asmx",".css" };
        public static string[] LanguageCodes = { "en-US", "vi_VN"};
        //public static string SYSTEM_MODULES = System.Configuration.ConfigurationManager.AppSettings["SYSTEM_MODULES"];
        
        #endregion

        public static string GetDataBaseOwner()
        {
            string databaseOwner = ConfigurationManager.AppSettings["databaseOwner"].ToString();
            if (databaseOwner != "" && databaseOwner.EndsWith(".") == false)
            {
                databaseOwner += ".";
            }
            return databaseOwner;
        }

        public static string GetObjectQualifer()
        {
            string objectQualifier = ConfigurationManager.AppSettings["objectQualifier"].ToString();
            if ((objectQualifier != "") && (objectQualifier.EndsWith("_") == false))
            {
                objectQualifier += "_";
            }
            return objectQualifier;
        }

        private static string GetConnectionString(string name)
        {
            var setting = ConfigurationManager.ConnectionStrings[name];
            return (setting == null) ? string.Empty : setting.ConnectionString;
        }
    }
}
