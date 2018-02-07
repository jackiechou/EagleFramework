namespace Eagle.Core.HtmlHelper.Alert
{
    public class AlertBoxFluentOptions : IAlertBoxFluentOptions
    {
        private readonly AlertBox _parent;

        public AlertBoxFluentOptions(AlertBox parent)
        {
            _parent = parent;
        }

        public IAlertBoxFluentOptions HideCloseButton(bool hideCloseButton = true)
        {
            return _parent.HideCloseButton(hideCloseButton);
        }

        public IAlertBoxFluentOptions Attributes(object htmlAttributes)
        {
            return _parent.Attributes(htmlAttributes);
        }

        public override string ToString()
        {
            return _parent.ToString();
        }

        public string ToHtmlString()
        {
            return ToString();
        }
    }
}
