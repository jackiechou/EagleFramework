using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Repositories;
using Eagle.Services.SystemManagement;

namespace Eagle.Services.Skins
{
    public class ThemeViewEngine : RazorViewEngine
    {
        private ICacheService CacheService { get; }
        private IDocumentService DocumentService { get;  }
        private IThemeService ThemeService { get; set; }

        //Desktop
        public static string DesktopMasterLayout { get; set; }
        public static string DesktopMainLayout { get; set; }
        public static string DesktopSubLayout { get; set; }

        //Admin
        public static string AdminAjaxLayout { get; set; }
        public static string AdminMasterLayout { get; set; }
        public static string AdminSharedLayout { get; set; }
        public static string AdminMainLayout { get; set; }
        public static string AdminFullMainLayout { get; set; }
        public static string AdminPopUpLayout { get; set; }
        public static string AdminReportLayout { get; set; }
        public static string AdminFormLayout { get; set; }
        public static string AdminLoginLayout { get; set; }
        public static string AdminDefaultLayout { get; set; }

        public static string ErrorLayout { get; set; }

        public ThemeViewEngine(string themeName)
        {
            IUnitOfWork unitOfWork = new UnitOfWork();
            DocumentService = new DocumentService(unitOfWork);
            CacheService = new CacheService(unitOfWork);
            ThemeService = new ThemeService(unitOfWork, CacheService, DocumentService);

            #region Desktop 
            DesktopMasterLayout = "~/Themes/" + themeName + "/Views/Shared/DesktopLayouts/DesktopMasterLayout.cshtml";
            DesktopMainLayout = "~/Themes/" + themeName + "/Views/Shared/DesktopLayouts/MainLayout.cshtml";
            DesktopSubLayout = "~/Themes/" + themeName + "/Views/Shared/DesktopLayouts/SubLayout.cshtml";

            AdminSharedLayout = "~/Themes/" + themeName + "/Views/Shared/AdminLayouts/SharedLayout.cshtml";
            AdminMasterLayout = "~/Themes/" + themeName + "/Views/Shared/AdminLayouts/MasterLayout.cshtml";
            AdminAjaxLayout = "~/Themes/" + themeName + "/Views/Shared/AdminLayouts/AjaxLayout.cshtml";
            AdminMainLayout = "~/Themes/" + themeName + "/Views/Shared/AdminLayouts/MainLayout.cshtml";
            AdminFullMainLayout = "~/Themes/" + themeName + "/Views/Shared/AdminLayouts/FullMainLayout.cshtml";
            AdminPopUpLayout = "~/Themes/" + themeName + "/Views/Shared/AdminLayouts/PopUpLayout.cshtml";
            AdminReportLayout = "~/Themes/" + themeName + "/Views/Shared/AdminLayouts/ReportLayout.cshtml";
            AdminFormLayout = "~/Themes/" + themeName + "/Views/Shared/AdminLayouts/FormLayout.cshtml";
            AdminLoginLayout = "~/Themes/" + themeName + "/Views/Shared/AdminLayouts/LoginLayout.cshtml";

            ErrorLayout = "~/Views/Shared/LayoutError.cshtml";

            MasterLocationFormats = new[] {
                   "~/Themes/" + themeName + "/Views/{0}.cshtml",
                   "~/Themes/" + themeName + "/Views/{1}/{0}.cshtml",
                   "~/Themes/" + themeName + "/Views/Shared/DesktopLayouts/{0}.cshtml",
                   "~/Themes/" + themeName + "/Views/Shared/AdminLayouts/{0}.cshtml"
                };

            ViewLocationFormats = new[] {
                    "~/Views/{0}.cshtml",
                    "~/Views/{1}/{0}.cshtml",
                    //"~/Views/" + themeName + "/{1}/{0}.cshtml",
                    "~/Views/Shared/{0}.cshtml",
                    "~/Views/Shared/{1}/{0}.cshtml",
                   // "~/Views/Shared/" + themeName + "/{0}.cshtml",
                    "~/Areas/Admin/Views/{1}/{0}.cshtml",
                    "~/Areas/Admin/Views/Shared/{0}.cshtml"
                };

            PartialViewLocationFormats = new[] {
                    "~/Views/{0}.cshtml",
                    "~/Views/{1}/{0}.cshtml",
                   //  "~/Views/" + themeName + "/{1}/{0}.cshtml",
                    "~/Views/Shared/{0}.cshtml",
                    "~/Views/Shared/{1}/{0}.cshtml",
                   //  "~/Views/Shared/" + themeName + "/{0}.cshtml",
                    "~/Areas/Admin/Views/{1}/{0}.cshtml",
                    "~/Areas/Admin/Views/Shared/{0}.cshtml"
                };

            #endregion
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (string.IsNullOrEmpty(viewName))
            {
                throw new ArgumentException("Value is required.", "viewName");
            }

            string themeName = GetThemeToUse(controllerContext);

            string[] searchedViewLocations;
            string[] searchedMasterLocations;

            string controllerName =
              controllerContext.RouteData.GetRequiredString("controller");

            string viewPath = GetViewPath(ViewLocationFormats, viewName,
                              controllerName, out searchedViewLocations);
            string masterPath = GetMasterPath(MasterLocationFormats, viewName,
                                controllerName, themeName, out searchedMasterLocations);

            if (!(string.IsNullOrEmpty(viewPath)) &&
               (masterPath != string.Empty || string.IsNullOrEmpty(masterName)))
            {
                return new ViewEngineResult(
                    (CreateView(controllerContext, viewPath, masterPath)), this);
            }
            return new ViewEngineResult(
              searchedViewLocations.Union(searchedMasterLocations));
        }

        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (string.IsNullOrEmpty(partialViewName))
            {
                throw new ArgumentException("Value is required.", partialViewName);
            }

            //string themeName = GetThemeToUse(controllerContext);

            string[] searchedLocations;

            string controllerName = controllerContext.RouteData.GetRequiredString("controller");

            string partialPath = GetViewPath(PartialViewLocationFormats,
                                 partialViewName, controllerName, out searchedLocations);

            if (string.IsNullOrEmpty(partialPath))
            {
                return new ViewEngineResult(searchedLocations);
            }
            return new ViewEngineResult(CreatePartialView(controllerContext,
                                        partialPath), this);
        }

        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            try
            {
                return File.Exists(controllerContext.HttpContext.Server.MapPath(virtualPath));
            }
            catch (HttpException exception)
            {
                if (exception.GetHttpCode() != 0x194)
                {
                    throw;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            string[] strArray;
            string[] strArray2;

            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (string.IsNullOrEmpty(viewName))
            {
                throw new ArgumentException("viewName must be specified.", "viewName");
            }

            var themeName = GetThemeToUse(controllerContext);

            var requiredString = controllerContext.RouteData.GetRequiredString("controller");

            var viewPath = GetPath(controllerContext, ViewLocationFormats, "ViewLocationFormats", viewName, themeName, requiredString, "View", useCache, out strArray);
            var masterPath = GetPath(controllerContext, MasterLocationFormats, "MasterLocationFormats", masterName, themeName, requiredString, "Master", useCache, out strArray2);

            if (!string.IsNullOrEmpty(viewPath) && (!string.IsNullOrEmpty(masterPath) || string.IsNullOrEmpty(masterName)))
            {
                return new ViewEngineResult(CreateView(controllerContext, viewPath, masterPath), this);
            }
            return new ViewEngineResult(strArray.Union(strArray2));
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            string[] strArray;
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (string.IsNullOrEmpty(partialViewName))
            {
                throw new ArgumentException("partialViewName must be specified.", "partialViewName");
            }

            var themeName = GetThemeToUse(controllerContext);

            var requiredString = controllerContext.RouteData.GetRequiredString("controller");
            var partialViewPath = GetPath(controllerContext, PartialViewLocationFormats, "PartialViewLocationFormats", partialViewName, themeName, requiredString, "Partial", useCache, out strArray);
            return string.IsNullOrEmpty(partialViewPath) ? new ViewEngineResult(strArray) : new ViewEngineResult(CreatePartialView(controllerContext, partialViewPath), this);
        }

        private string GetThemeToUse(ControllerContext controllerContext)
        {
            string themeName;
            if (controllerContext.HttpContext.Items[CacheKeySetting.ThemeName]!=null)
            {
                themeName = controllerContext.HttpContext.Items[CacheKeySetting.ThemeName].ToString();
            }
            else
            {
                themeName = CacheService.Get<string>(CacheKeySetting.ThemeName);
                if (string.IsNullOrEmpty(themeName))
                {
                    var applicationId = GlobalSettings.DefaultApplicationId;
                    var theme = ThemeService.GetSelectedTheme(applicationId);
                    if (theme != null)
                    {
                        controllerContext.HttpContext.Items[CacheKeySetting.ThemeName] = theme.PackageName;
                        controllerContext.HttpContext.Items[CacheKeySetting.ThemeSrc] = theme.PackageSrc;
                    }
                }
            }
            return themeName;
        }

        private static readonly string[] EmptyLocations = null;

        private string GetViewPath(string[] locations, string viewName,
                   string controllerName, out string[] searchedLocations)
        {
            searchedLocations = new string[locations.Length];

            for (int i = 0; i < locations.Length; i++)
            {
                var path = string.Format(CultureInfo.InvariantCulture, locations[i], viewName, controllerName);
                if (VirtualPathProvider.FileExists(path))
                {
                    searchedLocations = new string[0];
                    return path;
                }
                searchedLocations[i] = path;
            }
            return null;
        }

        private string GetMasterPath(string[] locations, string viewName,
                       string controllerName, string themeName, out string[] searchedLocations)
        {
            searchedLocations = new string[locations.Length];

            for (int i = 0; i < locations.Length; i++)
            {
                var path = string.Format(CultureInfo.InvariantCulture, locations[i], viewName, controllerName, themeName);
                if (VirtualPathProvider.FileExists(path))
                {
                    searchedLocations = new string[0];
                    return path;
                }
                searchedLocations[i] = path;
            }
            return null;
        }
        private string GetPath(ControllerContext controllerContext, string[] locations, string locationsPropertyName, string name, string themeName, string controllerName, string cacheKeyPrefix, bool useCache, out string[] searchedLocations)
        {
            searchedLocations = EmptyLocations;
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }
            if ((locations == null) || (locations.Length == 0))
            {
                throw new InvalidOperationException("locations must not be null or emtpy.");
            }

            bool flag = IsSpecificPath(name);
            string key = CreateCacheKey(cacheKeyPrefix, name, flag ? string.Empty : controllerName, themeName);
            if (useCache)
            {
                var viewLocation = ViewLocationCache.GetViewLocation(controllerContext.HttpContext, key);
                if (viewLocation != null)
                {
                    return viewLocation;
                }
            }
            return !flag ? GetPathFromGeneralName(controllerContext, locations, name, controllerName, themeName, key, ref searchedLocations)
                        : GetPathFromSpecificName(controllerContext, name, key, ref searchedLocations);
        }

        private static bool IsSpecificPath(string name)
        {
            var ch = name[0];
            if (ch != '~')
            {
                return (ch == '/');
            }
            return true;
        }

        private string CreateCacheKey(string prefix, string name, string controllerName, string themeName)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                ":ViewCacheEntry:{0}:{1}:{2}:{3}:{4}",
                new object[] { GetType().AssemblyQualifiedName, prefix, name, controllerName, themeName });
        }

        private string GetPathFromGeneralName(ControllerContext controllerContext, string[] locations, string name, string controllerName, string themeName, string cacheKey, ref string[] searchedLocations)
        {
            var virtualPath = string.Empty;
            searchedLocations = new string[locations.Length];
            for (var i = 0; i < locations.Length; i++)
            {
                var str2 = string.Format(CultureInfo.InvariantCulture, locations[i], new object[] { name, controllerName, themeName });

                if (FileExists(controllerContext, str2))
                {
                    searchedLocations = EmptyLocations;
                    virtualPath = str2;
                    ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, virtualPath);
                    return virtualPath;
                }
                searchedLocations[i] = str2;
            }
            return virtualPath;
        }

        private string GetPathFromSpecificName(ControllerContext controllerContext, string name, string cacheKey, ref string[] searchedLocations)
        {
            var virtualPath = name;
            if (!FileExists(controllerContext, name))
            {
                virtualPath = string.Empty;
                searchedLocations = new[] { name };
            }
            ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, virtualPath);
            return virtualPath;
        }
    }
}