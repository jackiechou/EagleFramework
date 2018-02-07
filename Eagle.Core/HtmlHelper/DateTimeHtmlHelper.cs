using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Eagle.Core.Configuration;
using Eagle.Resources;

namespace System.Web.Mvc
{
    public static class DatetimeHtmlHelper
    {

        #region Month Picker 
        public static MvcHtmlString MonthPickerNullFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string selectedDate = null, object htmlAttributes = null)
        {
            if (selectedDate == null) throw new ArgumentNullException(nameof(selectedDate));
            // get the metdata
            ModelMetadata metaData = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            // get the field name
            var fieldName = ExpressionHelper.GetExpressionText(expression);

            // get full field (template may have prefix)
            var propertyName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            string ctrlName = propertyName.Replace(".", "");
            string displayName = metaData.DisplayName ?? (metaData.PropertyName ?? ExpressionHelper.GetExpressionText(expression));

            string savedDate, selectedMonthYear;
            DateTime selectDate = new DateTime();

            if (string.IsNullOrEmpty(selectedDate))
            {
                if (metaData.Model != null && metaData.Model.ToString() != "01/01/0001 00:00:00")
                {
                    selectDate = Convert.ToDateTime(metaData.Model);
                    selectedMonthYear = selectDate.ToString("MM/yyyy");
                    savedDate = selectDate.ToString("MM/dd/yyyy");
                }
                else
                {
                    savedDate = string.Empty;
                    selectedMonthYear = string.Empty;
                }
            }
            else
            {
                savedDate = "01/" + selectedDate;
                selectedMonthYear = selectDate.ToString("MM/yyyy");
            }



            TagBuilder input = new TagBuilder("input");
            input.Attributes.Add("id", ctrlName + "Picker");
            input.Attributes.Add("name", ctrlName + "Picker");
            input.Attributes.Add("placeholder", "MM/yyyy");
            input.Attributes.Add("type", "text");
            input.Attributes.Add("value", selectedMonthYear);


            TagBuilder input2 = new TagBuilder("input");
            input2.Attributes.Add("id", propertyName);
            input2.Attributes.Add("name", propertyName);
            input2.Attributes.Add("type", "hidden");
            input2.Attributes.Add("class", "selectedDate");
            input2.Attributes.Add("value", savedDate);

            var classes = new List<string> { "mtz-monthpicker-widgetcontainer", "monthPicker", "span12" };
            var isRequired = metaData.ContainerType.GetProperty(propertyName).GetCustomAttributes(typeof(RequiredAttribute), false).Any();
            if (isRequired)
            {
                input.Attributes.Add("class", string.Join(" ", classes));
                input.Attributes.Add("data-val-required", $"{displayName} is required");
                input.Attributes.Add("data-val", "true");
            }
            else
            {
                input.Attributes.Add("class", string.Join(" ", classes));
            }

            if (htmlAttributes != null)
            {
                var customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                if (customAttributes != null)
                {
                    foreach (KeyValuePair<string, object> customAttribute in customAttributes)
                    {
                        input.MergeAttribute(customAttribute.Key, customAttribute.Value.ToString());
                    }
                }
            }

            string strHtml = input.ToString(TagRenderMode.SelfClosing) + input2.ToString(TagRenderMode.SelfClosing);
            return new MvcHtmlString(strHtml);
        }

        public static MvcHtmlString MonthPickerFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string selectedDate = null, object htmlAttributes = null)
        {
            // get the metdata
            ModelMetadata metaData = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            // get the field name
            var fieldName = ExpressionHelper.GetExpressionText(expression);

            // get full field (template may have prefix)
            var propertyName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            string ctrlName = propertyName.Replace(".", "");
            string displayName = metaData.DisplayName ?? (metaData.PropertyName ?? ExpressionHelper.GetExpressionText(expression));

            string savedDate, selectedMonthYear;
            DateTime selectDate = new DateTime();

            if (string.IsNullOrEmpty(selectedDate))
            {
                if (metaData.Model != null && metaData.Model.ToString() != "01/01/0001 00:00:00")
                {
                    selectDate = Convert.ToDateTime(metaData.Model);
                    selectedMonthYear = selectDate.ToString("MM/yyyy");
                    savedDate = selectDate.ToString("MM/dd/yyyy");
                }
                else
                {
                    DateTime firstDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
                    savedDate = firstDate.ToString("MM/dd/yyyy");
                    selectedMonthYear = firstDate.ToString("MM/yyyy");
                }
            }
            else
            {
                savedDate = "01/" + selectedDate;
                selectedMonthYear = selectDate.ToString("MM/yyyy");
            }



            TagBuilder input = new TagBuilder("input");
            input.Attributes.Add("id", ctrlName + "Picker");
            input.Attributes.Add("name", ctrlName + "Picker");
            input.Attributes.Add("placeholder", "MM/yyyy");
            input.Attributes.Add("type", "text");
            input.Attributes.Add("value", selectedMonthYear);

            TagBuilder input2 = new TagBuilder("input");
            input2.Attributes.Add("id", propertyName);
            input2.Attributes.Add("name", propertyName);
            input2.Attributes.Add("type", "hidden");
            input2.Attributes.Add("class", "selectedDate");
            input2.Attributes.Add("value", savedDate);

            var classes = new List<string> { "mtz-monthpicker-widgetcontainer", "monthPicker", "span12" };
            var isRequired = metaData.ContainerType.GetProperty(propertyName).GetCustomAttributes(typeof(RequiredAttribute), false).Any();
            if (isRequired)
            {
                input.Attributes.Add("class", string.Join(" ", classes));
                input.Attributes.Add("data-val-required", $"{displayName} is required.");
                input.Attributes.Add("data-val", "true");
            }
            else
            {
                input.Attributes.Add("class", string.Join(" ", classes));
            }

            input.Attributes.Add("data-val-data", "The field must be a date.");

            if (htmlAttributes != null)
            {
                var customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                if (customAttributes != null)
                {
                    foreach (KeyValuePair<string, object> customAttribute in customAttributes)
                    {
                        input.MergeAttribute(customAttribute.Key, customAttribute.Value.ToString());
                    }
                }
            }

            string strHtml = input.ToString(TagRenderMode.SelfClosing) + input2.ToString(TagRenderMode.SelfClosing);
            return new MvcHtmlString(strHtml);
        }
        #endregion

        #region Date Picker

        public static MvcHtmlString DateTimePickerFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            string format = GlobalSettings.DefaultDateFormat ?? "dd/MM/yyyy";

            // get the metdata
            ModelMetadata metaData = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            // get the field name
            var fieldName = ExpressionHelper.GetExpressionText(expression);

            // get full field (template may have prefix)
            var propertyName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            string ctrlName = propertyName.Replace(".", "");
            string displayName = metaData.DisplayName ?? (metaData.PropertyName ?? ExpressionHelper.GetExpressionText(expression));
            var classes = new List<string> { "form-control", "input-small", "date-input", "date", "ignore", "ignored", "datepicker" };

            string strSelectedDate = "", savedDate = "";
            if (metaData.Model != null)
            {
                var formatSelectedDate = ((DateTime)metaData.Model).ToString(format);
                if (formatSelectedDate != "01/01/0001")
                {
                    strSelectedDate = formatSelectedDate;
                    savedDate = ((DateTime)metaData.Model).ToString("MM/dd/yyyy");
                }
            }

            TagBuilder div = new TagBuilder("div");
            div.Attributes.Add("class", "input-group input-append date");
            div.Attributes.Add("data-provide", "date-picker");
            div.Attributes.Add("data-id", ctrlName);
            div.Attributes.Add("data-link-field", ctrlName);
            div.Attributes.Add("data-date", strSelectedDate);
            div.Attributes.Add("data-date-format", format);
            div.Attributes.Add("data-link-format", format);


            TagBuilder input = new TagBuilder("input");
            input.Attributes.Add("id", ctrlName + "Picker");
            input.Attributes.Add("name", ctrlName + "Picker");
            input.Attributes.Add("size", "19");
            input.Attributes.Add("placeholder", format);
            input.Attributes.Add("type", "text");
            input.Attributes.Add("readonly", "true");
            input.Attributes.Add("value", strSelectedDate);
            input.Attributes.Add("class", string.Join(" ", classes));

            TagBuilder input2 = new TagBuilder("input");
            input2.Attributes.Add("id", propertyName);
            input2.Attributes.Add("name", propertyName);
            input2.Attributes.Add("type", "hidden");
            input2.Attributes.Add("class", "selectedDate");
            input2.Attributes.Add("value", savedDate);


            if (metaData.ContainerType.GetProperty(propertyName) != null)
            {
                var isRequired = metaData.ContainerType.GetProperty(propertyName).GetCustomAttributes(typeof(RequiredAttribute), false).Any();
                if (isRequired)
                {
                    //input.Attributes.Add("readonly", "readonly");
                    input.Attributes.Add("data-val-required", $"{displayName} is required");
                    input.Attributes.Add("data-val", "true");
                }
            }

            //input.Attributes.Add("data-val-data", LanguageResource.InvalidDate);

            if (htmlAttributes != null)
            {
                var customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                if (customAttributes != null)
                {
                    foreach (KeyValuePair<string, object> customAttribute in customAttributes)
                    {
                        input.MergeAttribute(customAttribute.Key, customAttribute.Value.ToString());
                    }
                }
            }

            div.InnerHtml += input.ToString(TagRenderMode.SelfClosing) + input2.ToString(TagRenderMode.SelfClosing)
                 + "<span class='input-group-addon'><label for=\"" + ctrlName + "Picker\"><span class='glyphicon glyphicon-calendar'></span></label></span>";
            return new MvcHtmlString(div.ToString());
        }

        public static MvcHtmlString DatePicker2For<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            string format = GlobalSettings.DefaultDateFormat ?? "dd/MM/yyyy";

            // get the metdata
            ModelMetadata metaData = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            // get the field name
            var fieldName = ExpressionHelper.GetExpressionText(expression);

            // get full field (template may have prefix)
            var propertyName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            string ctrlName = propertyName.Replace(".", "");
            string displayName = metaData.DisplayName ?? (metaData.PropertyName ?? ExpressionHelper.GetExpressionText(expression));
            var classes = new List<string> { "form-control", "input-small", "date-input", "date", "ignore", "ignored", "datepicker" };

            if (string.IsNullOrEmpty(format))
            {
                format = "dd/MM/yyyy";
            }
            string date = "";
            string localdate = "";
            if (metaData.Model != null)
            {
                date = ((DateTime)metaData.Model).ToString(format);
                localdate = ((DateTime)metaData.Model).ToString(CultureInfo.InvariantCulture);
            }

            TagBuilder div = new TagBuilder("div");
            div.Attributes.Add("class", "input-append date");
            div.Attributes.Add("data-id", propertyName);
            //div.Attributes.Add("data-date-format", Format);


            TagBuilder input = new TagBuilder("input");
            input.Attributes.Add("id", ctrlName + "Picker");
            input.Attributes.Add("name", ctrlName + "Picker");
            input.Attributes.Add("size", "19");
            input.Attributes.Add("placeholder", format);
            input.Attributes.Add("type", "text");
            input.Attributes.Add("value", date);
            input.Attributes.Add("class", string.Join(" ", classes));

            TagBuilder input2 = new TagBuilder("input");
            input2.Attributes.Add("id", propertyName);
            input2.Attributes.Add("name", propertyName);
            input2.Attributes.Add("type", "hidden");
            input2.Attributes.Add("class", "selectedDate");
            input2.Attributes.Add("value", localdate);


            if (metaData.ContainerType.GetProperty(propertyName) != null)
            {
                var isRequired = metaData.ContainerType.GetProperty(propertyName).GetCustomAttributes(typeof(RequiredAttribute), false).Any();
                if (isRequired)
                {
                    //input.Attributes.Add("readonly", "readonly");
                    input.Attributes.Add("data-val-required", $"{displayName} is required");
                    input.Attributes.Add("data-val", "true");
                }
            }

            //input.Attributes.Add("data-val-data", LanguageResource.InvalidDate);

            if (htmlAttributes != null)
            {
                var customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                if (customAttributes != null)
                {
                    foreach (KeyValuePair<string, object> customAttribute in customAttributes)
                    {
                        input.MergeAttribute(customAttribute.Key, customAttribute.Value.ToString());
                    }
                }
            }

            div.InnerHtml += input.ToString(TagRenderMode.SelfClosing) + input2.ToString(TagRenderMode.SelfClosing)
                + "<span class='input-group-addon'><label for=\"" + ctrlName + "Picker\"><span class='glyphicon glyphicon-calendar'></span></label></span>";
            return new MvcHtmlString(div.ToString());
        }

        public static MvcHtmlString DatePicker3For<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            string format = GlobalSettings.DefaultDateFormat ?? "dd/MM/yyyy";

            // get the metdata
            ModelMetadata metaData = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            // get the field name
            var fieldName = ExpressionHelper.GetExpressionText(expression);

            // get full field (template may have prefix)
            var propertyName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            string ctrlName = propertyName.Replace(".", "");
            string displayName = metaData.DisplayName ?? (metaData.PropertyName ?? ExpressionHelper.GetExpressionText(expression));

            if (string.IsNullOrEmpty(format))
            {
                format = "MM/dd/yyyy";
            }
            string date = "";
            string localdate = "";
            if (metaData.Model != null)
            {
                var datetime = DateTime.ParseExact(metaData.Model.ToString(), format, new DateTimeFormatInfo());
                date = datetime.ToString(format);
                localdate = datetime.ToString(format);
            }

            TagBuilder div = new TagBuilder("div");
            div.Attributes.Add("class", "input-append date datepicker2");
            div.Attributes.Add("data-id", propertyName);
            //div.Attributes.Add("data-date-format", Format);


            TagBuilder input = new TagBuilder("input");
            input.Attributes.Add("id", ctrlName + "tmp");
            input.Attributes.Add("name", ctrlName + "tmp");
            input.Attributes.Add("size", "19");
            input.Attributes.Add("placeholder", format);
            input.Attributes.Add("type", "text");
            input.Attributes.Add("value", date);
            input.Attributes.Add("class", "datepicker input-small");

            TagBuilder input2 = new TagBuilder("input");
            input2.Attributes.Add("id", propertyName);
            input2.Attributes.Add("name", propertyName);
            input2.Attributes.Add("type", "hidden");
            input2.Attributes.Add("class", "selectedDate");
            input2.Attributes.Add("value", localdate);

            if (metaData.ContainerType.GetProperty(propertyName) != null)
            {
                var isRequired =
                    metaData.ContainerType.GetProperty(propertyName)
                        .GetCustomAttributes(typeof (RequiredAttribute), false)
                        .Any();
                if (isRequired)
                {
                    //input.Attributes.Add("readonly", "readonly");
                    input.Attributes.Add("data-val-required", $"{displayName} is required");
                    input.Attributes.Add("data-val", "true");
                }
            }


            input.Attributes.Add("data-val-data", LanguageResource.InvalidDate);

            if (htmlAttributes != null)
            {
                var customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                if (customAttributes != null)
                {
                    foreach (KeyValuePair<string, object> customAttribute in customAttributes)
                    {
                        input.MergeAttribute(customAttribute.Key, customAttribute.Value.ToString());
                    }
                }
            }

            div.InnerHtml += input.ToString(TagRenderMode.SelfClosing)
               + "<span class=\"input-group-addon\"><label for=\"" + ctrlName + "Picker\"><span class=\"glyphicon glyphicon-calendar\"></span></label></span>";
            return new MvcHtmlString(div + input2.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString DatePickerFromListFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            string format = GlobalSettings.DefaultDateFormat ?? "dd/MM/yyyy";

            // get the metdata
            ModelMetadata metaData = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            // get the field name
            var fieldName = ExpressionHelper.GetExpressionText(expression);

            // get full field (template may have prefix)
            var propertyName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            string ctrlName = propertyName.Replace(".", "");
            string displayName = metaData.DisplayName ?? (metaData.PropertyName ?? ExpressionHelper.GetExpressionText(expression));

            if (string.IsNullOrEmpty(format))
            {
                format = "MM/dd/yyyy";
            }
            string date = "";
            string localdate = "";
            if (metaData.Model != null)
            {
                date = ((DateTime)metaData.Model).ToString(format);
                localdate = ((DateTime)metaData.Model).ToString(CultureInfo.InvariantCulture);
            }

            TagBuilder div = new TagBuilder("div");
            div.Attributes.Add("class", "input-append date DatepickerFromList");
            div.Attributes.Add("data-id", propertyName);
            //div.Attributes.Add("data-date-format", Format);


            TagBuilder input = new TagBuilder("input");
            input.Attributes.Add("id", ctrlName + "tmp");
            input.Attributes.Add("name", ctrlName + "tmp");
            input.Attributes.Add("size", "19");
            input.Attributes.Add("placeholder", format);
            input.Attributes.Add("type", "text");
            input.Attributes.Add("value", date);
            input.Attributes.Add("class", "input-small");

            TagBuilder input2 = new TagBuilder("input");
            input2.Attributes.Add("id", propertyName);
            input2.Attributes.Add("name", propertyName);
            input2.Attributes.Add("type", "hidden");
            input2.Attributes.Add("class", "selectedDate");
            input2.Attributes.Add("value", localdate);

            if (metaData.ContainerType.GetProperty(propertyName) != null)
            {
                var isRequired =
                    metaData.ContainerType.GetProperty(propertyName)
                        .GetCustomAttributes(typeof (RequiredAttribute), false)
                        .Any();
                if (isRequired)
                {
                    //input.Attributes.Add("readonly", "readonly");
                    input.Attributes.Add("data-val-required", $"{displayName} is required");
                    input.Attributes.Add("data-val", "true");
                }
            }

            //input.Attributes.Add("data-val-data", LanguageResource.InvalidDate);

            if (htmlAttributes != null)
            {
                var customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                if (customAttributes != null)
                {
                    foreach (KeyValuePair<string, object> customAttribute in customAttributes)
                    {
                        input.MergeAttribute(customAttribute.Key, customAttribute.Value.ToString());
                    }
                }
            }

            div.InnerHtml += input.ToString(TagRenderMode.SelfClosing)
                + "<span class=\"input-group-addon\"><label for=\"" + ctrlName + "Picker\"><span class=\"glyphicon glyphicon-calendar\"></span></label></span>";
            return new MvcHtmlString(div + input2.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString DatePickerFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string selectedDate = null, object htmlAttributes = null)
        {
            string format = GlobalSettings.DefaultDateFormat ?? "dd/MM/yyyy";

            // get the metdata
            ModelMetadata metaData = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            // get the field name
            var fieldName = ExpressionHelper.GetExpressionText(expression);

            // get full field (template may have prefix)
            var propertyName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            string ctrlName = propertyName.Replace(".", "");
            string displayName = metaData.DisplayName ?? (metaData.PropertyName ?? ExpressionHelper.GetExpressionText(expression));

            string strSelectedDate = string.Empty, savedDate = string.Empty;
            if (!string.IsNullOrEmpty(selectedDate))
            {
                DateTime selectDate;
                if (DateTime.TryParseExact(selectedDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out selectDate))
                {
                    strSelectedDate = selectDate.ToString(format);
                    savedDate = selectDate.ToString("MM/dd/yyyy");
                }
                else
                {
                    selectDate = Convert.ToDateTime(selectedDate);
                    strSelectedDate = selectDate.ToString(format);
                    savedDate = selectDate.ToString("MM/dd/yyyy");
                }
            }
            else
            {
                if (metaData.Model != null)
                {
                    try
                    {
                        string formatSelectedDate = ((DateTime)metaData.Model).ToString(format);
                        if (formatSelectedDate != "01/01/0001")
                        {
                            strSelectedDate = ((DateTime)metaData.Model).ToString(format);
                            savedDate = ((DateTime)metaData.Model).ToString("MM/dd/yyyy");
                        }
                        else
                        {
                            strSelectedDate = string.Empty;
                            savedDate = string.Empty;
                        }
                    }
                    catch
                    {
                        strSelectedDate = ((DateTime)metaData.Model).ToString(format);
                        savedDate = ((DateTime)metaData.Model).ToString("MM/dd/yyyy");
                    }
                }
            }

            TagBuilder div = new TagBuilder("div");
            div.Attributes.Add("class", "input-group input-append date");
            div.Attributes.Add("data-id", propertyName);
            div.Attributes.Add("data-link-field", propertyName);
            div.Attributes.Add("data-date", strSelectedDate);
            div.Attributes.Add("data-date-format", format);
            div.Attributes.Add("data-link-format", format);
            div.Attributes.Add("data-provide", "datepicker");

            TagBuilder input = new TagBuilder("input");
            input.Attributes.Add("id", ctrlName + "Picker");
            input.Attributes.Add("name", ctrlName + "Picker");
            input.Attributes.Add("class", "form-control input-small date-input date ignored datepicker");
            input.Attributes.Add("size", "19");
            input.Attributes.Add("placeholder", format);
            input.Attributes.Add("type", "text");
            input.Attributes.Add("readonly", "true");
            input.Attributes.Add("value", strSelectedDate);


            TagBuilder input2 = new TagBuilder("input");
            input2.Attributes.Add("id", propertyName);
            input2.Attributes.Add("name", propertyName);
            input2.Attributes.Add("type", "hidden");
            input2.Attributes.Add("class", "selectedDate");
            input2.Attributes.Add("value", savedDate);

            if (metaData.ContainerType.GetProperty(propertyName) != null)
            {
                var isRequired =
                    metaData.ContainerType.GetProperty(propertyName)
                        .GetCustomAttributes(typeof (RequiredAttribute), false)
                        .Any();
                if (isRequired)
                {
                    input2.Attributes.Add("data-val-required", $"{displayName} is required");
                    input2.Attributes.Add("data-val", "true");
                    input2.Attributes.Add("aria-required", "true");
                    input2.Attributes.Add("aria-invalid", "true");
                }
            }

            if (htmlAttributes != null)
            {
                var customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                if (customAttributes != null)
                {
                    foreach (KeyValuePair<string, object> customAttribute in customAttributes)
                    {
                        input.MergeAttribute(customAttribute.Key, customAttribute.Value.ToString());
                    }
                }
            }

            div.InnerHtml += input.ToString(TagRenderMode.SelfClosing) + input2.ToString(TagRenderMode.SelfClosing)
                + "<span class=\"input-group-addon\"><label for=\"" + propertyName + "Picker\"><span class=\"glyphicon glyphicon-calendar\"></span></label></span>";
            return new MvcHtmlString(div.ToString());
        }

        public static MvcHtmlString DatePickerWithIconFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string selectedDate = null, object htmlAttributes = null)
        {
            string format = GlobalSettings.DefaultDateFormat ?? "dd/MM/yyyy";

            // get the metdata
            ModelMetadata metaData = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            // get the field name
            var fieldName = ExpressionHelper.GetExpressionText(expression);

            // get full field (template may have prefix)
            string propertyName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            string ctrlName = propertyName.Replace(".", "");
            string displayName = metaData.DisplayName ?? (metaData.PropertyName ?? ExpressionHelper.GetExpressionText(expression));
            
            string strSelectedDate = string.Empty, savedDate = string.Empty;
            if (!string.IsNullOrEmpty(selectedDate))
            {
                DateTime selectDate;
                if (DateTime.TryParseExact(selectedDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out selectDate))
                {
                    strSelectedDate = selectDate.ToString(format);
                    savedDate = selectDate.ToString("MM/dd/yyyy");
                }
                else
                {
                    selectDate = Convert.ToDateTime(selectedDate);
                    strSelectedDate = selectDate.ToString(format);
                    savedDate = selectDate.ToString("MM/dd/yyyy");
                }
            }
            else
            {
                if (metaData.Model != null)
                {
                    var formatSelectedDate = ((DateTime)metaData.Model).ToString(format);
                    if (formatSelectedDate != "01/01/0001")
                    {
                        strSelectedDate = formatSelectedDate;
                        savedDate = ((DateTime)metaData.Model).ToString("MM/dd/yyyy");
                    }
                }
            }

            TagBuilder div = new TagBuilder("div");
            div.Attributes.Add("class", "input-group input-append date");
            div.Attributes.Add("data-provide", "datepicker");
            div.Attributes.Add("data-id", ctrlName);
            div.Attributes.Add("data-link-field", ctrlName);
            div.Attributes.Add("data-date", strSelectedDate);
            div.Attributes.Add("data-date-format", format);
            div.Attributes.Add("data-link-format", format);

            TagBuilder input = new TagBuilder("input");
            input.Attributes.Add("id", ctrlName + "Picker");
            input.Attributes.Add("name", ctrlName + "Picker");
            input.Attributes.Add("class", "form-control input-small date-input date ignore ignored datepicker");
            input.Attributes.Add("size", "19");
            input.Attributes.Add("placeholder", format);
            input.Attributes.Add("type", "text");
            input.Attributes.Add("readonly", "true");
            input.Attributes.Add("value", strSelectedDate);


            TagBuilder input2 = new TagBuilder("input");
            input2.Attributes.Add("id", propertyName);
            input2.Attributes.Add("name", propertyName);
            input2.Attributes.Add("type", "hidden");
            input2.Attributes.Add("class", "selectedDate");
            input2.Attributes.Add("value", savedDate);


            if (metaData.ContainerType.GetProperty(propertyName) != null)
            {
                var isRequired = metaData.ContainerType.GetProperty(propertyName).GetCustomAttributes(typeof(RequiredAttribute), false).Any();
                if (isRequired)
                {
                    input.Attributes.Add("data-val-required", $"(*) {displayName} is required");
                    input.Attributes.Add("data-val", "true");
                    input.Attributes.Add("aria-required", "true");
                    input.Attributes.Add("aria-invalid", "true");
                    input.Attributes.Add("aria-describedby", $"{savedDate}-error");
                }
            }

            //input.Attributes.Add("data-val-data", $"{displayName} has invalid Date - {strSelectedDate}");

            if (htmlAttributes != null)
            {
                var customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                if (customAttributes != null)
                {
                    foreach (KeyValuePair<string, object> customAttribute in customAttributes)
                    {
                        input.MergeAttribute(customAttribute.Key, customAttribute.Value.ToString());
                    }
                }
            }

            div.InnerHtml += input.ToString(TagRenderMode.SelfClosing) + input2.ToString(TagRenderMode.SelfClosing)
                + "<span class='input-group-addon'><label for=\"" + ctrlName + "Picker\"><span class='glyphicon glyphicon-calendar'></span></label></span>";
            return new MvcHtmlString(div.ToString());
        }

        public static MvcHtmlString DatePicker(this HtmlHelper helper, string controlName, string selectedDate = null, bool? isRequired = false, object htmlAttributes = null)
        {
            string format = GlobalSettings.DefaultDateFormat ?? "dd/MM/yyyy";

            string strSelectedDate = string.Empty, savedDate = string.Empty;
            if (!string.IsNullOrEmpty(selectedDate))
            {
                DateTime selectDate;
                if (DateTime.TryParseExact(selectedDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out selectDate))
                {
                    strSelectedDate = selectDate.ToString(format);
                    savedDate = selectDate.ToString("MM/dd/yyyy");
                }
                else
                {
                    selectDate = Convert.ToDateTime(selectedDate);
                    strSelectedDate = selectDate.ToString(format);
                    savedDate = selectDate.ToString("MM/dd/yyyy");
                }
            }

            string ctrlName = controlName.Replace(".", "");

            TagBuilder div = new TagBuilder("div");
            div.Attributes.Add("class", "input-group input-append date");
            div.Attributes.Add("data-id", controlName);
            div.Attributes.Add("data-link-field", controlName);
            div.Attributes.Add("data-date", strSelectedDate);
            div.Attributes.Add("data-date-format", format);
            div.Attributes.Add("data-link-format", format);
            div.Attributes.Add("data-provide", "datepicker");


            TagBuilder input = new TagBuilder("input");
            input.Attributes.Add("id", ctrlName + "Picker");
            input.Attributes.Add("name", ctrlName + "Picker");
            input.Attributes.Add("class", "form-control input-small date-input date ignored datepicker");
            input.Attributes.Add("size", "19");
            input.Attributes.Add("placeholder", format);
            input.Attributes.Add("type", "text");
            //input.Attributes.Add("readonly", "true");   
            input.Attributes.Add("value", strSelectedDate);

            TagBuilder input2 = new TagBuilder("input");
            input2.Attributes.Add("id", controlName);
            input2.Attributes.Add("name", controlName);
            input2.Attributes.Add("type", "hidden");
            input2.Attributes.Add("class", "selectedDate");
            input2.Attributes.Add("value", savedDate);

            if (isRequired != null && isRequired == true)
            {
                input.Attributes.Add("data-val-required", $"(*) {controlName} is required");
                input.Attributes.Add("data-val", "true");
            }

            //input.Attributes.Add("data-val-data", string.Format("Invalid", strSelectedDate));
            if (htmlAttributes != null)
            {
                var customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                if (customAttributes != null)
                {
                    foreach (KeyValuePair<string, object> customAttribute in customAttributes)
                    {
                        input.MergeAttribute(customAttribute.Key, customAttribute.Value.ToString());
                    }
                }
            }

            div.InnerHtml += input.ToString(TagRenderMode.SelfClosing) + input2.ToString(TagRenderMode.SelfClosing)
                 + "<span class=\"input-group-addon\"><label for=\"" + ctrlName + "Picker\"><span class=\"glyphicon glyphicon-calendar\"></span></label></span>";
            return new MvcHtmlString(div.ToString());
        }

        public static MvcHtmlString DateOfBirthFor(this HtmlHelper html, string id, int minYear, int maxYear, object htmlAttribute = null)
        {
            RouteValueDictionary attributes = new RouteValueDictionary(htmlAttribute);

            var days = Enumerable.Range(1, 31).Select(x => new SelectListItem
            {
                Value = x.ToString(),
                Text = x.ToString()
            });
            var months = Enumerable.Range(1, 12).Select(x => new SelectListItem
            {
                Value = x.ToString(),
                Text = x.ToString()
            });
            var years = Enumerable.Range(minYear, maxYear - (minYear - 1)).Select(x => new SelectListItem
            {
                Value = x.ToString(),
                Text = x.ToString()
            });

            var mainDivTag = new TagBuilder("div");
            mainDivTag.MergeAttribute("id", id);
            mainDivTag.MergeAttributes(attributes);
            mainDivTag.InnerHtml = string.Concat(
                html.DropDownList("Day", days, new { style = "width : 40px " }).ToHtmlString(),
                html.DropDownList("Month", months, new { style = "width : 40px " }).ToHtmlString(),
                html.DropDownList("Year", years, new { style = "width : 60px " }).ToHtmlString()
            );

            return new MvcHtmlString(mainDivTag.ToString(TagRenderMode.SelfClosing));
        }

        //protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor)
        //{
        //    if (propertyDescriptor.Name == "DateOfBirth")
        //    {
        //        DateTime dob = new DateTime(int.Parse(controllerContext.HttpContext.Request.Form["Year"]), int.Parse(controllerContext.HttpContext.Request.Form["Month"]), int.Parse(controllerContext.HttpContext.Request.Form["Day"]));
        //        propertyDescriptor.SetValue(bindingContext.Model, dob);
        //    }
        //}
        #endregion

        #region Time Picker
        public static MvcHtmlString TimePickerFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string format = null, object htmlAttributes = null)
        {
            var data = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            if (string.IsNullOrEmpty(format))
            {
                format = "hh:mm";
            }

            string propertyName = data.PropertyName;
            string simpleDisplayText = "";

            try
            {
                simpleDisplayText = ((DateTime)data.Model).ToString("HH:mm");
            }
            catch
            {
                var timespan = (TimeSpan)data.Model;
                simpleDisplayText =
                    ((timespan.Hours < 10) ? "0" + timespan.Hours : timespan.Hours.ToString())
                    + ":" +
                    ((timespan.Minutes < 10) ? "0" + timespan.Minutes : timespan.Minutes.ToString());

            }

            TagBuilder div = new TagBuilder("div");
            div.Attributes.Add("class", "input-append timepicker");

            TagBuilder input = new TagBuilder("input");
            input.Attributes.Add("id", propertyName);
            input.Attributes.Add("name", propertyName);
            input.Attributes.Add("type", "text");
            input.Attributes.Add("class", "input-mini");
            input.Attributes.Add("placeholder", "hh:mm");
            input.Attributes.Add("data-format", format);
            input.Attributes.Add("value", simpleDisplayText);

            if (htmlAttributes != null)
            {
                var customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                if (customAttributes != null)
                {
                    foreach (KeyValuePair<string, object> customAttribute in customAttributes)
                    {
                        input.MergeAttribute(customAttribute.Key, customAttribute.Value.ToString());
                    }
                }
            }

            div.InnerHtml += input.ToString(TagRenderMode.SelfClosing) + "<span class=\"add-on\"><i data-time-icon=\"icon-time\" data-date-icon=\"icon-calendar\"></i></span>";
            return new MvcHtmlString(div.ToString());
        }
        #endregion
    }
}