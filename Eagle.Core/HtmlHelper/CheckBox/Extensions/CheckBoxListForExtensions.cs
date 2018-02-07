﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using Eagle.Core.HtmlHelper.CheckBox.Internal.Helpers;
using Eagle.Core.HtmlHelper.CheckBox.Internal.Model;
using Eagle.Core.HtmlHelper.CheckBox.Model;

namespace Eagle.Core.HtmlHelper.CheckBox.Extensions
{
    public static class CheckBoxListForExtensions
    {
        private static ListBuilder _listBuilder;
        static CheckBoxListForExtensions()
        {
            _listBuilder = new ListBuilder();
        }
       
        // Base extension

        /// <summary>
        /// Generates Model-based list of checkboxes (For...)
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <typeparam name="TProperty">ViewModel property</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listNameExpr">ViewModel Item type to serve as a name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValuesExpr">Data list of selected items (should be of same data type as a source list)</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty, TItem, TValue, TKey>
          (this HtmlHelper<TModel> htmlHelper,
           Expression<Func<TModel, TProperty>> listNameExpr,
           Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr,
           Expression<Func<TItem, TValue>> valueExpr,
           Expression<Func<TItem, TKey>> textToDisplayExpr,
           Expression<Func<TModel, IEnumerable<TItem>>> selectedValuesExpr)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(listNameExpr, htmlHelper.ViewData);
            return _listBuilder.CheckBoxList
              (new listConstructor
                 <TModel, TItem, TValue, TKey>
              {
                  htmlHelper = htmlHelper,
                  modelMetadata = modelMetadata,
                  listName = listNameExpr.toProperty(),
                  sourceDataExpr = sourceDataExpr,
                  valueExpr = valueExpr,
                  textToDisplayExpr = textToDisplayExpr,
                  selectedValuesExpr = selectedValuesExpr,
              });
        }
        /// <summary>
        /// Generates Model-based list of checkboxes (For...)
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <typeparam name="TProperty">ViewModel property</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listNameExpr">ViewModel Item type to serve as a name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValueExpr">Boolean value from db or selector corresponding to each item to be selected</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty, TItem, TValue, TKey>
          (this HtmlHelper<TModel> htmlHelper,
           Expression<Func<TModel, TProperty>> listNameExpr,
           Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr,
           Expression<Func<TItem, TValue>> valueExpr,
           Expression<Func<TItem, TKey>> textToDisplayExpr,
           Expression<Func<TItem, bool>> selectedValueExpr)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(listNameExpr, htmlHelper.ViewData);
            return _listBuilder.CheckBoxList
              (new listConstructor
                 <TModel, TItem, TValue, TKey>
              {
                  htmlHelper = htmlHelper,
                  modelMetadata = modelMetadata,
                  listName = listNameExpr.toProperty(),
                  sourceDataExpr = sourceDataExpr,
                  valueExpr = valueExpr,
                  textToDisplayExpr = textToDisplayExpr,
                  selectedValueExpr = selectedValueExpr,
              });
        }

        // Level 1

        /// <summary>
        /// Generates Model-based list of checkboxes (For...)
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <typeparam name="TProperty">ViewModel property</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listNameExpr">ViewModel Item type to serve as a name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValuesExpr">Data list of selected items (should be of same data type as a source list)</param>
        /// <param name="position">Direction of the list (e.g. 'Position2.Horizontal' or 'Position2.Vertical')</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty, TItem, TValue, TKey>
          (this HtmlHelper<TModel> htmlHelper,
           Expression<Func<TModel, TProperty>> listNameExpr,
           Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr,
           Expression<Func<TItem, TValue>> valueExpr,
           Expression<Func<TItem, TKey>> textToDisplayExpr,
           Expression<Func<TModel, IEnumerable<TItem>>> selectedValuesExpr,
           Position position)
        {
            return _listBuilder.CheckBoxList
              (new listConstructor
                 <TModel, TItem, TValue, TKey>
              {
                  htmlHelper = htmlHelper,
                  modelMetadata = ModelMetadata.FromLambdaExpression(listNameExpr, htmlHelper.ViewData),
                  listName = listNameExpr.toProperty(),
                  sourceDataExpr = sourceDataExpr,
                  valueExpr = valueExpr,
                  textToDisplayExpr = textToDisplayExpr,
                  selectedValuesExpr = selectedValuesExpr,
                  position = position
              });
        }
        /// <summary>
        /// Generates Model-based list of checkboxes (For...)
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <typeparam name="TProperty">ViewModel property</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listNameExpr">ViewModel Item type to serve as a name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValueExpr">Boolean value from db or selector corresponding to each item to be selected</param>
        /// <param name="position">Direction of the list (e.g. 'Position2.Horizontal' or 'Position2.Vertical')</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty, TItem, TValue, TKey>
          (this HtmlHelper<TModel> htmlHelper,
           Expression<Func<TModel, TProperty>> listNameExpr,
           Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr,
           Expression<Func<TItem, TValue>> valueExpr,
           Expression<Func<TItem, TKey>> textToDisplayExpr,
           Expression<Func<TItem, bool>> selectedValueExpr,
           Position position)
        {
            return _listBuilder.CheckBoxList
              (new listConstructor
                 <TModel, TItem, TValue, TKey>
              {
                  htmlHelper = htmlHelper,
                  modelMetadata = ModelMetadata.FromLambdaExpression(listNameExpr, htmlHelper.ViewData),
                  listName = listNameExpr.toProperty(),
                  sourceDataExpr = sourceDataExpr,
                  valueExpr = valueExpr,
                  textToDisplayExpr = textToDisplayExpr,
                  selectedValueExpr = selectedValueExpr,
                  position = position
              });
        }

        // Level 2

        /// <summary>
        /// Generates Model-based list of checkboxes (For...)
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <typeparam name="TProperty">ViewModel property</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listNameExpr">ViewModel Item type to serve as a name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValuesExpr">Data list of selected items (should be of same data type as a source list)</param>
        /// <param name="htmlAttributesExpr">Data list HTML tag attributes for each checkbox</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty, TItem, TValue, TKey>
          (this HtmlHelper<TModel> htmlHelper,
           Expression<Func<TModel, TProperty>> listNameExpr,
           Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr,
           Expression<Func<TItem, TValue>> valueExpr,
           Expression<Func<TItem, TKey>> textToDisplayExpr,
           Expression<Func<TModel, IEnumerable<TItem>>> selectedValuesExpr,
           Expression<Func<TItem, object>> htmlAttributesExpr)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(listNameExpr, htmlHelper.ViewData);
            return _listBuilder.CheckBoxList
              (new listConstructor
                 <TModel, TItem, TValue, TKey>
              {
                  htmlHelper = htmlHelper,
                  modelMetadata = modelMetadata,
                  listName = listNameExpr.toProperty(),
                  sourceDataExpr = sourceDataExpr,
                  valueExpr = valueExpr,
                  textToDisplayExpr = textToDisplayExpr,
                  htmlAttributesExpr = htmlAttributesExpr,
                  selectedValuesExpr = selectedValuesExpr,
              });
        }
        /// <summary>
        /// Generates Model-based list of checkboxes (For...)
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <typeparam name="TProperty">ViewModel property</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listNameExpr">ViewModel Item type to serve as a name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValueExpr">Boolean value from db or selector corresponding to each item to be selected</param>
        /// <param name="htmlAttributesExpr">Data list HTML tag attributes for each checkbox</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty, TItem, TValue, TKey>
          (this HtmlHelper<TModel> htmlHelper,
           Expression<Func<TModel, TProperty>> listNameExpr,
           Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr,
           Expression<Func<TItem, TValue>> valueExpr,
           Expression<Func<TItem, TKey>> textToDisplayExpr,
           Expression<Func<TItem, bool>> selectedValueExpr,
           Expression<Func<TItem, object>> htmlAttributesExpr)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(listNameExpr, htmlHelper.ViewData);
            return _listBuilder.CheckBoxList
              (new listConstructor
                 <TModel, TItem, TValue, TKey>
              {
                  htmlHelper = htmlHelper,
                  modelMetadata = modelMetadata,
                  listName = listNameExpr.toProperty(),
                  sourceDataExpr = sourceDataExpr,
                  valueExpr = valueExpr,
                  textToDisplayExpr = textToDisplayExpr,
                  htmlAttributesExpr = htmlAttributesExpr,
                  selectedValueExpr = selectedValueExpr,
              });
        }

        // Level 3

        /// <summary>
        /// Generates Model-based list of checkboxes (For...)
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <typeparam name="TProperty">ViewModel property</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listNameExpr">ViewModel Item type to serve as a name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValuesExpr">Data list of selected items (should be of same data type as a source list)</param>
        /// <param name="position">Direction of the list (e.g. 'Position2.Horizontal' or 'Position2.Vertical')</param>
        /// <param name="htmlAttributesExpr">Applies custom HTML tag attributes to each checkbox/label combo if defined locally (e.g.: entity => new { tagName = "tagValue" }), or to particular combos, if defined in the database (e.g.: entity => entity.TagsDbObject)</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty, TItem, TValue, TKey>
          (this HtmlHelper<TModel> htmlHelper,
           Expression<Func<TModel, TProperty>> listNameExpr,
           Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr,
           Expression<Func<TItem, TValue>> valueExpr,
           Expression<Func<TItem, TKey>> textToDisplayExpr,
           Expression<Func<TModel, IEnumerable<TItem>>> selectedValuesExpr,
           Position position,
           Expression<Func<TItem, object>> htmlAttributesExpr)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(listNameExpr, htmlHelper.ViewData);
            return _listBuilder.CheckBoxList
              (new listConstructor
                 <TModel, TItem, TValue, TKey>
              {
                  htmlHelper = htmlHelper,
                  modelMetadata = modelMetadata,
                  listName = listNameExpr.toProperty(),
                  sourceDataExpr = sourceDataExpr,
                  valueExpr = valueExpr,
                  textToDisplayExpr = textToDisplayExpr,
                  htmlAttributesExpr = htmlAttributesExpr,
                  selectedValuesExpr = selectedValuesExpr,
                  position = position
              });
        }
        /// <summary>
        /// Generates Model-based list of checkboxes (For...)
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <typeparam name="TProperty">ViewModel property</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listNameExpr">ViewModel Item type to serve as a name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValueExpr">Boolean value from db or selector corresponding to each item to be selected</param>
        /// <param name="position">Direction of the list (e.g. 'Position2.Horizontal' or 'Position2.Vertical')</param>
        /// <param name="htmlAttributesExpr">Applies custom HTML tag attributes to each checkbox/label combo if defined locally (e.g.: entity => new { tagName = "tagValue" }), or to particular combos, if defined in the database (e.g.: entity => entity.TagsDbObject)</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty, TItem, TValue, TKey>
          (this HtmlHelper<TModel> htmlHelper,
           Expression<Func<TModel, TProperty>> listNameExpr,
           Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr,
           Expression<Func<TItem, TValue>> valueExpr,
           Expression<Func<TItem, TKey>> textToDisplayExpr,
           Expression<Func<TItem, bool>> selectedValueExpr,
           Position position,
           Expression<Func<TItem, object>> htmlAttributesExpr)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(listNameExpr, htmlHelper.ViewData);
            return _listBuilder.CheckBoxList
              (new listConstructor
                 <TModel, TItem, TValue, TKey>
              {
                  htmlHelper = htmlHelper,
                  modelMetadata = modelMetadata,
                  listName = listNameExpr.toProperty(),
                  sourceDataExpr = sourceDataExpr,
                  valueExpr = valueExpr,
                  textToDisplayExpr = textToDisplayExpr,
                  htmlAttributesExpr = htmlAttributesExpr,
                  selectedValueExpr = selectedValueExpr,
                  position = position
              });
        }

        // Level 4 - Full

        /// <summary>
        /// Generates Model-based list of checkboxes (For...)
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <typeparam name="TProperty">ViewModel property</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listNameExpr">ViewModel Item type to serve as a name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValuesExpr">Data list of selected items (should be of same data type as a source list)</param>
        /// <param name="htmlAttributes">Applies custom HTML tag attributes to each checkbox/label combo</param>
        /// <param name="disabledValues">String array of values to disable</param>
        /// <param name="position">Direction of the list (e.g. 'Position2.Horizontal' or 'Position2.Vertical')</param>
        /// <param name="htmlAttributesExpr">Applies custom HTML tag attributes to each checkbox/label combo if defined locally (e.g.: entity => new { tagName = "tagValue" }), or to particular combos, if defined in the database (e.g.: entity => entity.TagsDbObject)</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty, TItem, TValue, TKey>
          (this HtmlHelper<TModel> htmlHelper,
           Expression<Func<TModel, TProperty>> listNameExpr,
           Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr,
           Expression<Func<TItem, TValue>> valueExpr,
           Expression<Func<TItem, TKey>> textToDisplayExpr,
           Expression<Func<TModel, IEnumerable<TItem>>> selectedValuesExpr,
           object htmlAttributes,
           string[] disabledValues,
           Position position,
           Expression<Func<TItem, object>> htmlAttributesExpr)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(listNameExpr, htmlHelper.ViewData);
            return _listBuilder.CheckBoxList
              (new listConstructor
                 <TModel, TItem, TValue, TKey>
              {
                  htmlHelper = htmlHelper,
                  modelMetadata = modelMetadata,
                  listName = listNameExpr.toProperty(),
                  sourceDataExpr = sourceDataExpr,
                  valueExpr = valueExpr,
                  textToDisplayExpr = textToDisplayExpr,
                  htmlAttributesExpr = htmlAttributesExpr,
                  selectedValuesExpr = selectedValuesExpr,
                  htmlAttributes = htmlAttributes,
                  disabledValues = disabledValues,
                  position = position
              });
        }
        /// <summary>
        /// Generates Model-based list of checkboxes (For...)
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <typeparam name="TProperty">ViewModel property</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listNameExpr">ViewModel Item type to serve as a name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValueExpr">Boolean value from db or selector corresponding to each item to be selected</param>
        /// <param name="htmlAttributes">Applies custom HTML tag attributes to each checkbox/label combo</param>
        /// <param name="disabledValues">String array of values to disable</param>
        /// <param name="position">Direction of the list (e.g. 'Position2.Horizontal' or 'Position2.Vertical')</param>
        /// <param name="htmlAttributesExpr">Applies custom HTML tag attributes to each checkbox/label combo if defined locally (e.g.: entity => new { tagName = "tagValue" }), or to particular combos, if defined in the database (e.g.: entity => entity.TagsDbObject)</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty, TItem, TValue, TKey>
          (this HtmlHelper<TModel> htmlHelper,
           Expression<Func<TModel, TProperty>> listNameExpr,
           Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr,
           Expression<Func<TItem, TValue>> valueExpr,
           Expression<Func<TItem, TKey>> textToDisplayExpr,
           Expression<Func<TItem, bool>> selectedValueExpr,
           object htmlAttributes,
           string[] disabledValues,
           Position position,
           Expression<Func<TItem, object>> htmlAttributesExpr)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(listNameExpr, htmlHelper.ViewData);
            return _listBuilder.CheckBoxList
              (new listConstructor
                 <TModel, TItem, TValue, TKey>
              {
                  htmlHelper = htmlHelper,
                  modelMetadata = modelMetadata,
                  listName = listNameExpr.toProperty(),
                  sourceDataExpr = sourceDataExpr,
                  valueExpr = valueExpr,
                  textToDisplayExpr = textToDisplayExpr,
                  htmlAttributesExpr = htmlAttributesExpr,
                  selectedValueExpr = selectedValueExpr,
                  htmlAttributes = htmlAttributes,
                  disabledValues = disabledValues,
                  position = position
              });
        }

        // Level 1

        /// <summary>
        /// Generates Model-based list of checkboxes (For...)
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <typeparam name="TProperty">ViewModel property</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listNameExpr">ViewModel Item type to serve as a name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValuesExpr">Data list of selected items (should be of same data type as a source list)</param>
        /// <param name="wrapInfo">Settings for HTML wrapper of the list (e.g. 'new HtmlListInfo2(HtmlTag2.vertical_columns, 2, new { style="color:green;" })')</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty, TItem, TValue, TKey>
          (this HtmlHelper<TModel> htmlHelper,
           Expression<Func<TModel, TProperty>> listNameExpr,
           Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr,
           Expression<Func<TItem, TValue>> valueExpr,
           Expression<Func<TItem, TKey>> textToDisplayExpr,
           Expression<Func<TModel, IEnumerable<TItem>>> selectedValuesExpr,
           HtmlListInfo wrapInfo)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(listNameExpr, htmlHelper.ViewData);
            return _listBuilder.CheckBoxList
              (new listConstructor
                 <TModel, TItem, TValue, TKey>
              {
                  htmlHelper = htmlHelper,
                  modelMetadata = modelMetadata,
                  listName = listNameExpr.toProperty(),
                  sourceDataExpr = sourceDataExpr,
                  valueExpr = valueExpr,
                  textToDisplayExpr = textToDisplayExpr,
                  selectedValuesExpr = selectedValuesExpr,
                  htmlListInfo = wrapInfo,
              });
        }
        /// <summary>
        /// Generates Model-based list of checkboxes (For...)
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <typeparam name="TProperty">ViewModel property</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listNameExpr">ViewModel Item type to serve as a name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValueExpr">Boolean value from db or selector corresponding to each item to be selected</param>
        /// <param name="wrapInfo">Settings for HTML wrapper of the list (e.g. 'new HtmlListInfo2(HtmlTag2.vertical_columns, 2, new { style="color:green;" })')</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty, TItem, TValue, TKey>
          (this HtmlHelper<TModel> htmlHelper,
           Expression<Func<TModel, TProperty>> listNameExpr,
           Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr,
           Expression<Func<TItem, TValue>> valueExpr,
           Expression<Func<TItem, TKey>> textToDisplayExpr,
           Expression<Func<TItem, bool>> selectedValueExpr,
           HtmlListInfo wrapInfo)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(listNameExpr, htmlHelper.ViewData);
            return _listBuilder.CheckBoxList
              (new listConstructor
                 <TModel, TItem, TValue, TKey>
              {
                  htmlHelper = htmlHelper,
                  modelMetadata = modelMetadata,
                  listName = listNameExpr.toProperty(),
                  sourceDataExpr = sourceDataExpr,
                  valueExpr = valueExpr,
                  textToDisplayExpr = textToDisplayExpr,
                  selectedValueExpr = selectedValueExpr,
                  htmlListInfo = wrapInfo,
              });
        }

        // Level 2

        /// <summary>
        /// Generates Model-based list of checkboxes (For...)
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <typeparam name="TProperty">ViewModel property</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listNameExpr">ViewModel Item type to serve as a name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValuesExpr">Data list of selected items (should be of same data type as a source list)</param>
        /// <param name="wrapInfo">Settings for HTML wrapper of the list (e.g. 'new HtmlListInfo2(HtmlTag2.vertical_columns, 2, new { style="color:green;" })')</param>
        /// <param name="htmlAttributesExpr">Data list HTML tag attributes for each checkbox</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty, TItem, TValue, TKey>
          (this HtmlHelper<TModel> htmlHelper,
           Expression<Func<TModel, TProperty>> listNameExpr,
           Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr,
           Expression<Func<TItem, TValue>> valueExpr,
           Expression<Func<TItem, TKey>> textToDisplayExpr,
           Expression<Func<TModel, IEnumerable<TItem>>> selectedValuesExpr,
           HtmlListInfo wrapInfo,
           Expression<Func<TItem, object>> htmlAttributesExpr)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(listNameExpr, htmlHelper.ViewData);
            return _listBuilder.CheckBoxList
              (new listConstructor
                 <TModel, TItem, TValue, TKey>
              {
                  htmlHelper = htmlHelper,
                  modelMetadata = modelMetadata,
                  listName = listNameExpr.toProperty(),
                  sourceDataExpr = sourceDataExpr,
                  valueExpr = valueExpr,
                  textToDisplayExpr = textToDisplayExpr,
                  htmlAttributesExpr = htmlAttributesExpr,
                  selectedValuesExpr = selectedValuesExpr,
                  htmlListInfo = wrapInfo,
              });
        }
        /// <summary>
        /// Generates Model-based list of checkboxes (For...)
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <typeparam name="TProperty">ViewModel property</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listNameExpr">ViewModel Item type to serve as a name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValueExpr">Boolean value from db or selector corresponding to each item to be selected</param>
        /// <param name="wrapInfo">Settings for HTML wrapper of the list (e.g. 'new HtmlListInfo2(HtmlTag2.vertical_columns, 2, new { style="color:green;" })')</param>
        /// <param name="htmlAttributesExpr">Data list HTML tag attributes for each checkbox</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty, TItem, TValue, TKey>
          (this HtmlHelper<TModel> htmlHelper,
           Expression<Func<TModel, TProperty>> listNameExpr,
           Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr,
           Expression<Func<TItem, TValue>> valueExpr,
           Expression<Func<TItem, TKey>> textToDisplayExpr,
           Expression<Func<TItem, bool>> selectedValueExpr,
           HtmlListInfo wrapInfo,
           Expression<Func<TItem, object>> htmlAttributesExpr)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(listNameExpr, htmlHelper.ViewData);
            return _listBuilder.CheckBoxList
              (new listConstructor
                 <TModel, TItem, TValue, TKey>
              {
                  htmlHelper = htmlHelper,
                  modelMetadata = modelMetadata,
                  listName = listNameExpr.toProperty(),
                  sourceDataExpr = sourceDataExpr,
                  valueExpr = valueExpr,
                  textToDisplayExpr = textToDisplayExpr,
                  htmlAttributesExpr = htmlAttributesExpr,
                  selectedValueExpr = selectedValueExpr,
                  htmlListInfo = wrapInfo,
              });
        }

        // Level 3 - Full

        /// <summary>
        /// Generates Model-based list of checkboxes (For...)
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <typeparam name="TProperty">ViewModel property</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listNameExpr">ViewModel Item type to serve as a name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValuesExpr">Data list of selected items (should be of same data type as a source list)</param>
        /// <param name="htmlAttributes">Applies custom HTML tag attributes to each checkbox/label combo</param>
        /// <param name="wrapInfo">Settings for HTML wrapper of the list (e.g. 'new HtmlListInfo2(HtmlTag2.vertical_columns, 2, new { style="color:green;" })')</param>
        /// <param name="disabledValues">String array of values to disable</param>
        /// <param name="htmlAttributesExpr">Applies custom HTML tag attributes to each checkbox/label combo if defined locally (e.g.: entity => new { tagName = "tagValue" }), or to particular combos, if defined in the database (e.g.: entity => entity.TagsDbObject)</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty, TItem, TValue, TKey>
          (this HtmlHelper<TModel> htmlHelper,
           Expression<Func<TModel, TProperty>> listNameExpr,
           Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr,
           Expression<Func<TItem, TValue>> valueExpr,
           Expression<Func<TItem, TKey>> textToDisplayExpr,
           Expression<Func<TModel, IEnumerable<TItem>>> selectedValuesExpr,
           object htmlAttributes,
           HtmlListInfo wrapInfo,
           string[] disabledValues,
           Expression<Func<TItem, object>> htmlAttributesExpr)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(listNameExpr, htmlHelper.ViewData);
            return _listBuilder.CheckBoxList
              (new listConstructor
                 <TModel, TItem, TValue, TKey>
              {
                  htmlHelper = htmlHelper,
                  modelMetadata = modelMetadata,
                  listName = listNameExpr.toProperty(),
                  sourceDataExpr = sourceDataExpr,
                  valueExpr = valueExpr,
                  textToDisplayExpr = textToDisplayExpr,
                  htmlAttributesExpr = htmlAttributesExpr,
                  selectedValuesExpr = selectedValuesExpr,
                  htmlAttributes = htmlAttributes,
                  htmlListInfo = wrapInfo,
                  disabledValues = disabledValues
              });
        }
        /// <summary>
        /// Generates Model-based list of checkboxes (For...)
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <typeparam name="TValue">ViewModel Item type of the value</typeparam>
        /// <typeparam name="TKey">ViewModel Item type of the key</typeparam>
        /// <typeparam name="TProperty">ViewModel property</typeparam>
        /// <param name="htmlHelper">MVC Html helper class that is being extended</param>
        /// <param name="listNameExpr">ViewModel Item type to serve as a name of each checkbox in a list (use this name to POST list values array back to the controller)</param>
        /// <param name="sourceDataExpr">Data list to be used as a source for the list (set in viewmodel)</param>
        /// <param name="valueExpr">Data list value type to be used as checkbox 'Value'</param>
        /// <param name="textToDisplayExpr">Data list value type to be used as checkbox 'Text'</param>
        /// <param name="selectedValueExpr">Boolean value from db or selector corresponding to each item to be selected</param>
        /// <param name="htmlAttributes">Applies custom HTML tag attributes to each checkbox/label combo</param>
        /// <param name="wrapInfo">Settings for HTML wrapper of the list (e.g. 'new HtmlListInfo2(HtmlTag2.vertical_columns, 2, new { style="color:green;" })')</param>
        /// <param name="disabledValues">String array of values to disable</param>
        /// <param name="htmlAttributesExpr">Applies custom HTML tag attributes to each checkbox/label combo if defined locally (e.g.: entity => new { tagName = "tagValue" }), or to particular combos, if defined in the database (e.g.: entity => entity.TagsDbObject)</param>
        /// <returns>HTML string containing checkbox list</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty, TItem, TValue, TKey>
          (this HtmlHelper<TModel> htmlHelper,
           Expression<Func<TModel, TProperty>> listNameExpr,
           Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr,
           Expression<Func<TItem, TValue>> valueExpr,
           Expression<Func<TItem, TKey>> textToDisplayExpr,
           Expression<Func<TItem, bool>> selectedValueExpr,
           object htmlAttributes,
           HtmlListInfo wrapInfo,
           string[] disabledValues,
           Expression<Func<TItem, object>> htmlAttributesExpr)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(listNameExpr, htmlHelper.ViewData);
            return _listBuilder.CheckBoxList
              (new listConstructor
                 <TModel, TItem, TValue, TKey>
              {
                  htmlHelper = htmlHelper,
                  modelMetadata = modelMetadata,
                  listName = listNameExpr.toProperty(),
                  sourceDataExpr = sourceDataExpr,
                  valueExpr = valueExpr,
                  textToDisplayExpr = textToDisplayExpr,
                  htmlAttributesExpr = htmlAttributesExpr,
                  selectedValueExpr = selectedValueExpr,
                  htmlAttributes = htmlAttributes,
                  htmlListInfo = wrapInfo,
                  disabledValues = disabledValues
              });
        }

    }
}
