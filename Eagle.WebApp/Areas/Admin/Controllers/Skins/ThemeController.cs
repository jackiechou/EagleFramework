using System.Configuration;
using System.Web;
using System.Web.Mvc;
using Eagle.Services;
using Eagle.Services.Skins;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Skins
{
    public class ThemeController : BaseController
    {
        private IThemeService ThemeService { get; set; }

        public ThemeController(IThemeService themeService) : base(new IBaseService[] { themeService })
        {
            ThemeService = themeService;
        }

        //
        // GET: /Admin/Theme/

        [HttpPost]
        public ActionResult Index(string selectedTheme)
        {
            var themeName = ConfigurationManager.AppSettings["ThemeName"];

            ControllerContext.RequestContext.HttpContext.Items[themeName] = selectedTheme;
            var themeCookie = new HttpCookie("theme", selectedTheme);
            HttpContext.Response.Cookies.Add(themeCookie);

            ////const string controller = "Home";
            ////const string action = "Index";

            ////ViewContext.RequestContext.HttpContext.Items[ThemeName].ToString()
            //ViewBag.Theme = _themeService.PopulateActiveSkinSelectList(GlobalSettings.ApplicationId, selectedTheme, false);
            ////return Redirect(string.Format("/{0}/{1}/{2}", controller, action, SelectedTheme));
            return View("../Skins/Themes/Index");
        }

        #region Dispose

        private bool _disposed;

        [NonAction]
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    ThemeService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
