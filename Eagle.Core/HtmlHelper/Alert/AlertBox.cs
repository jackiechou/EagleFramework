using System.Web.Mvc;

namespace Eagle.Core.HtmlHelper.Alert
{
    public class AlertBox : IAlertBox
    {
        private readonly string _text;

        private AlertType _alertType;

        private bool _hideCloseButton;

        private object _htmlAttributes;

        /// <summary>
        /// Returns a div alert box element with the options specified
        /// </summary>
        /// <param name="text">Sets the text to display</param>
        /// <param name="type">Sets style of alert box [Default | Success | Error | Warning | Info ]</param>
        /// <param name="hideCloseButton">Sets the close button visibility</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        public AlertBox(string text, AlertType type, bool hideCloseButton = false, object htmlAttributes = null)
        {
            _text = text;
            _alertType = type;
            _hideCloseButton = hideCloseButton;
            _htmlAttributes = htmlAttributes;
        }

        #region FluentAPI

        /// <summary>
        /// Sets the display style to Success
        /// </summary>
        public IAlertBoxFluentOptions Success()
        {
            _alertType = AlertType.Success;
            return new AlertBoxFluentOptions(this);
        }

        /// <summary>
        /// Sets the display style to Error
        /// </summary>
        /// <returns></returns>
        public IAlertBoxFluentOptions Error()
        {
            _alertType = AlertType.Error;
            return new AlertBoxFluentOptions(this);
        }

        /// <summary>
        /// Sets the display style to Warning
        /// </summary>
        /// <returns></returns>
        public IAlertBoxFluentOptions Warning()
        {
            _alertType = AlertType.Warning;
            return new AlertBoxFluentOptions(this);
        }

        /// <summary>
        /// Sets the display style to Info
        /// </summary>
        /// <returns></returns>
        public IAlertBoxFluentOptions Info()
        {
            _alertType = AlertType.Info;
            return new AlertBoxFluentOptions(this);
        }
        
        /// <summary>
        /// Sets the close button visibility
        /// </summary>
        /// <returns></returns>
        public IAlertBoxFluentOptions HideCloseButton(bool hideCloseButton = true)
        {
            _hideCloseButton = hideCloseButton;
            return new AlertBoxFluentOptions(this);
        }

        /// <summary>
        /// An object that contains the HTML attributes to set for the element.
        /// </summary>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public IAlertBoxFluentOptions Attributes(object htmlAttributes)
        {
            _htmlAttributes = htmlAttributes;
            return new AlertBoxFluentOptions(this);
        }
        #endregion //FluentAPI

        private string RenderAlert()
        {
            //<div class="alert-box">
            var wrapper = new TagBuilder("div");
            //merge attributes
            wrapper.MergeAttributes(_htmlAttributes != null ? System.Web.Mvc.HtmlHelper.AnonymousObjectToHtmlAttributes(_htmlAttributes) : null);

            if (_alertType != AlertType.Default)
                wrapper.AddCssClass(_alertType.ToString().ToLower());
            wrapper.AddCssClass("alert-box");


            //build html
            wrapper.InnerHtml = _text;

            //Add close button
            if (!_hideCloseButton)
                wrapper.InnerHtml += RenderCloseButton();

            return wrapper.ToString();
        }

        private static TagBuilder RenderCloseButton()
        {
            //<a href="" class="close">x</a>
            var closeButton = new TagBuilder("a");
            closeButton.AddCssClass("close");
            closeButton.Attributes.Add("href", "");
            closeButton.InnerHtml = "×";
            return closeButton;
        }

        //Render HTML
        public override string ToString()
        {
            return RenderAlert();
        }

        //Return ToString
        public string ToHtmlString()
        {
            return ToString();
        }
    }
}