using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Eagle.Repositories;
using Eagle.Services.Business;
using Eagle.Services.Skins;
using Eagle.Services.SystemManagement;
using Eagle.Common.Extensions;
using System.Web.Security;
using System.Security.Principal;
using System.Threading;
using System.Web.Script.Serialization;
using Eagle.Core.Configuration;

namespace Eagle.WebApp
{
    public class MvcApplication : HttpApplication
    {
        private readonly IApplicationService _appService;

        public MvcApplication()
        {
            IUnitOfWork unitOfWork = new UnitOfWork();
            ICacheService cacheService = new CacheService(unitOfWork);
            ILanguageService languageService = new LanguageService(unitOfWork);
            ILogService logService = new LogService(unitOfWork);
            IDocumentService documentService = new DocumentService(unitOfWork);
            IThemeService themeService = new ThemeService(unitOfWork, cacheService, documentService);
            IContactService contactService = new ContactService(unitOfWork, documentService);
            IVendorService vendorService = new VendorService(unitOfWork, contactService, documentService);
            _appService = new ApplicationService(unitOfWork, cacheService, languageService, logService, themeService, vendorService);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //HttpApplication context = (HttpApplication)sender;
            //context.Response.SuppressFormsAuthenticationRedirect = true;
            //if (ConfigurationManager.AppSettings["RedirectHttps"].Equals("true"))
            //{
            //    if (!Context.Request.IsSecureConnection)
            //        Response.Redirect(Context.Request.Url.ToString().Replace("http:", "https:"));
            //}

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }

        protected void Application_Start()
        {
            AutofacConfig.ConfigureContainer();
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            _appService.SetUpCulture(GlobalSettings.DefaultApplicationId);
            _appService.SetUpAppConfig(GlobalSettings.DefaultApplicationId);

            //log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Log4Net.config")));
           
            // To ensure that claims authentication works with AntiForgeryToken
            //AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
            //AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimsIdentity.DefaultNameClaimType;
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Email;

            ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());
        }

        //public override void Init()
        //{
        //    var sam = FederatedAuthentication.SessionAuthenticationModule;
        //    if (sam != null)
        //    {
        //        sam.IsSessionMode = true;
        //    }
        //    //PassiveModuleConfiguration.CacheSessionsOnServer();
        //    PassiveModuleConfiguration.EnableSlidingSessionExpirations();
        //    //PassiveModuleConfiguration.OverrideWSFedTokenLifetime();
        //    //PassiveModuleConfiguration.SuppressLoginRedirectsForApiCalls();
        //    //PassiveModuleConfiguration.SuppressSecurityTokenExceptions();
        //}

        //protected void Application_AuthorizeRequest(Object sender, EventArgs e)
        //{
        //    if (!Request.IsAuthenticated)
        //    {
        //        const string returnUrlParam = "desiredUrl";

        //        //Remove parameters in ReturnUrl QueryString
        //        if (Response.RedirectLocation != null && Response.RedirectLocation.Contains(returnUrlParam))
        //        {
        //            Response.RedirectLocation = string.Format(
        //                "{0}{1}={2}",
        //                Response.RedirectLocation.Remove(Response.RedirectLocation.IndexOf(returnUrlParam, StringComparison.Ordinal)),
        //                returnUrlParam,
        //                (Request.RawUrl.Contains(returnUrlParam)
        //                    ? Request.RawUrl.Substring(Request.RawUrl.IndexOf(returnUrlParam, StringComparison.Ordinal) + 10)
        //                    : Request.RawUrl
        //                )
        //            );
        //        }
        //    }
        //    else
        //    {
        //        Response.Redirect("Account/Login");
        //    }
        //}

        //protected void Application_EndRequest(object sender, EventArgs e)
        //{
        //    const string returnUrlParam = "desiredUrl";

        //    //Remove parameters in ReturnUrl QueryString
        //    if (Response.RedirectLocation != null && Response.RedirectLocation.Contains(returnUrlParam))
        //    {
        //        Response.RedirectLocation = string.Format(
        //            "{0}{1}={2}",
        //            Response.RedirectLocation.Remove(Response.RedirectLocation.IndexOf(returnUrlParam, StringComparison.Ordinal)),
        //            returnUrlParam,
        //            (Request.RawUrl.Contains(returnUrlParam)
        //                ? Request.RawUrl.Substring(Request.RawUrl.IndexOf(returnUrlParam, StringComparison.Ordinal) + 10)
        //                : Request.RawUrl
        //            )
        //        );
        //    }
        //}

        protected void Application_AuthenticateRequest()
        {
            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];

            // If the cookie can't be found, don't issue the ticket
            if (authCookie == null) return;

            // Get the authentication ticket and rebuild the principal & identity
            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            if (null == authTicket)
            {
                return;
            }

            // Get the custom user data encrypted in the ticket.
            var formsIdentity = new FormsIdentity(authTicket);
            string[] roles = formsIdentity.Ticket.UserData.Split(',');
            var claimsPrincipal = new GenericPrincipal(formsIdentity, roles); 

            // Set the context user.
            Context.User = claimsPrincipal;
        }

        /////// <summary>
        /////// hook up the PostAuthenticationRequest event
        /////// Normal forms authentication will have recognised the authentication cookie and created a GenericPrincipal and FormsIdentity.
        /////// </summary>
        /////// <param name="sender"></param>
        /////// <param name="e"></param>
        //protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        //{ 
        //    //var user = HttpContext.Current.User;
        //    //if (user == null || !user.Identity.IsAuthenticated)
        //    //{
        //    //    return;
        //    //}

        //    //// get the identity of the user - ClaimsPrincipal.Current //var userIdentity = (ClaimsIdentity)User.Identity;
        //    //FormsIdentity userIdentity = (FormsIdentity)user.Identity;
        //    //if (userIdentity == null) return;

        //    ////Get the form authentication ticket of the user
        //    //FormsAuthenticationTicket ticket = userIdentity.Ticket;

        //    ////Get the stored user-data, in this case, we get the roles stored for this request from the ticket
        //    ////var userInfo = JsonConvert.DeserializeObject<UserInfoDetail>(ticket.UserData);
        //    //var claims = userIdentity.Claims.ToList();

        //    ////Get the roles stored as UserData into ticket
        //    ////var roleClaimType = userIdentity.RoleClaimType;
        //    ////var roles = claims.Where(c => c.Type == roleClaimType).ToList();
        //    //string[] roles = claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray();

        //    ////Create general prrincipal and assign it to current request
        //    //var customPrincipal = new System.Security.Principal.GenericPrincipal(userIdentity, roles);
        //    //Thread.CurrentPrincipal = Context.User = customPrincipal;
        //}

        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    bool isAjaxCall = string.Equals("XMLHttpRequest", HttpContext.Current.Request.Headers["x-requested-with"], StringComparison.OrdinalIgnoreCase);
        //    if (!isAjaxCall)
        //    {
        //        _appService.HandleError();
        //    }
        //}
    }
}
