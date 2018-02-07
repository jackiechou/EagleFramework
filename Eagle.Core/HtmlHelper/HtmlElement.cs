using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Eagle.Core.HtmlHelper
{
    public class HtmlElement
    {
        public HtmlElement(string tagName)
        {
            this.TagName = tagName;
        }

        public string TagName { get; private set; }

        public string BuildOpenTag(object htmlAttributes)
        {
            return String.Format("<{0}{1}>", this.TagName, BuildHtmlAttributes(htmlAttributes));
        }

        public string OpenTag
        {
            get { return String.Format("<{0}>", this.TagName); }
        }

        public string ClosingTag
        {
            get { return String.Format("</{0}>", this.TagName); }
        }

        public string BuildSelfClosingTag(object htmlAttributes)
        {
            return String.Format("<{0}{1}/>", this.TagName, BuildHtmlAttributes(htmlAttributes));
        }

        internal static MvcHtmlString BuildHtmlAttributes(object htmlAttributes)
        {
            StringBuilder attributes = new StringBuilder();

            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(htmlAttributes).Cast<PropertyDescriptor>())
            {
                attributes.Append(String.Format(" {0}=\"{1}\"", pd.Name, pd.GetValue(htmlAttributes)));
            }

            return MvcHtmlString.Create(attributes.ToString());
        }

        public static readonly HtmlElement Table = new HtmlElement("table");
        public static readonly HtmlElement Tr = new HtmlElement("tr");
        public static readonly HtmlElement Td = new HtmlElement("td");
    }
}
