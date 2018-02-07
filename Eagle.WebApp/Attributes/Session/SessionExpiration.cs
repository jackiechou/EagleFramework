using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Eagle.Core.Cookie;
using Eagle.Resources;

namespace Eagle.WebApp.Attributes.Session
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SessionExpiration : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var response = filterContext.HttpContext.Response;
            var session = filterContext.HttpContext.Session;
            string returnedUrl = request.Url != null ? $"/Admin/Login?desiredUrl={request.Url.AbsoluteUri.ToLower()}" : "/Admin/Login";

            if (session == null)
            {
                if (request.IsAjaxRequest())
                {
                    // put whatever data you want which will be sent to the client
                    filterContext.Result = new JsonResult
                    {
                        Data = new
                        {
                            desiredUrl = returnedUrl,
                            isTimedOut = true,
                            message = LanguageResource.SessionTimeOut
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{{ "action", "Login" },{ "controller", "User" },{ "desiredUrl", filterContext.HttpContext.Request.RawUrl}});
                    //filterContext.Result = new EmptyResult();
                    //if (!response.IsRequestBeingRedirected)
                    //{
                    //    response.Redirect("/Admin/Login", true);
                    //    response.Buffer = true;
                    //    response.Flush();
                    //}
                }
            }
            else
            {
                // check if a new session id was generated
                if (session.IsNewSession)
                {
                    // If it says it is a new session, but an existing cookie exists, then it must have timed out
                    string sessionCookie = request.Headers[CookieSetting.UserInfo];
                    if ((null != sessionCookie) &&
                        (sessionCookie.IndexOf("ASP.NET_SessionId", StringComparison.Ordinal) >= 0))
                    {
                        if (request.IsAuthenticated)
                        {
                            FormsAuthentication.SignOut();

                            session.Clear();
                            session.Abandon();

                            response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
                            response.Cache.SetCacheability(HttpCacheability.NoCache);
                            response.Cache.SetNoStore();
                            response.Redirect("/Admin/Login", true);
                            response.Buffer = true;
                            response.End();
                        }
                        //else
                        //{
                        //    //redirect to desired url 
                        //    //filterContext.Result =new RedirectToRouteResult(new RouteValueDictionary{{"action", "Login"},{"controller", "User"},{"desiredUrl", filterContext.HttpContext.Request.RawUrl}});
                        //    response.Redirect(returnedUrl);
                        //}
                    }
                    //else
                    //{
                    //    //redirect to desired url 
                    //    response.Redirect(returnedUrl);
                    //}
                }
                base.OnActionExecuting(filterContext);
            }
        }
    }
}
