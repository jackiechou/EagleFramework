using System.Web;

namespace Eagle.Core.HtmlHelper.Alert
{
    public interface IAlertBoxFluentOptions : IHtmlString
    {
        IAlertBoxFluentOptions HideCloseButton(bool hideCloseButton = true);
        IAlertBoxFluentOptions Attributes(object htmlAttributes);
    }
}