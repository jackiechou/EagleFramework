using System.Web.Mvc;

namespace Eagle.WebApp.Attributes.ThemeLayout
{
    public class PageTitleAttribute : ActionFilterAttribute
    {
        private string PageTitle { get; }
    
        public PageTitleAttribute(string pageTitle)
        {
            PageTitle = pageTitle;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            var result = filterContext.Result as ViewResult;
            if (result != null)
            {
                result.ViewBag.Title = PageTitle;
            }
        }
    }
}