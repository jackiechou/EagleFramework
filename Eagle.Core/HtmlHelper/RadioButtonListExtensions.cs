using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Routing;
using Eagle.Core.HtmlHelper.CheckBox.Model;
using Eagle.Resources;

namespace System.Web.Mvc
{
    public static class RadioButtonListExtensions
    {
        #region -- RadioButtonList (Horizontal) --
        /// <summary>
        /// RadioButtonList.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="listInfo">RadioButtonListInfo.</param>
        /// <returns></returns>
        public static MvcHtmlString RadioButtonList(this HtmlHelper htmlHelper,
             string name,
           IEnumerable<SelectListItem> listInfo)
        {
            return htmlHelper.RadioButtonList(name, listInfo, null, 0);
        }

        /// <summary>
        /// RadioButtonList.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="listInfo">RadioButtonListInfo.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString RadioButtonList(this HtmlHelper htmlHelper,
             string name,
            IEnumerable<SelectListItem> listInfo,
            object htmlAttributes)
        {
            return htmlHelper.RadioButtonList(name,listInfo,new RouteValueDictionary(htmlAttributes),0);
        }


        /// <summary>
        /// RadioButtonList.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="listInfo">The list info.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="columnNumber">Row</param>
        /// <returns></returns>
        public static MvcHtmlString RadioButtonList(this HtmlHelper htmlHelper,
           string name,
           IEnumerable<SelectListItem> listInfo,
           IDictionary<string, object> htmlAttributes, 
           int? columnNumber = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(LanguageResource.RadioButtonListExtensions_RadioButtonList_ArgumentException_RadioButtonList_Tag_Name, "name");
            }
            var lst = listInfo as SelectListItem[] ?? listInfo.ToArray();
            if (lst == null || !lst.Any())
            {
                throw new ArgumentNullException("listInfo", LanguageResource.NoReferenceList);
            }

            StringBuilder sb = new StringBuilder();
            int lineNumber = 0;
            string str = string.Empty;

            foreach (SelectListItem info in lst)
            {
                lineNumber++;

                TagBuilder inputBuilder = new TagBuilder("input");
                if (info.Selected)
                {
                    inputBuilder.MergeAttribute("checked", "checked");
                }
                inputBuilder.MergeAttributes(htmlAttributes);
                inputBuilder.MergeAttribute("type", "radio");
                inputBuilder.MergeAttribute("value", info.Value);
                inputBuilder.MergeAttribute("name", name);

                TagBuilder labelBuilder = new TagBuilder("label");
                labelBuilder.Attributes.Add("class", info.Selected ? "btn btn-success active" : "btn btn-default");
                labelBuilder.MergeAttribute("for", name);
                labelBuilder.InnerHtml = $"{inputBuilder.ToString(TagRenderMode.Normal)} {info.Text}";
                
                if (columnNumber ==null || columnNumber == 0 || (lineNumber%columnNumber == 0))
                {
                    str+= labelBuilder.ToString(TagRenderMode.Normal);
                }
                else
                {
                    TagBuilder divRadioBuilder = new TagBuilder("div");
                    divRadioBuilder.Attributes.Add("class", "radio");
                    divRadioBuilder.InnerHtml = labelBuilder.ToString(TagRenderMode.Normal);
                    str += divRadioBuilder.ToString(TagRenderMode.Normal);
                }
            }

            TagBuilder containerBuilder = new TagBuilder("div");
            containerBuilder.Attributes.Add("class", "btn-group btn-toggle radioList");
            containerBuilder.Attributes.Add("data-toggle", "buttons");
            containerBuilder.InnerHtml = str;
            sb.Append(containerBuilder.ToString(TagRenderMode.Normal));

            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion

        #region -- RadioButtonVertical --
        /// <summary>
        /// Checks the box list vertical.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="listInfo">The list info.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="columnNumber">The column number.</param>
        /// <returns></returns>
        public static MvcHtmlString RadioButtonVertical(this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> listInfo,
            IDictionary<string, object> htmlAttributes,
            int columnNumber = 1)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentException(LanguageResource.NoFoundTagName, "name");
            }
            var lst = listInfo as SelectListItem[] ?? listInfo.ToArray();
            if (lst == null || !lst.Any())
            {
                throw new ArgumentNullException("listInfo", LanguageResource.NoReferenceList);
            }

            int dataCount = lst.Count();

            // calculate number of rows
            int rows = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(dataCount) / Convert.ToDecimal(columnNumber)));
            if (dataCount <= columnNumber || dataCount - columnNumber == 1)
            {
                rows = dataCount;
            }

            TagBuilder wrapBuilder = new TagBuilder("div");
            wrapBuilder.MergeAttribute("style", "float: left; light-height: 25px; padding-right: 5px;");

            string wrapStart = wrapBuilder.ToString(TagRenderMode.StartTag);
            string wrapClose = string.Concat(wrapBuilder.ToString(TagRenderMode.EndTag), " <div style=\"clear:both;\"></div>");
            string wrapBreak = string.Concat("</div>", wrapBuilder.ToString(TagRenderMode.StartTag));

            StringBuilder sb = new StringBuilder();
            sb.Append(wrapStart);

            int lineNumber = 0;

            foreach (var info in lst)
            {
                TagBuilder builder = new TagBuilder("input");
                if (info.Selected)
                {
                    builder.MergeAttribute("checked", "checked");
                }
                builder.MergeAttributes(htmlAttributes);
                builder.MergeAttribute("type", "radio");
                builder.MergeAttribute("value", info.Value);
                builder.MergeAttribute("name", name);
                sb.Append(builder.ToString(TagRenderMode.Normal));

                TagBuilder labelBuilder = new TagBuilder("label");
                labelBuilder.MergeAttribute("for", name);
                labelBuilder.InnerHtml = info.Text;
                sb.Append(labelBuilder.ToString(TagRenderMode.Normal));

                lineNumber++;

                if (lineNumber.Equals(rows))
                {
                    sb.Append(wrapBreak);
                    lineNumber = 0;
                }
                else
                {
                    sb.Append("<br/>");
                }
            }
            sb.Append(wrapClose);
            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion

        #region -- RadioButtonList (Horizonal, Vertical) --

        /// <summary>
        /// Checks the box list.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="listInfo">The list info.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="position">The position.</param>
        /// <param name="number">Position.Horizontal Row, Position.Vertical Column</param>
        /// <returns></returns>
        public static MvcHtmlString RadioButtonList(this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> listInfo,
            Position? position,
            int? number = 0,
            IDictionary<string, object> htmlAttributes = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(LanguageResource.NoFoundTagName, "name");
            }


            var selectListItems = listInfo as SelectListItem[] ?? listInfo.ToArray();
            if (listInfo == null || !selectListItems.Any())
            {
                throw new ArgumentNullException("listInfo", LanguageResource.RadioButtonListExtensions_RadioButtonList_NullReferenceList);
            }

            StringBuilder sb = new StringBuilder();
            int lineNumber = 0;

            switch (position)
            {
                case Position.Horizontal:

                    foreach (SelectListItem item in selectListItems)
                    {
                        lineNumber++;

                        var id = $"{name}_{item.Value}";
                        var attributes = new Dictionary<string, object> {{"id", id}};

                        var firstOrDefault = selectListItems.FirstOrDefault();
                        if (firstOrDefault != null && item.Value == firstOrDefault.Value)
                        {
                            attributes.Add("checked", "checked");
                        }

                        if (htmlAttributes != null)
                        {
                            foreach (var attribute in htmlAttributes)
                            {
                                attributes.Add(attribute.Key, attribute.Value);
                            }
                        }

                        sb.Append(CreateRadioButtonItem(item, name, attributes));

                        if (number != null && number != 0 && (lineNumber % number != 0))
                        {
                            sb.Append("<br />");
                        }
                    }
                    break;

                case Position.Vertical:

                    int dataCount = selectListItems.Count();

                    //(rows)
                    int rows = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(dataCount) / Convert.ToDecimal(number)));
                    if (dataCount <= number || dataCount - number == 1)
                    {
                        rows = dataCount;
                    }

                    TagBuilder wrapBuilder = new TagBuilder("div");
                    wrapBuilder.MergeAttribute("style", "float: left; light-height: 25px; padding-right: 5px;");

                    string wrapStart = wrapBuilder.ToString(TagRenderMode.StartTag);
                    string wrapClose = string.Concat(wrapBuilder.ToString(TagRenderMode.EndTag), " <div style=\"clear:both;\"></div>");
                    string wrapBreak = string.Concat("</div>", wrapBuilder.ToString(TagRenderMode.StartTag));

                    sb.Append(wrapStart);

                    foreach (SelectListItem item in selectListItems)
                    {
                        lineNumber++;

                        var id = $"{name}_{item.Value}";
                        var attributes = new Dictionary<string, object> { { "id", id } };

                        var firstOrDefault = selectListItems.FirstOrDefault();
                        if (firstOrDefault != null && item.Value == firstOrDefault.Value)
                        {
                            attributes.Add("checked", "checked");
                        }

                        if (htmlAttributes != null)
                        {
                            foreach (var attribute in htmlAttributes)
                            {
                                attributes.Add(attribute.Key, attribute.Value);
                            }
                        }

                        sb.Append(CreateRadioButtonItem(item, name, attributes));

                        if (lineNumber.Equals(rows))
                        {
                            sb.Append(wrapBreak);
                            lineNumber = 0;
                        }
                        else
                        {
                            sb.Append("<br/>");
                        }
                    }
                    sb.Append(wrapClose);
                    break;
                default:
                    foreach (SelectListItem item in selectListItems)
                    {
                        lineNumber++;

                        var id = $"{name}_{item.Value}";
                        var attributes = new Dictionary<string, object> { { "id", id } };

                        var firstOrDefault = selectListItems.FirstOrDefault();
                        if (firstOrDefault != null && item.Value == firstOrDefault.Value)
                        {
                            attributes.Add("checked", "checked");
                        }

                        if (htmlAttributes != null)
                        {
                            foreach (var attribute in htmlAttributes)
                            {
                                attributes.Add(attribute.Key, attribute.Value);
                            }
                        }

                        sb.Append(CreateRadioButtonItem(item, name, attributes));

                        if (number != null && number != 0 && (lineNumber % number != 0))
                        {
                            sb.Append("<br />");
                        }
                    }
                    break;
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        /// CreateRadioButtonItem
        /// </summary>
        /// <param name="info"></param>
        /// <param name="name"></param>
        /// <param name="includeLabel"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        internal static string CreateRadioButtonItem(
            SelectListItem info,
            string name, 
            IDictionary<string, object> htmlAttributes=null, 
            bool? includeLabel = true)
        {
            StringBuilder sb = new StringBuilder();
            var sbLabelClass = new StringBuilder();

            TagBuilder builder = new TagBuilder("input");
            if (info.Selected)
            {
                builder.MergeAttribute("checked", "checked");
                sbLabelClass.Append("btn btn-success active ");
            }
            else
            {
                sbLabelClass.Append("btn btn-default");
            }

            if (htmlAttributes!=null)
            {
                builder.MergeAttributes(htmlAttributes);
            }

            builder.MergeAttribute("type", "radio");
            builder.MergeAttribute("value", info.Value);
            builder.MergeAttribute("name", name);
            if (includeLabel != null && includeLabel == true)
            {
                TagBuilder labelBuilder = new TagBuilder("label");
                labelBuilder.MergeAttribute("for", name);
                labelBuilder.MergeAttribute("class", sbLabelClass.ToString());
                labelBuilder.InnerHtml = builder.ToString(TagRenderMode.Normal) + info.Text;
                sb.Append(labelBuilder.ToString(TagRenderMode.Normal));
            }
            else
            {
                sb.Append(builder.ToString(TagRenderMode.Normal));
            }

            return sb.ToString();
        }
        #endregion

        #region RadioButtonListFor
        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> listInfo,
            string selectedItemValue = "", IDictionary<string, object> htmlAttributes = null,
            Position? position = Position.Horizontal,
            int? number = 0)

        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var sb = new StringBuilder();
            int lineNumber = 0;

            var lst = listInfo as SelectListItem[] ?? listInfo.ToArray();
            if (lst.Any())
            {
                switch (position)
                {
                    case Position.Horizontal:
                        foreach (SelectListItem item in lst)
                        {
                            lineNumber++;

                            // Generate an id to be given to the radio button field 
                            var id = $"{metaData.PropertyName}_{item.Value}";
                            var name = metaData.PropertyName;

                            var attributes = new Dictionary<string, object> { { "id", id } };

                            if (!string.IsNullOrEmpty(selectedItemValue))
                            {
                                if (selectedItemValue == item.Value)
                                {
                                    attributes.Add("checked", "checked");
                                }
                            }
                            else
                            {
                                var firstItem = lst.FirstOrDefault();
                                if (firstItem != null && item.Value == firstItem.Value)
                                {
                                    attributes.Add("checked", "checked");
                                }
                            }

                            if (htmlAttributes != null)
                            {
                                foreach (var attribute in htmlAttributes)
                                {
                                    attributes.Add(attribute.Key, attribute.Value);
                                }
                            }

                            sb.Append(CreateRadioButtonItem(item, name, attributes));

                            if (number != null && number != 0 && (lineNumber%number != 0))
                            {
                                sb.Append("<br />");
                            }
                        }
                        break;

                    case Position.Vertical:
                        int dataCount = lst.Count();

                        if (number == null)
                            number = 0;

                        //(rows)
                        int rows = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(dataCount) / Convert.ToDecimal(number)));
                        if (dataCount <= number || dataCount - number == 1)
                        {
                            rows = dataCount;
                        }

                        TagBuilder wrapBuilder = new TagBuilder("div");
                        wrapBuilder.MergeAttribute("style", "float: left; light-height: 25px; padding-right: 5px;");

                        string wrapStart = wrapBuilder.ToString(TagRenderMode.StartTag);
                        string wrapClose = string.Concat(wrapBuilder.ToString(TagRenderMode.EndTag), " <div style=\"clear:both;\"></div>");
                        string wrapBreak = string.Concat("</div>", wrapBuilder.ToString(TagRenderMode.StartTag));

                        sb.Append(wrapStart);

                        foreach (SelectListItem item in lst)
                        {
                            lineNumber++;

                            // Generate an id to be given to the radio button field 
                            var id = $"{metaData.PropertyName}_{item.Value}";
                            var name = metaData.PropertyName;

                            var attributes = new Dictionary<string, object> { { "id", id } };

                            if (!string.IsNullOrEmpty(selectedItemValue))
                            {
                                if (selectedItemValue == item.Value)
                                {
                                    attributes.Add("checked", "checked");
                                }
                            }
                            else
                            {
                                var firstItem = lst.FirstOrDefault();
                                if (firstItem != null && item.Value == firstItem.Value)
                                {
                                    attributes.Add("checked", "checked");
                                }
                            }

                            if (htmlAttributes != null)
                            {
                                foreach (var attribute in htmlAttributes)
                                {
                                    attributes.Add(attribute.Key, attribute.Value);
                                }
                            }

                            sb.Append(CreateRadioButtonItem(item, name, attributes));

                            if (lineNumber.Equals(rows))
                            {
                                sb.Append(wrapBreak);
                                lineNumber = 0;
                            }
                        }
                        sb.Append(wrapClose);
                        break;
                    default:
                        foreach (SelectListItem item in lst)
                        {
                            lineNumber++;

                            // Generate an id to be given to the radio button field 
                            var id = $"{metaData.PropertyName}_{item.Value}";
                            var name = metaData.PropertyName;

                            var attributes = new Dictionary<string, object> { { "id", id } };

                            if (!string.IsNullOrEmpty(selectedItemValue))
                            {
                                if (selectedItemValue == item.Value)
                                {
                                    attributes.Add("checked", "checked");
                                }
                            }
                            else
                            {
                                var firstOrDefault = lst.FirstOrDefault();
                                if (firstOrDefault != null && item.Value == firstOrDefault.Value)
                                {
                                    attributes.Add("checked", "checked");
                                }
                            }

                            if (htmlAttributes != null)
                            {
                                foreach (var attribute in htmlAttributes)
                                {
                                    attributes.Add(attribute.Key, attribute.Value);
                                }
                            }

                            sb.Append(CreateRadioButtonItem(item, name, attributes));

                            if (number != null && number != 0 && (lineNumber % number != 0))
                            {
                                sb.Append("<br />");
                            }
                        }
                        break;
                }
               
            }

            TagBuilder containerBuilder = new TagBuilder("div");
            containerBuilder.Attributes.Add("class", "btn-group btn-toggle radioList");
            containerBuilder.Attributes.Add("data-toggle", "buttons");
            containerBuilder.InnerHtml = sb.ToString();

            return MvcHtmlString.Create(containerBuilder.ToString(TagRenderMode.Normal));
        }
        #endregion

    }
}
