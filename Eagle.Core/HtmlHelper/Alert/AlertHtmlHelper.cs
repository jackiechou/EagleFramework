using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Eagle.Core.HtmlHelper.Alert
{
    /// <summary>
    /// Generates an Alert message
    /// </summary>
    public static class AlertHtmlHelper
    {
        /// <summary>
        /// Generates an Alert message
        /// </summary>
        public static AlertBox Alert(this System.Web.Mvc.HtmlHelper html, 
            string text,
            AlertType alertStyle = AlertType.Default,
            bool hideCloseButton = false,
            object htmlAttributes = null
            )
        {
            return new AlertBox(text, alertStyle, hideCloseButton, htmlAttributes);
        }

        // Strongly typed
        /// <summary>
        /// Generates an Alert message
        /// </summary>
        public static AlertBox AlertFor<TModel, TTextProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TTextProperty>> expression,
            AlertType alertStyle = AlertType.Default,
            bool hideCloseButton = false,
            object htmlAttributes = null
            )
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            return new AlertBox((string)metadata.Model, alertStyle, hideCloseButton, htmlAttributes);
        }

        /// <summary>
        /// Generates an Alert message
        /// </summary>
        public static AlertBox AlertFor<TModel, TTextProperty, TStyleProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TTextProperty>> textExpression,
            Expression<Func<TModel, TStyleProperty>> styleExpression,
            bool hideCloseButton = false,
            object htmlAttributes = null
            )
        {
            var text = (string)ModelMetadata.FromLambdaExpression(textExpression, html.ViewData).Model;
            var alertStyle = (AlertType)ModelMetadata.FromLambdaExpression(styleExpression, html.ViewData).Model;

            return new AlertBox(text, alertStyle, hideCloseButton, htmlAttributes);
        }

      
    }
}
        