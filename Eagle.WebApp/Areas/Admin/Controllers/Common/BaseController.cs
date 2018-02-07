using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Mvc;
using Eagle.Common.Extensions;
using Eagle.Core.Configuration;
using Eagle.Services;
using Eagle.Services.SystemManagement;

namespace Eagle.WebApp.Areas.Admin.Controllers.Common
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            if (ControllerContext != null)
            {
                var routeData = ControllerContext.RequestContext.RouteData;
                ViewBag.Title = routeData.Values["controller"].ToString();
            }
        }

        //protected override void Initialize(RequestContext requestContext)
        //{
        //    base.Initialize(requestContext);

        //    if (ClaimsPrincipal.Current == null)
        //    {
        //        requestContext.HttpContext.Response.Clear();
        //        requestContext.HttpContext.Response.Redirect("/Admin/Login");
        //        requestContext.HttpContext.Response.End();            
        //    }
        //    CurrentClaimsIdentity = ClaimsPrincipal.Current;
        //}


        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        protected BaseController(IEnumerable<IBaseService> services)
        {
            Services = services;
        }

        /// <summary>
        /// To be injected using property injection.
        /// </summary>
        public ISecurityService BaseSecurityService { get; set; }

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <value>
        /// The services.
        /// </value>
        protected IEnumerable<IBaseService> Services { get; }

        /// <summary>
        /// Sets the identity.
        /// </summary>
        internal void SetIdentity()
        {
            if (CurrentClaimsIdentity == null)
            {
                RedirectToAction("LogOff", "Account");
            }
            else if (CurrentClaimsIdentity.Claims == null)
            {
                RedirectToAction("LogOff", "Account");
            }
            else
            {
                foreach (var service in Services)
                {
                    service.SetIdentity(CurrentClaimsIdentity);
                }
                BaseSecurityService.SetIdentity(CurrentClaimsIdentity);
            }
        }

        /// <summary>
        /// Gets the current claims identity.
        /// </summary>
        /// <value>
        /// The current claims identity.
        /// </value>
        public ClaimsPrincipal CurrentClaimsIdentity
        {
            get
            {
                if (ClaimsPrincipal.Current != null)
                {
                    return ClaimsPrincipal.Current;
                }

                Response.Clear();
                Response.Redirect("/Admin/Login");
                HttpContext.Response.End();
                return ClaimsPrincipal.Current;
            }
        }

        public IEnumerable<Claim> CurrentClaims
        {
            get
            {
                if (ClaimsPrincipal.Current != null)
                {
                    var claims = ClaimsPrincipal.Current.Claims;
                    return claims;
                }
                else
                {
                    return null;
                }
            }
        }
        

        public List<string> Roles
{
    get
    {
        return CurrentClaims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();
    }
}

public Guid ApplicationId
{
    get
    {
        var claim = CurrentClaims.FirstOrDefault(c => c.Type == SettingKeys.ApplicationId);
        return claim?.Value.ToGuid() ?? GlobalSettings.DefaultApplicationId;
    }
}

public string LanguageCode
{
    get
    {
        var claim = CurrentClaims.FirstOrDefault(c => c.Type == SettingKeys.LanguageCode);
        return claim?.Value;
    }
}

public string UserName
{
    get
    {
        var claim = CurrentClaims.FirstOrDefault(c => c.Type == SettingKeys.UserName);
        return claim?.Value;
    }
}

public Guid UserId
{
    get
    {
        var claim = CurrentClaims.FirstOrDefault(c => c.Type == SettingKeys.UserId);
        return claim?.Value.ToGuid() ?? Guid.Parse(GlobalSettings.DefaultUserId);
    }
}

public Guid RoleId
{
    get
    {
        var claim = CurrentClaims.FirstOrDefault(c => c.Type == SettingKeys.RoleId);
        return claim?.Value.ToGuid() ?? GlobalSettings.DefaultRoleId;
    }
}

public int VendorId
{
    get
    {
        var claim = CurrentClaims.FirstOrDefault(c => c.Type == SettingKeys.VendorId);
        return claim?.Value.ToInt() ?? GlobalSettings.DefaultVendorId;
    }
}

//protected override ViewResult View(string viewName, string masterName, object model)
//{
//    var page = model as Page;
//    if (page != null)
//    {
//        ViewBag.Title = page.Title;
//    }
//    return base.View(viewName, masterName, model);
//}

//protected override void OnException(ExceptionContext filterContext)
//{
//    //Log error
//    logger = log4net.LogManager.GetLogger(filterContext.Controller.ToString());
//    logger.Error(filterContext.Exception.Message, filterContext.Exception);

//    var message = ValidationExtension.ConvertValidateErrorToString((filterContext.Exception as ValidationError));

//    //If the request is AJAX return JSON else redirect user to Error view.
//    if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
//    {
//        //Return JSON
//        filterContext.Result = new JsonResult
//        {
//            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
//            Data = new { error = true, message = message }
//        };
//    }
//    else
//    {

//        //Redirect user to error page
//        filterContext.ExceptionHandled = true;
//        //filterContext.Result = this.RedirectToAction("Index", "Error");
//        filterContext.Result = this.RedirectToAction("Index", "Error");
//    }
//    base.OnException(filterContext);
//}

//private log4net.ILog logger;

#region Dispose

/// <summary>
/// Resolves required object
/// </summary>
/// <typeparam name="TObject"></typeparam>
/// <returns></returns>
protected TObject Resolve<TObject>()
    where TObject : class
{
    var result = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(TObject)) as TObject;
    var disposable = (IDisposable)result;
    if (disposable != null)
    {
        Disposables.Add(disposable);
    }
    return result;
}

protected override void Dispose(bool disposing)
{
    if (_isDisposed || !disposing) return;

    foreach (var disposable in Disposables)
    {
        disposable?.Dispose();
    }

    _isDisposed = true;
    Disposables.Clear();
    Disposables = null;

    base.Dispose();

}


protected T CreateObject<T>(Func<T> createObject) where T : IDisposable
{
    var result = createObject();
    Manage(result);
    return result;
}

protected void Manage(IDisposable disposable)
{
    if (disposable != null) Disposables.Add(disposable);
}

protected void Manage(IEnumerable<IDisposable> disposables)
{
    foreach (var disposable in disposables)
    {
        Manage(disposable);
    }
}

private ICollection<IDisposable> Disposables { get; set; } = new List<IDisposable>();

private bool _isDisposed;

       
        #endregion Dispose
    }
}