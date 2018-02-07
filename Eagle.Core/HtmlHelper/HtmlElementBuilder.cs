using System.Web.Mvc;

namespace Eagle.Core.HtmlHelper
{
    public abstract class HtmlElementBuilder
    {
        protected System.Web.Mvc.HtmlHelper htmlHelper;
        protected object htmlAttributes;

        public HtmlElementBuilder(System.Web.Mvc.HtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        public System.Web.Mvc.HtmlHelper HtmlHelper { get { return this.htmlHelper; } }

        internal HtmlElementBuilder HtmlAttributesCore(object htmlAttributes)
        {
            this.htmlAttributes = htmlAttributes;

            return this;
        }

        public abstract MvcHtmlString Build();

        public MvcHtmlString ToHtmlString()
        {
            return this.Build();
        }

        //public override string ToString()
        //{
        //    return this.Build().ToHtmlString();
        //}        
    }

    public static class HtmlElementBuilderExtensions
    {
        public static TBuilder HtmlAttributes<TBuilder>(this TBuilder builder, object htmlAttributes)
            where TBuilder : HtmlElementBuilder
        {
            return (TBuilder)builder.HtmlAttributesCore(htmlAttributes);
        }
    }
}
