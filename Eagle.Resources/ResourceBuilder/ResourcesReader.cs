using System.Web;

namespace Eagle.Resources.ResourceBuilder
{
    public static class ResourcesReader
    {
        public static string GetString(string key)
        {
            var globalResourceObject = HttpContext.GetGlobalResourceObject("Eagle.Resources", key);
            return globalResourceObject != null ? globalResourceObject.ToString() : string.Empty;
        }
    }
}
