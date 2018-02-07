using System.Resources;
using System.Web.Mvc;
using Eagle.Core.UI.Layout;
using Eagle.Resources;

namespace Eagle.WebApp.Attributes.ThemeLayout
{
    public class PageLayoutAttribute: ActionFilterAttribute
    {
        public LayoutSetting LayoutSetting { get; set; }
        public string PageTitle { get; set; }
        public PageLayoutAttribute(LayoutSetting layoutSetting, string pageTitle=null)
        {
            LayoutSetting = layoutSetting;
            PageTitle = pageTitle;
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            var viewResult = filterContext.Result as ViewResult;
            var request = filterContext.HttpContext.Request;
            var routeData = filterContext.Controller.ControllerContext.RouteData;

            string layoutName =string.Empty;
            if (viewResult != null)
            {
                if (PageTitle != null)
                {
                    ResourceManager resourceManager = new ResourceManager(typeof(LanguageResource));
                    viewResult.ViewBag.Title = resourceManager.GetString(PageTitle);
                }
                else
                {
                    viewResult.ViewBag.Title = routeData.Values["controller"].ToString();
                }

                if (!request.IsAjaxRequest())
                {
                    switch (LayoutSetting)
                    {
                        case LayoutSetting.FullMainLayout:
                            layoutName = LayoutType.FullMainLayout;
                            break;
                        case LayoutSetting.MainLayout:
                            layoutName = LayoutType.MainLayout;
                            break;
                    }

                    viewResult.MasterName = layoutName;
                }
            }
        }
    }
}
