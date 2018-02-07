namespace Eagle.Core.Cookie
{
    public class CookieSetting
    {
        //Cookie Keys
        public static string UserInfo = "Eagle-UserInfo";
        public static string CustInfo = "Eagle-CustInfo";
        public static string CustInfoEmail = "Eagle-CustInfo-Email";
        public static string CustInfoPassword = "Eagle-CustInfo-Password";

        //Cookie Settings - AuthCookie
        public static bool HttpOnly = true;
        public static string CookieName = "EAGLE-ASPXAUTH";
        public static string CookiePath = "/";
        public static string LoginPath = "/Admin/Login";
        public static string LogoutPath = "/Admin/LogOff";
        public static string ReturnUrlParameter = "desiredUrl";
        public static int Expires = 43200; // allows you to set how long the issued cookie is valid for 30 days 
    }
}