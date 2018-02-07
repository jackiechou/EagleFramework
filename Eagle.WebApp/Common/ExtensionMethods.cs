using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using Eagle.Services.Dtos.Common;

namespace Eagle.WebApp.Common
{
    public static class ExtensionMethods
    {
        public static async Task<ActionResult> InvokeAsync(this MethodInfo @this, object obj, params object[] parameters)
        {
            dynamic awaitable = @this.Invoke(obj, parameters);
            await awaitable;
            return awaitable.GetAwaiter().GetResult();
        }

        public static async Task<FormActionResultModel> InvokeFormAsync(this MethodInfo @this, object obj, params object[] parameters)
        {
            dynamic awaitable = @this.Invoke(obj, parameters);
            await awaitable;
            return awaitable.GetAwaiter().GetResult();
        }

        public static async Task<dynamic> InvokeDynamicAsync(this MethodInfo @this, object obj, params object[] parameters)
        {
            dynamic awaitable = @this.Invoke(obj, parameters);
            await awaitable;
            return awaitable.GetAwaiter().GetResult();
        }
    }
}
