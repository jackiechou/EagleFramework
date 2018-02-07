using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc.Html;
using System.Web.UI;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;

namespace Eagle.Core.HtmlHelper.DataGrid
{
    /// <summary>
    /// Model => IPagedList<Product> => ToPagedList(page, 10, "Id", sort);
    /// @Html.DataGrid<Product>(ViewData[“products”]) 
    /// @Html.DataGrid<Product>(Model, new string[] {"Id", "Name"})
    /// @Html.DataGrid<Product>(Model, new string[] { "Id", "Name" }, new DataGridOption() )
    /// </summary>
    public static class DataGridHelper
    {
        public static string DataGrid<T>(this System.Web.Mvc.HtmlHelper helper)
        {
            return DataGrid<T>(helper, null, null);
        }
        public static string DataGrid<T>(this System.Web.Mvc.HtmlHelper helper, object data)
        {
            return DataGrid<T>(helper, data, null);
        }
        public static string DataGrid<T>(this System.Web.Mvc.HtmlHelper helper, object data, string[] columns)
        {
            // Get items
            var items = (IEnumerable<T>)data;
            if (items == null)
                items = (IEnumerable<T>)helper.ViewData.Model;


            // Get column names
            if (columns == null)
                columns = typeof(T).GetProperties().Select(p => p.Name).ToArray();

            // Create HtmlTextWriter
            var writer = new HtmlTextWriter(new StringWriter());

            // Open table tag
            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            // Render table header
            writer.RenderBeginTag(HtmlTextWriterTag.Thead);
            RenderHeader(helper, writer, columns);
            writer.RenderEndTag();

            // Render table body
            writer.RenderBeginTag(HtmlTextWriterTag.Tbody);
            foreach (var item in items)
                RenderRow<T>(helper, writer, columns, item);
            writer.RenderEndTag();

            // Close table tag
            writer.RenderEndTag();

            // Return the string
            return writer.InnerWriter.ToString();
        }

        //Add Paging
        public static string DataGrid<T>(this System.Web.Mvc.HtmlHelper helper, object data, string[] columns, DataGridOption options)
        {
            // Get items  
            var items = (IEnumerable<T>)data;
            if (items == null)
                items = (IEnumerable<T>)helper.ViewData.Model;



            // Get column names 
            if (columns == null)
                columns = typeof(T).GetProperties().Select(p => p.Name).ToArray();

            // Create HtmlTextWriter 
            var writer = new HtmlTextWriter(new StringWriter());

            // Open table tag   
            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            // Render table header   
            writer.RenderBeginTag(HtmlTextWriterTag.Thead);
            RenderHeader(helper, writer, columns, options);
            writer.RenderEndTag();


            string identityColumnName = ((PagedList<T>)items).SortColumnName;

            // Render table body 
            writer.RenderBeginTag(HtmlTextWriterTag.Tbody);
            foreach (var item in items)
                RenderRow<T>(helper, writer, columns, item, identityColumnName, options);

            writer.RenderEndTag();

            RenderPagerRow<T>(helper, writer, (PagedList<T>)items, columns.Count());


            // Close table tag   
            writer.RenderEndTag();

            // Return the string   
            return writer.InnerWriter.ToString();
        }
        private static void RenderHeader(System.Web.Mvc.HtmlHelper helper, HtmlTextWriter writer, string[] columns)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            foreach (var columnName in columns)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Th);
                writer.Write(helper.Encode(columnName));
                writer.RenderEndTag();
            }
            writer.RenderEndTag();
        }
        private static void RenderHeader(System.Web.Mvc.HtmlHelper helper, HtmlTextWriter writer, string[] columns, DataGridOption options)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            int i = 0;

            foreach (var columnName in columns)
            {

                writer.RenderBeginTag(HtmlTextWriterTag.Th);
                var currentAction = (string)helper.ViewContext.RouteData.Values["action"];


                string link = null;
                if (options.Columns == null)
                    link = helper.ActionLink(columnName, currentAction, new { sort = columnName }).ToHtmlString();
                else
                {
                    link = helper.ActionLink(options.Columns[i], currentAction, new { sort = columnName }).ToHtmlString();
                    i++;
                }

                writer.Write(link);

                writer.RenderEndTag();
            }

            // Show edit column?

            bool showEditColumn = options.ShowEditButton || options.ShowDeleteButton;

            if (showEditColumn)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Th);
                writer.Write(helper.Encode(""));
                writer.RenderEndTag();
            }

            writer.RenderEndTag();
        }
        private static void RenderRow<T>(System.Web.Mvc.HtmlHelper helper, HtmlTextWriter writer, string[] columns, T item)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            foreach (var columnName in columns)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                var value = typeof(T).GetProperty(columnName).GetValue(item, null) ?? String.Empty;
                writer.Write(helper.Encode(value.ToString()));
                writer.RenderEndTag();
            }
            writer.RenderEndTag();
        }
        private static void RenderRow<T>(System.Web.Mvc.HtmlHelper helper, HtmlTextWriter writer, string[] columns, T item, string identityColumnName, DataGridOption options)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            foreach (var columnName in columns)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                var value = typeof(T).GetProperty(columnName).GetValue(item, null) ?? String.Empty;
                writer.Write(helper.Encode(value.ToString()));
                writer.RenderEndTag();
            }


            // Show edit column?
            bool showEditColumn = options.ShowEditButton || options.ShowDeleteButton;
            if (showEditColumn)
            {
                var identityVaule = typeof(T).GetProperty(identityColumnName).GetValue(item, null);
                writer.RenderBeginTag(HtmlTextWriterTag.Td);

                if (options.ShowEditButton)
                {
                    var link = helper.ActionLink(options.EditButtonText, options.EditAction, new { id = identityVaule });
                    writer.Write(link);
                    writer.Write(" ");
                }



                if (options.ShowDeleteButton)
                {
                    var link = helper.ActionLink(options.DeleteButtonText, options.DeleteAction, new { id = identityVaule });
                    writer.Write(link);
                }

                writer.RenderEndTag();
            }
            writer.RenderEndTag();
        }
        private static void RenderPagerRow<T>(System.Web.Mvc.HtmlHelper helper, HtmlTextWriter writer, PagedList<T> items, int columnCount)
        {
            int nrOfPagesToDisplay = GlobalSettings.DefaultPageSize;

            // Don't show paging UI for only 1 page   
            if (items.TotalPageCount == 1)
                return;

            // Render page numbers   
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Colspan, columnCount.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            var currentAction = (string)helper.ViewContext.RouteData.Values["action"];

            if (items.PageIndex >= 1)
            {
                var linkText = String.Format("{0}", "<<<");
                var link = helper.ActionLink(linkText, currentAction, new { page = items.PageIndex, sort = items.SortExpression });
                writer.Write(link + "&nbsp;");
            }

            int start = 0;
            int end = items.TotalPageCount;
            if (items.TotalPageCount > nrOfPagesToDisplay)
            {
                int middle = (int)Math.Ceiling(nrOfPagesToDisplay / 2d) - 1;
                int below = (items.PageIndex - middle);
                int above = (items.PageIndex + middle);

                if (below < 4)
                {
                    above = nrOfPagesToDisplay;
                    below = 0;
                }
                else if (above > (items.TotalPageCount - 4))
                {
                    above = items.TotalPageCount;
                    below = (items.TotalPageCount - nrOfPagesToDisplay);
                }

                start = below;
                end = above;
            }


            if (start > 3)
            {
                var linkText = String.Format("{0}", "1");
                var link = helper.ActionLink(linkText, currentAction, new { page = 1, sort = items.SortExpression });
                writer.Write(link + "&nbsp;");


                linkText = String.Format("{0}", "2");
                link = helper.ActionLink(linkText, currentAction, new { page = 2, sort = items.SortExpression });
                writer.Write(link + "&nbsp;");

                writer.Write(String.Format("{0}", "..."));
            }


            for (var i = start; i < end; i++)
            {

                if (i == items.PageIndex)
                {
                    writer.Write(String.Format("<strong>{0}</strong>&nbsp;", i + 1));
                }
                else
                {
                    var linkText = String.Format("{0}", i + 1);
                    var link = helper.ActionLink(linkText, currentAction, new { page = i + 1, sort = items.SortExpression });
                    writer.Write(link + "&nbsp;");
                }
            }


            if (end < (items.TotalPageCount - 3))
            {
                writer.Write(String.Format("{0}", "..."));
                var linkText = String.Format("{0}", items.TotalPageCount - 1);
                var link = helper.ActionLink(linkText, currentAction, new { page = items.TotalPageCount - 1, sort = items.SortExpression });
                writer.Write(link + "&nbsp;");
                linkText = String.Format("{0}", items.TotalPageCount);
                link = helper.ActionLink(linkText, currentAction, new { page = items.TotalPageCount, sort = items.SortExpression });
                writer.Write(link + "&nbsp;");
            }

            if (items.PageIndex + 2 <= items.TotalPageCount)
            {
                var linkText = String.Format("{0}", ">>>");
                var link = helper.ActionLink(linkText, currentAction, new { page = items.PageIndex + 2, sort = items.SortExpression });
                writer.Write(link + "&nbsp;");
            }

            writer.RenderEndTag();
            writer.RenderEndTag();
        }
        //private static void RenderPagerRow<T>(HtmlHelper helper, HtmlTextWriter writer, PagedList<T> items, int columnCount)
        //{
        //    // Don't show paging UI for only 1 page
        //    if (items.TotalPageCount == 1)
        //        return;

        //    // Render page numbers
        //    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
        //    writer.AddAttribute(HtmlTextWriterAttribute.Colspan, columnCount.ToString());
        //    writer.RenderBeginTag(HtmlTextWriterTag.Td);
        //    var currentAction = (string)helper.ViewContext.RouteData.Values["action"];
        //    for (var i = 0; i < items.TotalPageCount; i++)
        //    {
        //        if (i == items.PageIndex)
        //        {
        //            writer.Write(String.Format("<strong>{0}</strong>&nbsp;", i + 1));
        //        }
        //        else
        //        {
        //            var linkText = String.Format("{0}", i + 1);
        //            var link = helper.ActionLink(linkText, currentAction, new { page = i, sort = items.SortExpression });
        //            writer.Write(link + "&nbsp;");
        //        }
        //    }
        //    writer.RenderEndTag();
        //    writer.RenderEndTag();
        //}
    }
}