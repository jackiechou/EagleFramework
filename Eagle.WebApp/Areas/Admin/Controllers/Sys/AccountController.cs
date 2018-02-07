using System;
using System.IdentityModel.Services;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Eagle.Common.Utilities;
using Eagle.Core.Configuration;
using Eagle.Core.Cookie;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement.Identity;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Sys
{
    // [Authorize]
    public class AccountController : BaseController
    {
        private IApplicationService ApplicationService { get; set; }
        private ICacheService CacheService { get; set; }
        private IContactService ContactService { get; set; }
        private ICurrencyService CurrencyService { get; set; }
        private ILanguageService LanguageService { get; set; }
        private IRoleService RoleService { get; set; }
        private ISecurityService SecurityService { get; set; }
        private IUserService UserService { get; set; }
        
        public AccountController(IApplicationService applicationService, ICacheService cacheService, ICurrencyService currencyService,
            ILanguageService languageService, IContactService contactService, IUserService userService,
            IRoleService roleService, ISecurityService securityService)
            : base(new IBaseService[] { applicationService, cacheService, currencyService, languageService, contactService, userService, roleService, securityService })
        {
            ApplicationService = applicationService;
            CacheService = cacheService;
            CurrencyService = currencyService;
            LanguageService = languageService;
            ContactService = contactService;
            UserService = userService;
            RoleService = roleService;
            SecurityService = securityService;
        }

        [HttpGet]
        public ActionResult GetProfile()
        {
            var profile = SecurityService.GetUserProfile(UserId);
            return View("../Sys/Account/Profile", profile);
        }

        #region LOGIN - LOG OFF
        [HttpGet]
        public ActionResult Login(string desiredUrl)
        {
            HttpCookie userInfo = Request.Cookies[CookieSetting.UserInfo];
            if (userInfo == null) return View("../Sys/Account/Login");

            string username = userInfo["userName"];
            string password = (!string.IsNullOrEmpty(userInfo["passWord"])) ? StringUtils.DecodePassword(userInfo["passWord"]) : null;

            if (Request.Url != null)
            {
                var url = Request.Url.AbsoluteUri;
                if (url.Contains("?desiredUrl="))
                {
                    url = url.Substring(url.IndexOf("?desiredUrl=", StringComparison.Ordinal) + 11);
                    ViewBag.DesiredUrl = url;
                }
            }

            var loginModel = new LoginModel
            {
                RememberMe = true,
                UserName = username,
                Password = password,
                DesiredUrl = desiredUrl
            };
            return Login(loginModel);
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", LanguageResource.IncorrectUserNamePassword);
                    return View("../Sys/Account/Login", model);
                }
                else
                {
                    string userName = model.UserName;
                    string password = model.Password;
                    string desiredUrl = model.DesiredUrl;
                    bool isPersistent = model.RememberMe;

                    var result = SecurityService.CheckLogin(userName, password);
                    if (!result)
                    {
                        return View("../Sys/Account/Login", model);
                    }

                    var userData = SecurityService.GetUserDetail(userName);
                    var claimPrincipal = SecurityService.CreateUserClaims(userData, desiredUrl, isPersistent);
                    
                    if (!string.IsNullOrEmpty(desiredUrl) && !desiredUrl.Contains("login"))
                    {
                        // return RedirectToLocal(desiredUrl);
                        return Redirect(desiredUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
            }
            catch (ValidationError ex)
            {
                this.ShowException(ex);
                return View("../Sys/Account/Login");
            }
        }

        public ActionResult LogOff(string desiredUrl)
        {
            System.Web.HttpContext.Current.Session.Clear();
            Session.Clear();
            Session.Abandon();

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Request.Cookies.Remove(SettingKeys.UserId);
            Request.Cookies.Remove(SettingKeys.AccountInfo);
            Request.Cookies.Remove(SettingKeys.UserInfo);

            //Remove Cache
            CacheService.Remove(CacheKeySetting.MenuByRole);
            CacheService.Remove(CacheKeySetting.MenuDesktop);

            var sessionAuthenticationModule = FederatedAuthentication.SessionAuthenticationModule;
            sessionAuthenticationModule.CookieHandler.Delete();
            sessionAuthenticationModule.DeleteSessionTokenCookie();
            sessionAuthenticationModule.SignOut();

            FederatedAuthentication.WSFederationAuthenticationModule.SignOut(true);
            FormsAuthentication.SignOut();
            
            return View("../Sys/Account/Login");
        }

        public ActionResult Unauthorized(string desiredUrl=null)
        {
            if (!string.IsNullOrEmpty(desiredUrl))
            {
                return new RedirectResult(desiredUrl);
            }
            return View("../Sys/Account/Login");
        }

        #endregion

        #region RESET PASSWORD

        [HttpGet]
        public ActionResult ResetPassword(string email)
        {
            bool flag = false;
            string message = string.Empty;
            if (!string.IsNullOrEmpty(email))
            {
                string newPassword;
                flag = SecurityService.ResetPassword(email, out newPassword);
                message = (flag) ? LanguageResource.ResetPasswordSuccessfully : LanguageResource.ResetPasswordFailed;
            }
            return Json(JsonUtils.SerializeResult(flag, message), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CHANGE PASSWORD

        [HttpGet]
        public ActionResult ChangePassword()
        {
            try
            {
                var passWord = CurrentClaims.FirstOrDefault(c => c.Type == SettingKeys.Password)?.Value;
                var model = new ChangePasswordModel { UserName = UserName, OldPassword = passWord };
                return PartialView("../Sys/Account/_ChangePassword", model);
            }
            catch (ValidationError ex)
            {
                this.ShowException(ex);
                return PartialView("../Sys/Account/_ChangePassword");
            }
        }

        [HttpPut]
        public ActionResult ChangePassword(ChangePasswordModel entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SecurityService.ChangePassword(entry);
                    return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateSuccess }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var modeStateErrors = ModelState.GetModelStateErrors();
                    return Json(new FailResult { Status = ResultStatusType.Fail, Errors = modeStateErrors }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        //private ActionResult RedirectToLocal(string returnUrl)
        //{
        //    if (Url.IsLocalUrl(returnUrl))
        //    {
        //        return Redirect(returnUrl);
        //    }
        //    return RedirectToAction("Index", "Dashboard");
        //}

        //#region Helpers
        //// Used for XSRF protection when adding external logins
        //private const string XsrfKey = "XsrfId";

        //private IAuthenticationManager AuthenticationManager
        //{
        //    get
        //    {
        //        return HttpContext.GetOwinContext().Authentication;
        //    }
        //}

        //private void AddErrors(IdentityResult result)
        //{
        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError("", error);
        //    }
        //}

        //private ActionResult RedirectToLocal(string returnUrl)
        //{
        //    if (Url.IsLocalUrl(returnUrl))
        //    {
        //        return Redirect(returnUrl);
        //    }
        //    return RedirectToAction("Index", "Home");
        //}

        //internal class ChallengeResult : HttpUnauthorizedResult
        //{
        //    public ChallengeResult(string provider, string redirectUri)
        //        : this(provider, redirectUri, null)
        //    {
        //    }

        //    public ChallengeResult(string provider, string redirectUri, string userId)
        //    {
        //        LoginProvider = provider;
        //        RedirectUri = redirectUri;
        //        UserId = userId;
        //    }

        //    public string LoginProvider { get; set; }
        //    public string RedirectUri { get; set; }
        //    public string UserId { get; set; }

        //    public override void ExecuteResult(ControllerContext context)
        //    {
        //        var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
        //        if (UserId != null)
        //        {
        //            properties.Dictionary[XsrfKey] = UserId;
        //        }
        //        context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
        //    }
        //}
        //#endregion

        #region Dispose

        private bool _disposed;

        [NonAction]
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    ApplicationService = null;
                    CacheService = null;
                    ContactService = null;
                    CurrencyService = null;
                    LanguageService = null;
                    RoleService = null;
                    SecurityService = null;
                    UserService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}