using System;
using System.Web;

namespace Eagle.Common.Cookie
{
    /// <summary>
    /// A generic Cookie class that manages an individual Cookie by localizing the
    /// cookie management into a single class. This means the Cookie's name and
    /// and timing is abstracted.
    /// 
    /// The GetId() method is the key method here which retrieves a Cookie Id.
    /// If the cookie exists it returns the value, otherwise it generates a new
    /// Id and creates the cookie with the specs of the class and
    /// 
    /// It's recommended you store this class as a static member off another
    /// object to have
    /// </summary>
    public class CookieManager : System.Web.UI.Page
    {
        public static HttpCookie GetCookieValue(string key)
        {
            if (HttpContext.Current.Request.Cookies[key] == null) return null;
            return HttpContext.Current.Request.Cookies[key];
        }
        //public string[] LoadCookies()
        //{
        //    List<string> items = new List<string>(2);
        //    if (HttpContext.Current.Request.Cookies[CookieName] != null)
        //    {
        //        string username = HttpContext.Current.Server.HtmlEncode(HttpContext.Current.Request.Cookies[CookieName]["UserName"]);
        //        string password = HttpContext.Current.Server.HtmlEncode(HttpContext.Current.Request.Cookies[CookieName]["Password"]);
        //        items.Add(username);
        //        items.Add(password);
        //    }
        //    return items.ToArray();
        //}

        /// <summary>
        /// Writes the cookie into the response stream with the value passed. The value
        /// is always the UserId.
        /// <seealso>Class WebStoreCookie</seealso>
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="value"></param>
        /// <param name="nonPersistent"></param>
        /// <param name="expiredMonths"></param>
        /// <returns>Void</returns>
        public static void WriteCookie(string cookieName, string value, bool nonPersistent=false, int? expiredMonths = 48)
        {
            HttpCookie cookie = new HttpCookie(cookieName, value);

            SetCookiePath(cookie, null);

            if (!nonPersistent)
                cookie.Expires = DateTime.UtcNow.AddMonths(Convert.ToInt32(expiredMonths));

            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public void WriteCookies(string cookieName, string username, string password, int? expiredDays = 30)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Values.Add("UserName", username);
            cookie.Values.Add("Password", password);

            cookie.Expires = DateTime.UtcNow.AddDays(Convert.ToInt32(expiredDays));
            HttpContext.Current.Request.Cookies.Add(cookie);
        }

        /// <summary>
        /// Removes the cookie by clearing it out and expiring it immediately.
        /// <seealso>Class WebStoreCookie</seealso>
        /// </summary>
        /// <returns>Void</returns>
        public static void RemoveCookie(string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];

            if (cookie != null)
            {
                SetCookiePath(cookie, null);
                cookie.Expires = DateTime.UtcNow.AddHours(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        //private void DeleteAllCookie()
        //{
        //    string[] theCookies = System.IO.Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Cookies));
        //    foreach (string currentFile in theCookies)
        //    {
        //        try
        //        {

        //            System.IO.File.Delete(currentFile);
        //        }
        //        catch (Exception ex)
        //        {
        //            ex.Message.ToString();
        //        }
        //    }
        //}

        /// <summary>
        /// Method that generates the ID stored in the cookie. You can override
        /// this method in a subclass to handle custom or specific Id creation.
        /// </summary>
        /// <returns></returns>
        protected virtual string GenerateId()
        {
            return Guid.NewGuid().ToString().GetHashCode().ToString("x");
        }

        /// <summary>
        /// Determines whether the cookie exists
        /// <seealso>Class wwCookie</seealso>
        /// </summary>
        /// <returns>Boolean</returns>
        public static bool HasCookieExisted(string cookieName)
        {
            // Check to see if we have a cookie we can use
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie == null)
                return false;

            return true;
        }

        public static void SetCookieValue(string key, string value, DateTime? expiresWhen)
        {
            var cookie = new HttpCookie(key, value);
            if (expiresWhen.HasValue) cookie.Expires = expiresWhen.Value;
            HttpContext.Current.Response.Cookies.Set(cookie);
        }
        public static void SetCookieValue(string key, string value, int? expiredDays = 30)
        {
            SetCookieValue(key, value, DateTime.UtcNow.AddDays(Convert.ToInt32(expiredDays)));
        }

        /// <summary>
        /// Sets the Cookie Path
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="path"></param>
        private static void SetCookiePath(HttpCookie cookie, string path)
        {
            if (path == null)
            {
                path = HttpContext.Current.Request.ApplicationPath;
                if (path != "/")
                    cookie.Path = path + "/";
                else
                    cookie.Path = "/";
            }
        }

        //protected override WebRequest GetWebRequest(Uri address)
        //{
        //    WebRequest request = base.GetWebRequest(address);

        //    if (request is HttpWebRequest)
        //    {
        //        CookieContainer mContainer = new CookieContainer();
        //        (request as HttpWebRequest).CookieContainer = mContainer;
        //    }
        //    return request;
        //}
    }
}
