using System;
using System.IO;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Eagle.Common.Utilities;

namespace Eagle.Core.HtmlHelper
{
    public static class ImageHtmlHelper
    {
        #region Image =====================================================================================================
        public static string Image(this System.Web.Mvc.HtmlHelper helper, string url, object htmlAttributes)
        {
            string fileNameWithoutExtension;
            if (string.IsNullOrEmpty(url))
            {
                url = "/Images/no-image.png";
                fileNameWithoutExtension = "no_image";
            }
            else
                fileNameWithoutExtension = Path.GetFileNameWithoutExtension(url);

            TagBuilder builder = new TagBuilder("img");
            builder.Attributes.Add("src", url);
            builder.Attributes.Add("alt", fileNameWithoutExtension);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return builder.ToString(TagRenderMode.SelfClosing);
        }

        public static string Image(this System.Web.Mvc.HtmlHelper helper, string propertyName, string url, string altText, object htmlAttributes)
        {
            string fileNameWithoutExtension;
            if (string.IsNullOrEmpty(url))
            {
                url = "/Images/no-image.png";
                fileNameWithoutExtension = "no_image";
            }
            else
                fileNameWithoutExtension = Path.GetFileNameWithoutExtension(url);
            string strAltText = string.IsNullOrEmpty(altText) ? fileNameWithoutExtension : altText;

            TagBuilder builder = new TagBuilder("img");
            builder.Attributes.Add("id", propertyName);
            builder.Attributes.Add("src", url);
            builder.Attributes.Add("alt", strAltText);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return builder.ToString(TagRenderMode.SelfClosing);
        }
        public static string ImageLink(this System.Web.Mvc.HtmlHelper helper, string imageUrl, string alternateText, object imageHtmlAttributes, string actionName, object routeValues, object linkHtmlAttributes)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var url = urlHelper.Action(actionName, routeValues);

            // Create link
            var linkTagBuilder = new TagBuilder("a");
            linkTagBuilder.MergeAttribute("href", url);
            linkTagBuilder.MergeAttributes(new RouteValueDictionary(linkHtmlAttributes));

            // Create image
            var imageTagBuilder = new TagBuilder("img");
            imageTagBuilder.MergeAttribute("src", urlHelper.Content(imageUrl));
            imageTagBuilder.MergeAttribute("alt", urlHelper.Encode(alternateText));
            imageTagBuilder.MergeAttributes(new RouteValueDictionary(imageHtmlAttributes));

            // Add image to link
            linkTagBuilder.InnerHtml = imageTagBuilder.ToString(TagRenderMode.SelfClosing);

            return linkTagBuilder.ToString();
        }

        public static MvcHtmlString Image(this System.Web.Mvc.HtmlHelper helper, string propertyName, string dirPath, string fileName, string altText, object htmlAttributes = null)
        {
            string filePath, fileNameWithoutExtension;
            if (!string.IsNullOrEmpty(fileName))
            {
                filePath = "/" + dirPath + "/" + fileName;
                fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            }
            else
            {
                filePath = "/Images/no-image.png";
                fileNameWithoutExtension = "no_image";
            }

            string strAltText = string.IsNullOrEmpty(altText) ? fileNameWithoutExtension : altText;
            TagBuilder img = new TagBuilder("img");
            img.Attributes.Add("id", propertyName);
            img.Attributes.Add("src", filePath);
            img.Attributes.Add("alt", strAltText);
            if (htmlAttributes != null)
            {
                var attributes = System.Web.Mvc.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                img.MergeAttributes(attributes, true);
            }
            return new MvcHtmlString(img.ToString());
        }

        public static MvcHtmlString ImageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var data = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string propertyName = data.PropertyName;
            var imgUrl = expression.Compile()(htmlHelper.ViewData.Model).ToString();
            TagBuilder tag = new TagBuilder("img");
            tag.Attributes.Add("id", propertyName);
            tag.Attributes.Add("src", imgUrl);
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString ImageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var data = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string propertyName = data.PropertyName;
            var imgUrl = expression.Compile()(htmlHelper.ViewData.Model).ToString();
            TagBuilder tag = new TagBuilder("img");
            tag.Attributes.Add("id", propertyName);
            tag.Attributes.Add("src", imgUrl);
            if (htmlAttributes != null)
                tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
        public static MvcHtmlString ImageFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string dirPath, string fileName, object htmlAttributes = null)
        {
            var data = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            string propertyName = data.PropertyName;
            string filePath, fileNameWithoutExtension;
            if (!string.IsNullOrEmpty(fileName))
            {
                filePath = "/" + dirPath + "/" + fileName;
                fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                string physicalPath = HttpContext.Current.Server.MapPath(filePath);
                if (File.Exists(physicalPath))
                    filePath = "/Images/no-image.png";
            }
            else
            {
                filePath = "/Images/no-image.png";
                fileNameWithoutExtension = "no_image";
            }

            TagBuilder img = new TagBuilder("img");
            img.Attributes.Add("id", propertyName);
            img.Attributes.Add("src", filePath);
            img.Attributes.Add("alt", fileNameWithoutExtension);

            if (htmlAttributes != null)
            {
                var attributes = System.Web.Mvc.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                img.MergeAttributes(attributes, true);
            }
            return new MvcHtmlString(img.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString ImageByFileId(this System.Web.Mvc.HtmlHelper helper, string propertyName, int? fileId, string filePath, string altText, object htmlAttributes = null)
        {
            filePath = filePath ?? "/Images/no-image.png";
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            string strAltText = altText ?? StringUtils.ConvertTitle2Alias(fileNameWithoutExtension);
            TagBuilder img = new TagBuilder("img");
            img.Attributes.Add("id", propertyName);
            img.Attributes.Add("src", filePath);
            img.Attributes.Add("alt", strAltText);
            if (htmlAttributes != null)
            {
                var attributes = System.Web.Mvc.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                img.MergeAttributes(attributes, true);
            }
            return new MvcHtmlString(img.ToString());
        }
        #endregion ========================================================================================================

    }
}