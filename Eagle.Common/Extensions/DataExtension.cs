#region "References"
using System;
using System.Collections.Generic;
using System.Collections;
using System.Resources;
using System.Web.UI.WebControls;
using System.Web;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Xml.Xsl;
using System.Xml;
using System.IO;
using System.Web.Security;
#endregion

namespace Eagle.Common.Extensions
{
    public class DataExtension
    {
        public void ExportDataTableToXml(DataTable formattedDataTable, string filename)
        {
            formattedDataTable.TableName = "LoginLog";
            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            string result;
            using (StringWriter sw = new StringWriter())
            {
                formattedDataTable.WriteXml(sw);
                result = sw.ToString();
            }
            context.Response.Write(result);
            context.Response.ContentType = "application/octet-stream";
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + ".xml");
            context.Response.End();
        }

        public void ExportDataTableToCsv(DataTable formattedDataTable, string filename)
        {
            DataTable toExcel = formattedDataTable.Copy();
            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            foreach (DataColumn column in toExcel.Columns)
            {
                context.Response.Write(column.ColumnName + ",");
            }
            context.Response.Write(Environment.NewLine);
            foreach (DataRow row in toExcel.Rows)
            {
                for (int i = 0; i < toExcel.Columns.Count; i++)
                {
                    context.Response.Write(row[i].ToString().Replace(",", string.Empty) + ",");
                }
                context.Response.Write(Environment.NewLine);
            }
            context.Response.ContentType = "text/csv";
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + ".csv");
            context.Response.End();
        }

        public void ExportDataTableToXls(DataTable formattedDataTable, string filename)
        {
            DataTable toExcel = formattedDataTable.Copy();
            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            foreach (DataColumn column in toExcel.Columns)
            {
                context.Response.Write(column.ColumnName + "\t");//+ ","
            }
            context.Response.Write(Environment.NewLine);
            foreach (DataRow row in toExcel.Rows)
            {
                for (int i = 0; i < toExcel.Columns.Count; i++)
                {
                    //context.Response.Write(row[i].ToString().Replace(",", string.Empty) + ",");
                    context.Response.Write(row[i].ToString().Replace(",", string.Empty) + "\t");//
                }
                context.Response.Write(Environment.NewLine);
            }
            context.Response.ContentType = "application/xls";
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + ".xls");
            context.Response.End();
        }

        public void ExportDataTableToXlsx(DataTable formattedDataTable, string filename)
        {
            DataTable toExcel = formattedDataTable.Copy();
            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            foreach (DataColumn column in toExcel.Columns)
            {
                context.Response.Write(column.ColumnName + "\t");//+ ","
            }
            context.Response.Write(Environment.NewLine);
            foreach (DataRow row in toExcel.Rows)
            {
                for (int i = 0; i < toExcel.Columns.Count; i++)
                {
                    context.Response.Write(row[i].ToString().Replace(",", string.Empty) + "\t");//
                }

                context.Response.Write(Environment.NewLine);
            }
            context.Response.ContentType = "application/vnd.ms-excel";//application/xls //application/vnd.ms-excel
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + ".xlsx");
            context.Response.End();
        }

        public object[] DataTableToObject(DataTable dtReturn)
        {
            object[] obj = new object[dtReturn.Rows.Count];
            for (int i = 0; i < dtReturn.Rows.Count; i++)
            {
                object[] objItem = new object[dtReturn.Columns.Count];
                for (int j = 0; j < dtReturn.Columns.Count; j++)
                {
                    objItem[j] = (object)dtReturn.Rows[i][j];
                }
                obj[i] = objItem;
            }
            return obj;
        }

        //Adedd By alok
        public DataTable LinqToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();
            // column names 
            PropertyInfo[] oProps = null;
            if (varlist == null) return dtReturn;
            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others 
                //will follow 
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }
                DataRow dr = dtReturn.NewRow();
                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
                }
                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }
    }
}
