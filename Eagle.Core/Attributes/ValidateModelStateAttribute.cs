using System.Linq;
using System.Web.Mvc;

namespace Eagle.Core.Attributes
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var viewData = filterContext.Controller.ViewData;

            if (!viewData.ModelState.IsValid)
            {
                var errors = viewData.ModelState.Values.SelectMany(v => v.Errors);
                string message = errors.Aggregate(string.Empty, (current, modelError) => current + (modelError.ErrorMessage + "<br/>"));
                viewData.ModelState.AddModelError("", message);
                filterContext.Controller.ViewBag.Message = message;

                filterContext.Result = new ViewResult
                {
                    ViewData = viewData,
                    TempData = filterContext.Controller.TempData
                };
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
