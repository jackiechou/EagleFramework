using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.UI;

namespace System.Web.Mvc
{
    public static class CustomHelpers
    {

        #region Required LabelFor
        public static MvcHtmlString RequiredLabelFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            var data = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            string propertyName = data.PropertyName;

            var metaData = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            string metaDataValue = metaData.DisplayName ?? (metaData.PropertyName ?? ExpressionHelper.GetExpressionText(expression));

            TagBuilder label = new TagBuilder("label");
            label.Attributes.Add("for", propertyName);
            label.InnerHtml += $"{metaDataValue} (<span class='color-red'>*</span>)";
            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                label.MergeAttributes(attributes, true);
            }
            return new MvcHtmlString(label.ToString());
        }

        public static MvcHtmlString RequiredLabel(this HtmlHelper htmlHelper, string labelName, string labelText, object htmlAttributes = null)
        {
            if (string.IsNullOrEmpty(labelText))
            {
                labelText = labelName.Split('.').Last();
            }
            if (string.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }

            var asteriskTag = new TagBuilder("span");
            asteriskTag.Attributes.Add("class", "color-red required");
            asteriskTag.SetInnerText("*");

            TagBuilder label = new TagBuilder("label");
            label.Attributes.Add("id", labelName);
            label.Attributes.Add("name", labelName);
            label.Attributes.Add("for",TagBuilder.CreateSanitizedId(htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(labelName)));
            label.Attributes.Add("class", "label-required");
            label.InnerHtml=$"{labelText} ({asteriskTag.ToString(TagRenderMode.Normal)})";

            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                label.MergeAttributes(attributes, true);
            }

            return MvcHtmlString.Create(label.ToString(TagRenderMode.Normal));
        }

        #endregion

        #region Enum DropDownList ==========================================================================================
        //@Html.DisplayEnumFor(model => model.Profile.Contact.Sex)
        public static MvcHtmlString DisplayEnumFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
        {
            ModelMetadata metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string value = metaData.Model.ToString();
            Type type = typeof(TEnum);
            var member = type.GetMember(value)[0];
            var attributes = member.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attributes.Length > 0)
            {
                string displayname = ((DisplayAttribute)attributes[0]).ResourceType != null ? ((DisplayAttribute)attributes[0]).GetName() : ((DisplayAttribute)attributes[0]).Name;
                return new MvcHtmlString(displayname);
            }
            return new MvcHtmlString(value);
        }

        //@Sex.EnumDisplayName()
        public static HtmlString EnumDisplayName(this Enum item)
        {
            var type = item.GetType();
            var member = type.GetMember(item.ToString());
            var attributes = member[0].GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attributes.Length > 0)
            {
                string displayname = ((DisplayAttribute)attributes[0]).ResourceType != null ? ((DisplayAttribute)attributes[0]).GetName() : ((DisplayAttribute)attributes[0]).Name;
                return new MvcHtmlString(displayname);
            }
            return new MvcHtmlString(item.ToString());
        }

        private static readonly SelectListItem[] SingleEmptyItem = new[] { new SelectListItem { Text = "", Value = "" } };
        private static IDictionary<string, object> MergeHtmlAttributes(this HtmlHelper helper, object htmlAttributesObject, object defaultHtmlAttributesObject)
        {
            var concatKeys = new string[] { "class" };

            var htmlAttributesDict = htmlAttributesObject as IDictionary<string, object>;
            var defaultHtmlAttributesDict = defaultHtmlAttributesObject as IDictionary<string, object>;

            RouteValueDictionary htmlAttributes = (htmlAttributesDict != null)
                ? new RouteValueDictionary(htmlAttributesDict)
                : HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributesObject);
            RouteValueDictionary defaultHtmlAttributes = (defaultHtmlAttributesDict != null)
                ? new RouteValueDictionary(defaultHtmlAttributesDict)
                : HtmlHelper.AnonymousObjectToHtmlAttributes(defaultHtmlAttributesObject);

            foreach (var item in htmlAttributes)
            {
                if (concatKeys.Contains(item.Key))
                {
                    defaultHtmlAttributes[item.Key] = (defaultHtmlAttributes[item.Key] != null)
                        ? string.Format("{0} {1}", defaultHtmlAttributes[item.Key], item.Value)
                        : item.Value;
                }
                else
                {
                    defaultHtmlAttributes[item.Key] = item.Value;
                }
            }

            return defaultHtmlAttributes;
        }
        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, string optionLabel, int? selectedValue = null, object htmlAttributes = null)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var enumType = Nullable.GetUnderlyingType(metaData.ModelType) ?? metaData.ModelType;
            var enumValues = Enum.GetValues(enumType).Cast<TEnum>().ToList();

            var items = enumValues.Select(item =>
            {
                var type = item.GetType();
                var member = type.GetMember(item.ToString());
                var attribute = member[0].GetCustomAttribute<DisplayAttribute>();

                string text;
                if (attribute == null)
                {
                    text = item.ToString();
                }
                else
                {
                    text = attribute.Description ?? attribute.Name;
                }
               
                string value = (Convert.ToInt32(item)).ToString();
                bool selected;
                if (selectedValue != null)
                {
                    selected = Convert.ToInt32(item) == Convert.ToInt32(selectedValue);
                }
                else
                {
                    selected = item.Equals(metaData.Model);
                }

                return new SelectListItem
                {
                    Text = text,
                    Value = value,
                    Selected = selected
                };
            });

            // If the enum is nullable, add an 'empty' item to the collection
            if (metaData.IsNullableValueType)
                items = SingleEmptyItem.Concat(items);

            var defaultHtmlAttributesObject = new { @class = "form-control" };
            var customHtmlAttributes = htmlHelper.MergeHtmlAttributes(htmlAttributes, defaultHtmlAttributesObject);
            return htmlHelper.DropDownListFor(expression, items, optionLabel, customHtmlAttributes);
        }

        #endregion =========================================================================================================

        public static string BulletedList(this HtmlHelper helper, string name)
        {
            var items = helper.ViewData.Eval(name) as IEnumerable;
            if (items == null)
                throw new NullReferenceException("Cannot find " + name + " in view data");

            var writer = new HtmlTextWriter(new StringWriter());

            // Open UL
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);
            foreach (var item in items)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Li);
                writer.Write(helper.Encode(item));
                writer.RenderEndTag();
                writer.WriteLine();
            }
            // Close UL
            writer.RenderEndTag();

            // Return the HTML string
            return writer.InnerWriter.ToString();
        }

        #region NumericUpDownFor ========================================================================================
        public static MvcHtmlString NumericUpDownFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression,
            int? min = null, int? max = null, string step = "any", string selectedValue = null, object htmlAttributes = null)
        {
            var data = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            string propertyName = data.PropertyName;

            string selectedVal = string.Empty;
            if (!string.IsNullOrEmpty(selectedValue))
                selectedVal = selectedValue;
            else
            {
                if (data.Model != null)
                    selectedVal = data.Model.ToString();
            }

            TagBuilder tagInput = new TagBuilder("input");
            tagInput.Attributes.Add("id", propertyName);
            tagInput.Attributes.Add("name", propertyName);
            tagInput.Attributes.Add("type", "number");

            if (min != null)
                tagInput.Attributes.Add("Min", min.ToString());
            if (max != null)
                tagInput.Attributes.Add("Max", max.ToString());
            if (step != null)
                tagInput.Attributes.Add("Step", step);

            tagInput.Attributes.Add("value", selectedVal);

            if (data.IsRequired)
            {
                //input.Attributes.Add("readonly", "readonly");
                string required = $"Please complete the \"{data.PropertyName}\" field.";
                tagInput.Attributes.Add("data-val-required", required);
            }
            tagInput.Attributes.Add("data-val-data", "The field must be a number.");
            if (htmlAttributes == null) return new MvcHtmlString(tagInput.ToString());
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            tagInput.MergeAttributes(attributes, true);
            return new MvcHtmlString(tagInput.ToString());
        }
        #endregion NumericUpDownFor =====================================================================================

        #region DropDownList ====================================================================================
        /// <summary>
        /// This HTML helper will create a Dropdownlist.
        /// </summary>
        /// <param name="helper">HTMLHelper</param>
        /// <param name="dataSource">Datasource to create the Dropdownlist.</param>
        /// <param name="dataValueProperty">Property name , which is used to populate the value field for Dropdownlist</param>
        /// <param name="dataTextProperty">Property name , which is used to populate the Text field for Dropdownlist</param>
        /// <param name="selectedValue">Property name , which is used to identify the selected item for Dropdownlist</param>
        /// <param name="htmlAttributes">Additional htmlAttributes.</param>
        /// <returns></returns>
        public static string DataBoundDropDownList(this HtmlHelper helper, object dataSource, string dataValueProperty, string dataTextProperty, string selectedValue, object htmlAttributes)
        {
            TagBuilder selectTag = CreateDropDownlist(dataSource, dataValueProperty, dataTextProperty, selectedValue);
            selectTag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return (selectTag.ToString());
        }

        public static string DataBoundDropDownList(this HtmlHelper helper, object dataSource, string dataValueProperty, string dataTextProperty, string selectedValue)
        {
            TagBuilder selectTag = CreateDropDownlist(dataSource, dataValueProperty, dataTextProperty, selectedValue);
            return (selectTag.ToString());
        }

        private static TagBuilder CreateDropDownlist(object dataSource, string dataValueProperty, string dataTextProperty, string selectedValue)
        {
            #region "Create the DropdownList"
            TagBuilder selectBuilder = new TagBuilder("select");
            StringBuilder optionStringBuilder = new StringBuilder();
            #endregion

            #region "Create OptionList"
            IEnumerable ddlSource = dataSource as IEnumerable;
            if (ddlSource == null)
            {
                throw new Exception("DataSource must implement IEnumerable Interface.");
            }
            IEnumerator item = ddlSource.GetEnumerator();
            while (item.MoveNext())
            {
                var propertyValue = DataBinder.GetPropertyValue(item.Current, dataTextProperty);
                if (propertyValue != null)
                {
                    string strText = propertyValue.ToString();
                    var value = DataBinder.GetPropertyValue(item.Current, dataValueProperty);
                    if (value != null)
                    {
                        string strVal = value.ToString();
                        TagBuilder optionBuilder = new TagBuilder("option");
                        optionBuilder.MergeAttribute("value", strVal);
                        optionBuilder.InnerHtml = strText;
                        if (strVal == selectedValue)
                        {
                            optionBuilder.MergeAttribute("selected", "selected");
                        }
                        optionStringBuilder.Append(optionBuilder);
                    }
                }
            }
            selectBuilder.InnerHtml = optionStringBuilder.ToString();
            #endregion

            return (selectBuilder);
        }
        #endregion ==============================================================================================

        #region Span ============================================================================================
        public static MvcHtmlString SpanFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            var valueGetter = expression.Compile();
            var value = valueGetter(helper.ViewData.Model);

            var span = new TagBuilder("span");
            span.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            if (value != null)
            {
                span.SetInnerText(value.ToString());
            }

            return MvcHtmlString.Create(span.ToString());
        }
        #endregion ==============================================================================================

        #region CheckBox =========================================================================================
        public static MvcHtmlString CheckBoxDisplayFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression,
           bool? selectedValue = null, object htmlAttributes = null)
        {
            var data = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            string propertyName = data.PropertyName;

            bool? selectedVal = false;
            if (selectedValue != null && selectedValue.ToString() != string.Empty)
                selectedVal = selectedValue;
            else
            {
                if (data.Model != null)
                    selectedVal = Convert.ToBoolean(data.Model);
            }

            TagBuilder checkboxInput = new TagBuilder("input");
            checkboxInput.Attributes.Add("id", propertyName);
            checkboxInput.Attributes.Add("name", propertyName);
            checkboxInput.Attributes.Add("type", HtmlHelper.GetInputTypeString(InputType.CheckBox));
            checkboxInput.Attributes.Add("value", selectedVal.ToString());

            TagBuilder hiddenInput = new TagBuilder("input");
            hiddenInput.Attributes.Add("name", propertyName);
            hiddenInput.Attributes.Add("type", HtmlHelper.GetInputTypeString(InputType.Hidden));
            hiddenInput.Attributes.Add("value", selectedVal.ToString());


            if (data.IsRequired)
            {
                //input.Attributes.Add("readonly", "readonly");
                string required = $"Please complete the {data.PropertyName} field.";
                checkboxInput.Attributes.Add("data-val-required", required);
            }

            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                checkboxInput.MergeAttributes(attributes, true);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(checkboxInput.ToString(TagRenderMode.SelfClosing));
            sb.Append(hiddenInput.ToString(TagRenderMode.SelfClosing));
            return new MvcHtmlString(sb.ToString());
        }
        #endregion ===============================================================================================

        public static IHtmlString HiddenForModel<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TProperty>> expression)
        {
            RemoveStateFor(htmlHelper, expression);
            return htmlHelper.HiddenFor(expression);
        }

        /// <summary>
        /// Custom HiddenFor that addresses the issues noted here:
        /// http://stackoverflow.com/questions/594600/possible-bug-in-asp-net-mvc-with-form-values-being-replaced
        /// We will only ever want values pulled from the model passed to the page instead of 
        /// pulling from modelstate.  
        /// Note, do not use 'ValueFor' in this method for these reasons.
        /// </summary>
        public static IHtmlString HiddenFieldFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object value = null, bool withValidation = false)
        {
            if (value == null)
            {
                value = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model;
            }

            return new HtmlString(
                $"<input type='hidden' id='{htmlHelper.IdFor(expression)}' name='{htmlHelper.NameFor(expression)}' value='{value}' />");
        }

        public static MvcHtmlString EmailFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, Object htmlAttributes)
        {
            MvcHtmlString emailfor = html.TextBoxFor(expression, htmlAttributes);
            return new MvcHtmlString(emailfor.ToHtmlString().Replace("type=\"text\"", "type=\"email\""));
        }

        public static MvcHtmlString ValidationSummaryJQuery(this HtmlHelper htmlHelper, string message, IDictionary<string, object> htmlAttributes)
        {
            if (!htmlHelper.ViewData.ModelState.IsValid)
                return htmlHelper.ValidationSummary(message, htmlAttributes);


            StringBuilder sb = new StringBuilder(Environment.NewLine);

            var divBuilder = new TagBuilder("div");
            divBuilder.MergeAttributes(htmlAttributes);
            divBuilder.AddCssClass(HtmlHelper.ValidationSummaryValidCssClassName); // intentionally add VALID css class

            if (!string.IsNullOrEmpty(message))
            {
                //--------------------------------------------------------------------------------
                // Build an EMPTY error summary message <span> tag
                //--------------------------------------------------------------------------------
                var spanBuilder = new TagBuilder("span");
                spanBuilder.SetInnerText(message);
                sb.Append(spanBuilder.ToString(TagRenderMode.Normal)).Append(Environment.NewLine);
            }

            divBuilder.InnerHtml = sb.ToString();
            return MvcHtmlString.Create(divBuilder.ToString(TagRenderMode.Normal));
        }

        public static byte[] GetBytesFromFile(HttpFileCollectionBase files)
        {
            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        BinaryReader br = new BinaryReader(file.InputStream);
                        byte[] data = br.ReadBytes((int)file.InputStream.Length);
                        br.Close();
                        return data;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        private static Type GetNonNullableModelType(ModelMetadata modelMetadata)
        {
            Type realModelType = modelMetadata.ModelType;

            Type underlyingType = Nullable.GetUnderlyingType(realModelType);
            if (underlyingType != null)
            {
                realModelType = underlyingType;
            }
            return realModelType;
        }

        /// <summary>
        /// Removes the ModelState entry corresponding to the specified property on the model if no validation errors exist. 
        /// Call this when changing Model values on the server after a postback, 
        /// to prevent ModelState entries from taking precedence.
        /// </summary>
        public static void RemoveStateFor<TModel, TProperty>(this HtmlHelper helper,
            Expression<Func<TModel, TProperty>> expression)
        {
            //First get the expected name value. This is equivalent to helper.NameFor(expression)
            string name = ExpressionHelper.GetExpressionText(expression);
            string fullHtmlFieldName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            //Now check whether modelstate errors exist for this input control
            ModelState modelState;
            if (!helper.ViewData.ModelState.TryGetValue(fullHtmlFieldName, out modelState) ||
                modelState.Errors.Count == 0)
            {
                //Only remove ModelState value if no modelstate error exists,
                //so the ModelState will not be used over the Model
                helper.ViewData.ModelState.Remove(name);
            }
        }

        public static MvcHtmlString StripHtml(this HtmlHelper html, string input)
        {
            return new MvcHtmlString(Regex.Replace(input, "<.*?>", string.Empty));
        }

        public static MvcHtmlString DisplayUpperFirstEachTitleFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string metaDataValue = metaData.DisplayName ?? (metaData.PropertyName ?? ExpressionHelper.GetExpressionText(expression));

            if (string.IsNullOrEmpty(metaDataValue))
            {
                return new MvcHtmlString(string.Empty);
            }
            var a = metaDataValue.ToLower().ToCharArray();

            for (var i = 0; i < a.Count(); i++)
            {
                a[i] = i == 0 || a[i - 1] == ' ' ? char.ToUpper(a[i]) : a[i];

            }
            return new MvcHtmlString(new string(a));
        }

        public static MvcHtmlString DisplayUpperFirstEachTitle(this HtmlHelper htmlHelper, string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return new MvcHtmlString(string.Empty);
            }
            var a = title.ToLower().ToCharArray();

            for (var i = 0; i < a.Count(); i++)
            {
                a[i] = i == 0 || a[i - 1] == ' ' ? char.ToUpper(a[i]) : a[i];

            }
            return new MvcHtmlString(new string(a));
        }

        public static MvcHtmlString DisplayUpperTitleFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string metaDataValue = metaData.DisplayName ?? (metaData.PropertyName ?? ExpressionHelper.GetExpressionText(expression));

            if (string.IsNullOrEmpty(metaDataValue))
            {
                return new MvcHtmlString(string.Empty);
            }
            return new MvcHtmlString(metaDataValue.ToUpper());
        }

        public static MvcHtmlString DisplayUpperTitle(this HtmlHelper htmlHelper, string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return new MvcHtmlString(string.Empty);
            }
            return new MvcHtmlString(title.ToUpper());
        }

        public static MvcHtmlString DisplayTitleCaseFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string metaDataValue = metaData.DisplayName ?? (metaData.PropertyName ?? ExpressionHelper.GetExpressionText(expression));
            if (string.IsNullOrEmpty(metaDataValue))
            {
                return new MvcHtmlString(string.Empty);
            }
            return new MvcHtmlString(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(metaDataValue));
        }

        public static MvcHtmlString DisplayTitleCase(this HtmlHelper htmlHelper, string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return new MvcHtmlString(string.Empty);
            }
            return new MvcHtmlString(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title));
        }

        public static MvcHtmlString DisplayLowerTitleCaseFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string metaDataValue = metaData.DisplayName ?? (metaData.PropertyName ?? ExpressionHelper.GetExpressionText(expression));
            if (string.IsNullOrEmpty(metaDataValue))
            {
                return new MvcHtmlString(string.Empty);
            }
            return new MvcHtmlString(metaDataValue.ToLower());
        }

        public static MvcHtmlString DisplayLowerTitleCase(this HtmlHelper htmlHelper, string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return new MvcHtmlString(string.Empty);
            }
            return new MvcHtmlString(title.ToLower());
        }

        //public static IHtmlString RenderStyles(this HtmlHelper helper, params string[] additionalPaths)
        //{
        //    var page = helper.ViewDataContainer as WebPageExecutingBase;
        //    if (page != null && page.VirtualPath.StartsWith("~/"))
        //    {
        //        var virtualPath = "~/bundles" + page.VirtualPath.Substring(1);
        //        if (BundleTable.Bundles.GetBundleFor(virtualPath) == null)
        //        {
        //            var defaultPath = page.VirtualPath + ".css";
        //            BundleTable.Bundles.Add(new StyleBundle(virtualPath).Include(defaultPath).Include(additionalPaths));
        //        }
        //        return MvcHtmlString.Create(@"<link href=""" + HttpUtility.HtmlAttributeEncode(BundleTable.Bundles.ResolveBundleUrl(virtualPath)) + @""" rel=""stylesheet""/>");
        //    }
        //    return MvcHtmlString.Empty;
        //}

        //public static IHtmlString RenderScripts(this HtmlHelper helper, params string[] additionalPaths)
        //{
        //    var page = helper.ViewDataContainer as WebPageExecutingBase;
        //    if (page != null && page.VirtualPath.StartsWith("~/"))
        //    {
        //        var virtualPath = "~/bundles" + page.VirtualPath.Substring(1);
        //        if (BundleTable.Bundles.GetBundleFor(virtualPath) == null)
        //        {
        //            var defaultPath = page.VirtualPath + ".js";
        //            BundleTable.Bundles.Add(new ScriptBundle(virtualPath).Include(defaultPath).Include(additionalPaths));
        //        }
        //        return MvcHtmlString.Create(@"<script src=""" + HttpUtility.HtmlAttributeEncode(BundleTable.Bundles.ResolveBundleUrl(virtualPath)) + @"""></script>");
        //    }
        //    return MvcHtmlString.Empty;
        //}
    }
}