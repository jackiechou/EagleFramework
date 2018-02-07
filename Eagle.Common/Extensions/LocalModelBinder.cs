using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Eagle.Common.Extensions
{
    public class LocalModelBinder : System.Web.Mvc.IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext)
        {
            //var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            //var date = value.ConvertTo(typeof(DateTime), CultureInfo.CreateSpecificCulture("en-AU"));

            //return date;
            string dateString = controllerContext.HttpContext.Request.QueryString[bindingContext.ModelName];
            var dt = new DateTime();
            bool success = DateTime.TryParse(dateString, CultureInfo.GetCultureInfo("en-AU"), DateTimeStyles.None, out dt);
            if (success)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
    }
}
