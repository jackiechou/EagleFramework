using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Eagle.Common.Extensions
{
    public class DateTimeModelBinder : IModelBinder
    {

        #region IModelBinder Members
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {

            if (controllerContext.HttpContext.Request.HttpMethod == "GET")
            {
                string theDate = controllerContext.HttpContext.Request.Form[bindingContext.ModelName];
                DateTime dt = new DateTime();
                //bool success = DateTime.TryParse(theDate, System.Globalization.CultureInfo.CurrentUICulture, System.Globalization.DateTimeStyles.None, out dt);
                bool success = DateTime.TryParse(theDate, System.Globalization.CultureInfo.GetCultureInfo("en-AU"), System.Globalization.DateTimeStyles.None, out dt);
                if (success)
                {
                    return dt;
                }
                else
                {
                    return null;
                }
            }

            DefaultModelBinder binder = new DefaultModelBinder();
            return binder.BindModel(controllerContext, bindingContext);

        }
        #endregion
    }
}
