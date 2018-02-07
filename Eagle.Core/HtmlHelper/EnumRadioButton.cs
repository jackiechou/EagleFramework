using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Eagle.Core.HtmlHelper;
using Eagle.Core.HtmlHelper.CheckBox.Model;

namespace System.Web.Mvc
{
    public static class EnumRadioButton
    {
        private static Type GetEnumTypeFromAttribute<TModel, TValue>(Expression<Func<TModel, TValue>> expression)
        {
            MemberInfo propertyInfo =
                ((expression.Body as MemberExpression).Member as MemberInfo);
            if (propertyInfo == null)
                throw new ArgumentException("ArgumentException-MemberInfo");
            EnumDataTypeAttribute attr = Attribute.GetCustomAttribute(propertyInfo,
                typeof(EnumDataTypeAttribute)) as EnumDataTypeAttribute;
            if (attr == null)
                throw new ArgumentException("ArgumentException-EnumDataType");
            return attr.EnumType;
        }

        private static bool GetEnumPartsDescription<TValue>(TValue enumValue, Type enumType)
        {
            if (enumValue != null)
            {
                FieldInfo fi = enumType.GetField(enumType.GetEnumName(enumValue));
                PartsDescriptionAttribute[] attributes = (PartsDescriptionAttribute[])fi.GetCustomAttributes(typeof(PartsDescriptionAttribute), false);
                if ((attributes != null) && (attributes.Length > 0))
                {
                    return attributes[0].EnableParts;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private static string GetEnumDescription<TValue>(TValue enumValue, Type enumType)
        {
            FieldInfo fi = enumType.GetField(enumType.GetEnumName(enumValue));

            PartsDescriptionAttribute[] attributes = (PartsDescriptionAttribute[])fi.GetCustomAttributes(typeof(PartsDescriptionAttribute), false);

            if ((attributes != null) && (attributes.Length > 0))
                return attributes[0].Description;
            else
                return enumType.GetEnumName(enumValue);
        }

        public static IList<SelectListItem> GetSelectItemsForEnum(Type enumType, object selectedValue)
        {
            var selectItems = new List<SelectListItem>();

            if (enumType.IsGenericType &&
                enumType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                enumType = enumType.GetGenericArguments()[0];
                selectItems.Add(new SelectListItem { Value = string.Empty, Text = string.Empty });
            }

            var selectedValueName = selectedValue != null ? selectedValue.ToString() : null;
            var enumEntryNames = Enum.GetNames(enumType);
            var entries = enumEntryNames
                .Select(n => new
                {
                    Name = n,
                    DisplayAttribute = enumType
                        .GetField(n)
                        .GetCustomAttributes(typeof(DisplayAttribute), false)
                        .OfType<DisplayAttribute>()
                        .SingleOrDefault() ?? new DisplayAttribute()
                })
                .Select(e => new
                {
                    Value = e.Name,
                    DisplayName = e.DisplayAttribute.Name ?? e.Name,
                    Order = e.DisplayAttribute.GetOrder() ?? 50
                })
                .OrderBy(e => e.Order)
                .ThenBy(e => e.DisplayName)
                .Select(e => new SelectListItem
                {
                    Value = e.Value,
                    Text = e.DisplayName,
                    Selected = e.Value == selectedValueName
                });

            selectItems.AddRange(entries);

            return selectItems;
        }

        public static MvcHtmlString EnumRadioButtonFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, bool? isHorizontal = true, object htmlAttributes = null)
        {
            ModelMetadata metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string name = ExpressionHelper.GetExpressionText(expression);
            if (!metaData.ModelType.IsEnum)
            {
                throw new ArgumentException($"The property {name} is not an enum");
            }

            Type enumType = GetEnumTypeFromAttribute(expression);
            IEnumerable<int> values = Enum.GetValues(enumType).Cast<int>();

            var rtnstr = string.Empty;

            foreach (var val in values.Where(x => GetEnumPartsDescription(x, enumType)))
            {
                TagBuilder radioTag = new TagBuilder("input");
                radioTag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
                radioTag.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.Radio));

                string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(
                        ExpressionHelper.GetExpressionText(expression));

                radioTag.MergeAttribute("name", fullName, true);

                ModelState modelState;
                string modelStateValue = null;
                if (htmlHelper.ViewData.ModelState.TryGetValue(fullName, out modelState))
                {
                    if (modelState.Value != null)
                    {
                        modelStateValue = modelState.Value.ConvertTo(typeof(string), null) as string;
                    }
                }
                bool isChecked = false;
                bool useModelState = false;
                if (modelStateValue != null)
                {
                    isChecked = String.Equals(modelStateValue, val.ToString(), StringComparison.Ordinal);
                    useModelState = true;
                }
                if (!useModelState)
                {
                    if (htmlHelper.ViewData.Model != null)
                    {
                        string viewdataval = htmlHelper.ViewData.Eval(fullName).ToString();
                        isChecked = val.ToString() == viewdataval;
                    }
                }
                if (isChecked)
                {
                    radioTag.MergeAttribute("checked", "checked");
                }
                radioTag.MergeAttribute("value", val.ToString(), true);

                // id
                string id = fullName + val;
                radioTag.GenerateId(id);

                // label
                TagBuilder labelTag = new TagBuilder("Label") { InnerHtml = radioTag.ToString() };
                labelTag.InnerHtml += GetEnumDescription(val, enumType);
                labelTag.MergeAttribute("for", radioTag.Attributes["id"]);

                if (isHorizontal != null && isHorizontal == true)
                {
                    labelTag.AddCssClass("radio-inline");
                }

                rtnstr += labelTag.ToString();
            }

            //div
            TagBuilder divTag = new TagBuilder("div");
            divTag.Attributes.Add("class", "btn-group");
            divTag.Attributes.Add("role", "group");
            divTag.Attributes.Add("data-toggle", "buttons");
            divTag.InnerHtml = rtnstr;

            return new MvcHtmlString(divTag.ToString());
        }


        public static MvcHtmlString RadioButtonsForEnum(this HtmlHelper htmlHelper, string controlName, Type enumType, bool isHorizontal = false, int? selectedValue = null, object htmlAttribute = null)
        {
            var sb = new StringBuilder();
            sb = sb.AppendFormat("<div class='radioList'>");

            // Create a radio button for each item in the list
            var names = Enum.GetNames(enumType);
            foreach (var name in names)
            {
                var labelText = name;

                var memInfo = enumType.GetMember(name);
                var attributes = memInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);

                if (attributes.Length > 0)
                {
                    labelText = ((DisplayAttribute)attributes[0]).ResourceType != null ? ((DisplayAttribute)attributes[0]).GetName() : ((DisplayAttribute)attributes[0]).Name;
                }

                var id = name;
                var value = Convert.ToInt32(names.Where(e => e.StartsWith(name)).Select(e => (int)Enum.Parse(enumType, e)).FirstOrDefault());

                // Create tag builder
                var radio = new TagBuilder("input");
                radio.Attributes.Add("id", id);
                radio.Attributes.Add("name", controlName);
                radio.Attributes.Add("type", "radio");
                radio.Attributes.Add("value", value.ToString());

                string selectedFlag = string.Empty;
                if (selectedValue != null && selectedValue > 0)
                {
                    if (value == selectedValue)
                    {
                        selectedFlag = "active";
                        radio.Attributes.Add("checked", "checked");
                    }
                }
                else
                {
                    if (names.First() == name)
                    {
                        selectedFlag = "active";
                        radio.Attributes.Add("checked", "checked");
                    }
                }

                // Add attributes                    
                radio.MergeAttributes(new RouteValueDictionary(htmlAttribute));
                radio.ToString();

                sb.AppendFormat(isHorizontal ? "<label class='radio-inline " + selectedFlag + "'>{0}{1}</label>" :
                    "<label class='radio " + selectedFlag + "'>{0}{1}</label>", radio, HttpUtility.HtmlEncode(labelText));
            }

            sb = sb.AppendFormat("</div>");

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString RadioButtonEnumFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object selectedValue = null, bool? isHorizontal = true)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (!metaData.ModelType.IsEnum)
            {
                throw new ArgumentException("This helper is intended to be used with enum types");
            }

            var names = Enum.GetNames(metaData.ModelType);
            var sb = new StringBuilder();
            sb = sb.AppendFormat("<div>");

            //var fields = metaData.ModelType.GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public);

            // Create a radio button for each item in the list
            foreach (var name in names)
            {
                string description = string.Empty;
                var metaDataValue = names.Where(e => e.StartsWith(name)).Select(e => (int)Enum.Parse(metaData.ModelType, e)).FirstOrDefault();

                var memInfo = metaData.ModelType.GetMember(name);
                if (memInfo != null)
                {
                    var enumAttributes = memInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
                    if (enumAttributes.Length > 0)
                    {
                        description = ((DisplayAttribute)enumAttributes[0]).ResourceType != null ? ((DisplayAttribute)enumAttributes[0]).GetName() : name;
                    }
                }

                // Add attributes  
                var id = $"{htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix}_{metaData.PropertyName}_{name}";
                var attributes = new Dictionary<string, object> { { "id", id } };
                string selectedFlag = string.Empty;
                if (selectedValue != null)
                {
                    if (metaDataValue == Convert.ToInt32(selectedValue))
                    {
                        selectedFlag = "active";
                        attributes.Add("checked", "checked");
                    }
                }
                else
                {
                    if (names.First() == name)
                    {
                        selectedFlag = "active";
                        attributes.Add("checked", "checked");
                    }
                }

                //var radio = htmlHelper.RadioButtonFor(expression, description, attributes).ToHtmlString();
                var radio = htmlHelper.RadioButtonFor(expression, name, attributes).ToHtmlString();

                sb.AppendFormat((isHorizontal != null && isHorizontal == true) ?
                    "<label for=\"{0}\" class='radio-inline " + selectedFlag + "'>{1}{2}</label>" :
                    "<label class='radio " + selectedFlag + " for=\"{0}\">{1}{2}</label>", id, radio, HttpUtility.HtmlEncode(description));
            }

            sb = sb.AppendFormat("</div>");

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString RadioButtonSwitchEnum(this HtmlHelper htmlHelper, string controlName, Type enumType, int? selectedValue = null, object htmlAttribute = null)
        {
            var names = Enum.GetNames(enumType);
            var sb = new StringBuilder();

            sb = sb.AppendFormat("<div class='btn-group btn-toggle radioList' data-toggle='buttons'>");

            // Create a radio button for each item in the list
            foreach (var name in names)
            {
                var labelText = name;

                var memInfo = enumType.GetMember(name);

                var attributes = memInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);

                if (attributes.Length > 0)
                {
                    labelText = ((DisplayAttribute)attributes[0]).ResourceType != null ? ((DisplayAttribute)attributes[0]).GetName() : ((DisplayAttribute)attributes[0]).Name;
                }

                var value = Convert.ToInt32(names.Where(e => e.StartsWith(name)).Select(e => (int)Enum.Parse(enumType, e)).FirstOrDefault());
                // Create tag builder
                var radio = new TagBuilder("input");
                radio.Attributes.Add("id", $"{controlName}_{name}");
                radio.Attributes.Add("name", controlName);
                radio.Attributes.Add("type", "radio");
                radio.Attributes.Add("value", value.ToString());

                string checkedFlag, selectedFlag;
                if (selectedValue != null && selectedValue > 0)
                {
                    if (value == selectedValue)
                    {
                        checkedFlag = "checked";
                        selectedFlag = "class='btn btn-sm btn-success active'";
                        radio.Attributes.Add("checked", checkedFlag);
                    }
                    else
                    {
                        selectedFlag = "class='btn btn-sm btn-default'";
                    }
                }
                else
                {
                    if (names.First() == name)
                    {
                        checkedFlag = "checked";
                        selectedFlag = "class='btn btn-sm btn-success active'";
                        radio.Attributes.Add("checked", checkedFlag);
                    }
                    else
                    {
                        selectedFlag = "class='btn btn-sm btn-default'";
                    }
                }

                // Add attributes                    
                radio.MergeAttributes(new RouteValueDictionary(htmlAttribute));
                radio.ToString();

                sb.AppendFormat("<label {0}>{1}{2}</label>", selectedFlag, radio, HttpUtility.HtmlEncode(labelText));
            }

            sb = sb.AppendFormat("</div>");

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString RadioButtonSwitchEnumFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object selectedValue = null, bool? isHorizontal = true, object htmlAttribute = null)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (!metaData.ModelType.IsEnum)
            {
                throw new ArgumentException("This helper is intended to be used with enum types");
            }

            // get the field name
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            // get full field (template may have prefix)
            var propertyName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            string ctrlName = propertyName.Replace(".", "");

            var names = Enum.GetNames(metaData.ModelType);
            var values = Enum.GetValues(metaData.ModelType);

            var sb = new StringBuilder();

            // Create a radio button for each item in the list
            foreach (var name in names)
            {
                string description = string.Empty;
                var propertyValue = names.Where(e => e.StartsWith(name)).Select(e => (int)Enum.Parse(metaData.ModelType, e)).FirstOrDefault();

                var memInfo = metaData.ModelType.GetMember(name);
                var attributes = memInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
                if (attributes.Length > 0)
                {
                    description = ((DisplayAttribute)attributes[0]).ResourceType != null ? ((DisplayAttribute)attributes[0]).GetName() : name;
                }

                // Create tag builder
                var radio = new TagBuilder("input");
                radio.Attributes.Add("id", $"{propertyName}_{name}");
                radio.Attributes.Add("name", ctrlName);
                radio.Attributes.Add("type", "radio");
                radio.Attributes.Add("value", propertyValue.ToString());

                string selectedFlag;
                if (selectedValue != null)
                {
                    if (propertyValue == Convert.ToInt32(selectedValue))
                    {
                        selectedFlag = "class='btn btn-sm btn-success active'";
                        radio.Attributes.Add("checked", "checked");
                    }
                    else
                    {
                        selectedFlag = "class='btn btn-sm btn-default'";
                    }
                }
                else
                {
                    var selectedPropertyName = metaData.Model;
                    if (selectedPropertyName != null)
                    {
                        if (name == selectedPropertyName.ToString())
                        {
                            selectedFlag = "class='btn btn-sm btn-success active'";
                            radio.Attributes.Add("checked", "checked");
                        }
                        else
                        {
                            selectedFlag = "class='btn btn-sm btn-default'";
                        }
                    }
                    else
                    {
                        if (names.First() == name)
                        {
                            selectedFlag = "class='btn btn-sm btn-success active'";
                            radio.Attributes.Add("checked", "checked");
                        }
                        else
                        {
                            selectedFlag = "class='btn btn-sm btn-default'";
                        }
                    }
                }

                // Add attributes                    
                radio.MergeAttributes(new RouteValueDictionary(htmlAttribute));
                radio.ToString();

                sb.AppendFormat("<label {0}>{1}{2}</label>", selectedFlag, radio, HttpUtility.HtmlEncode(description));
            }

            string result = string.Format("<div class='btn-group btn-toggle radioList' data-toggle='buttons'>{0}</div>", sb.ToString());
            return MvcHtmlString.Create(result);
        }

        //public static MvcHtmlString EnumRadioButtonExtFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper,Expression<Func<TModel, TEnum>> expression,Position position = Position.Horizontal)
        //{
        //    var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
        //    string fullName = ExpressionHelper.GetExpressionText(expression);
        //    var sb = new StringBuilder();

        //    Type enumType = GetEnumTypeFromAttribute(expression);

        //    var listOfValues = from TEnum e in Enum.GetValues(enumType)
        //        select new SelectListItem { Value = e.ToString(), Text = Enum.GetName(typeof(TEnum), e) };

        //    // Create a radio button for each item in the list 
        //    foreach (SelectListItem item in listOfValues)
        //    {
        //        // Generate an id to be given to the radio button field 
        //        var id = $"rb_{fullName.Replace("[", "").Replace("]", "").Replace(".", "_")}_{item.Value}";

        //        // Create and populate a radio button using the existing html helpers 
        //        var label = htmlHelper.Label(id, HttpUtility.HtmlEncode(item.Text));
        //        //var radio = htmlHelper.RadioButtonFor(expression, item.Value, new { id = id }).ToHtmlString();
        //        var radio = htmlHelper.RadioButton(fullName, item.Value, item.Selected, new { id = id }).ToHtmlString();

        //        // Create the html string that will be returned to the client 
        //        // e.g. <input data-val="true" data-val-required=
        //        //   "You must select an option" id="TestRadio_1" 
        //        // name = "TestRadio" type = "radio"
        //        //   value="1" /><label for="TestRadio_1">Line1</label> 
        //        sb.AppendFormat("<{2} class=\"RadioButton\">{0}{1}</{2}>",
        //           radio, label, (position == Position.Horizontal ? "span" : "div"));

        //    }

        //    return MvcHtmlString.Create(sb.ToString());

        //}

    }
}
