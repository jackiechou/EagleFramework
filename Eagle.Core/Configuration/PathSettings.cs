using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Core.Configuration
{
    public class PathSettings
    {
        public const string RootClientSitePath = "/";

        public static bool IsClientSitePath(string virtualPath)
        {
            return
                virtualPath.StartsWith(RootClientSitePath) ||
                virtualPath.StartsWith("~" + RootClientSitePath);
        }

        public static bool IsVirtualPath(string virtualPath)
        {
            return virtualPath.Contains("~");
        }

        public static string Join(params object[] segmentPaths)
        {
            var path = string.Join("/", segmentPaths);
            return path.Replace("\\", "/").Replace("//", "/");
        }

        public static string VirtualPath(string path)
        {
            if (path.StartsWith("/"))
                path = path.Insert(0, "~");
            else if (path.StartsWith("~") && !path.StartsWith("~/"))
                path = path.Insert(1, "/");

            return path;
        }

        public static string RazorViewName(string viewName)
        {
            if (!viewName.EndsWith(".cshtml"))
                viewName = viewName + ".cshtml";

            return viewName;
        }

        public static string GlobalClientSitePath()
        {
            return Join(VirtualPath(RootClientSitePath), "global");
        }

        public static string GlobalThemePath()
        {
            return Join(GlobalClientSitePath(), "theme");
        }

        public static string GlobalModulePath()
        {
            return Join(GlobalClientSitePath(), "modules");
        }

        public static string GlobalScriptsPath()
        {
            return Join(GlobalThemePath(), "scripts");
        }

        public static string GlobalStylesPath()
        {
            return Join(GlobalThemePath(), "styles");
        }

        public static string GlobalFontsPath()
        {
            return Join(GlobalThemePath(), "fonts");
        }

        public static string GlobalCustomScriptsPath()
        {
            return Join(GlobalThemePath(), "customscripts");
        }

        public static string GlobalImagesPath()
        {
            return Join(GlobalThemePath(), "images");
        }

        public static string GlobalViewPath()
        {
            return Join(GlobalClientSitePath(), "views");
        }

        public static string GlobalViewPath(string viewName)
        {
            return Join(GlobalViewPath(), RazorViewName(viewName));
        }


        public static string ClientSitePath(string websiteId)
        {
            return Join(VirtualPath(RootClientSitePath), websiteId);
            //return string.Format("{0}/{1}/Clients/{2}", SMConfig.BlobStorageUrl, SMConfig.Environment, websiteID);
            //return "";
        }

        public static string ThemePath(string websiteId, string themeName)
        {
            return Join(ClientSitePath(websiteId), themeName);
        }

        public static string ModulePath(string websiteId, string themeName)
        {
            return Join(ClientSitePath(websiteId), themeName, "modules");
        }

        public static string ScriptsPath(string websiteId, string themeName)
        {
            return Join(themeName, "scripts");
        }

        public static string StylesPath(string websiteId, string themeName)
        {
            return Join(themeName, "styles");
        }

        public static string FontsPath(string websiteId, string themeName)
        {
            return Join(themeName, "fonts");
        }

        public static string CustomScriptsPath(string websiteId, string themeName)
        {
            return Join(themeName, "customscripts");
        }

        public static string ImagesPath(string websiteId, string themeName)
        {
            return Join(themeName, "images");
        }
    }
}
