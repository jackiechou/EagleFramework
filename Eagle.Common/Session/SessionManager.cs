using System;
using System.Collections.Concurrent;
using System.Web;
using System.Web.Security;

namespace Eagle.Common.Session
{
    public class SessionManager : System.Web.UI.Page
    {
        public void SetSession(string sessionKey, object objSessionValue)
        {
            HttpContext.Current.Session[sessionKey] = objSessionValue;
        }

        public T GetSessionValue<T>(string sessionKey)
        {
            return (T)HttpContext.Current.Session[sessionKey];
        }

        public int GetSessionTimeoutInMs()
        {
            return (Session.Timeout * 60000) - 10000;
        }

        public void RefreshSession(string cookieName, string redirectUrl)
        {
            if (Context.Session != null)
            {
                if (Session.IsNewSession)
                {
                    // If it says it is a new session, but an existing cookie exists, then it must have timed out
                    string cookieHeader = HttpContext.Current.Request.Headers[cookieName];
                    if ((null != cookieHeader) &&
                        (cookieHeader.IndexOf("Username", StringComparison.Ordinal) >= 0))
                    {
                        if (HttpContext.Current.Request.IsAuthenticated)
                        {
                            FormsAuthentication.SignOut();

                            HttpContext.Current.Session.Clear();
                            HttpContext.Current.Session.Abandon();

                            HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
                            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            HttpContext.Current.Response.Cache.SetNoStore();
                            HttpContext.Current.Response.Redirect("/Admin/Login", true);
                            HttpContext.Current.Response.Buffer = true;
                            HttpContext.Current.Response.End();
                        }
                        else
                        {
                            HttpContext.Current.Response.Redirect(redirectUrl);
                        }
                    }
                }
            }
        }
    }
}
