using System.Web.Mvc;

namespace Eagle.Repositories.Themes
{
    public class LayoutAttribute: ActionFilterAttribute
    {
        private readonly string _masterName;

        public LayoutAttribute(string masterName)
        {
            _masterName = masterName;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            var result = filterContext.Result as ViewResult;
            var request = filterContext.HttpContext.Request;
            if (request.IsAjaxRequest())
            {
                result.MasterName = null;
            }
            else
            {               
                if (result != null)
                    result.MasterName = _masterName;               
            }
           
        }
    }
}
