using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Eagle.Common.Utilities
{
    public enum makejson
    {
        e_without_square_brackets,
        e_with_square_brackets
    }

    public static class JsonUtils
    {
        #region Method 1 ==============================================================================
        /// <summary>
        /// requested this through an ajax call, you can call a => [{name:{name1:"ab",name2:"cd"},id:9}]
        /// var jsondatastructure = eval (yourResponseText);
        /// </summary>
        public static string ConvertDataTableToJson(DataTable table, makejson e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in table.Rows)
            {
                if (sb.Length != 0)
                    sb.Append(",");
                sb.Append("{");
                StringBuilder sb2 = new StringBuilder();
                foreach (DataColumn col in table.Columns)
                {
                    string fieldname = col.ColumnName;
                    string fieldvalue = dr[fieldname].ToString();
                    if (sb2.Length != 0)
                        sb2.Append(",");
                    sb2.Append(string.Format("\"{0}\":\"{1}\"", fieldname, fieldvalue));


                }
                sb.Append(sb2.ToString());
                sb.Append("}");


            }
            if (e == makejson.e_with_square_brackets)
            {
                sb.Insert(0, "[");
                sb.Append("]");
            }
            return sb.ToString();
        }

        public static string ConvertDataTableToJsonWithouKeys(DataTable table, makejson e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in table.Rows)
            {
                if (sb.Length != 0)
                    sb.Append(",");
                sb.Append("{");
                StringBuilder sb2 = new StringBuilder();
                foreach (DataColumn col in table.Columns)
                {
                    string fieldname = col.ColumnName;
                    string fieldvalue = dr[fieldname].ToString();
                    if (sb2.Length != 0)
                        sb2.Append(",");
                    sb2.Append(string.Format("\"{0}\":\"{1}\"", fieldname, fieldvalue));


                }
                sb.Append(sb2.ToString());
                sb.Append("}");


            }
            if (e == makejson.e_with_square_brackets)
            {
                sb.Insert(0, "[");
                sb.Append("]");
            }
            return sb.ToString();
        }

        private static List<Dictionary<string, object>> RowsToDictionary(DataTable table)
        {
            List<Dictionary<string, object>> objs = new List<Dictionary<string, object>>();
            foreach (DataRow dr in table.Rows)
            {
                Dictionary<string, object> drow = new Dictionary<string, object>();
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    drow.Add(table.Columns.ToString(), dr);
                }
                objs.Add(drow);
            }
            return objs;
        }

        public static Dictionary<string, object> ToJson(DataTable table)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d.Add(table.TableName, RowsToDictionary(table)); return d;
        }

        public static Dictionary<string, object> ToJson(DataSet data)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            foreach (DataTable table in data.Tables)
            {
                d.Add(table.TableName, RowsToDictionary(table));
            }
            return d;
        }

        public static string ToJson(Dictionary<string, object> dict)
        {
            //Dictionary<string, object> dict = new Dictionary<string, object>();
            //dict.Add("id", 1);
            //dict.Add("title", "The title");

            var serializer = new JavaScriptSerializer();
            return serializer.Serialize((object)dict);
        }

        public static string ToJson(Dictionary<string, object> dict, int recursionDepth)
        {
            var serializer = new JavaScriptSerializer();
            serializer.RecursionLimit = recursionDepth;
            return serializer.Serialize((object)dict);
        }
        #endregion =====================================================================================

        public static string SerializeResult(bool flag, string message)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            dict.Add("flag", flag.ToString().ToLower());
            dict.Add("message", message);
            var s = serializer.Serialize(dict) + ",";
            return s.Remove(s.LastIndexOf(",", StringComparison.Ordinal), 1);
        }

        public static string SerializeToJSon(DataTable dt)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Dictionary<string, object>> dataRows = new List<Dictionary<string, object>>();
            dt.Rows.Cast<DataRow>().ToList().ForEach(dataRow =>
            {
                var row = new Dictionary<string, object>();
                dt.Columns.Cast<DataColumn>().ToList().ForEach(column =>
                {
                    row.Add(column.ColumnName, dataRow[column]);
                });
                dataRows.Add(row);
            });
            return ser.Serialize(dataRows);
        }

        public static string ConvertDataTable2JsonString(DataTable dt)
        {
            int index = 0;
            Dictionary<string, object> resultMain = new Dictionary<string, object>();

            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    dict.Add(dc.ColumnName, dr[dc].ToString());
                }
                resultMain.Add(index.ToString(), dict);
                index++;
            }

            JavaScriptSerializer objSer = new JavaScriptSerializer();
            string result = objSer.Serialize(resultMain);
            return result;
        }

        public static string FromDataTable(DataTable dt)
        {
            string rowDelimiter = "";

            StringBuilder result = new StringBuilder("[");
            foreach (DataRow row in dt.Rows)
            {
                result.Append(rowDelimiter);
                result.Append(FromDataRow(row));
                rowDelimiter = ",";
            }
            result.Append("]");

            return result.ToString();
        }

        public static string FromDataRow(DataRow row)
        {
            DataColumnCollection cols = row.Table.Columns;
            string colDelimiter = "";

            StringBuilder result = new StringBuilder("{");
            for (int i = 0; i < cols.Count; i++)
            { // use index rather than foreach, so we can use the index for both the row and cols collection
                result.Append(colDelimiter).Append("\"")
                      .Append(cols[i].ColumnName).Append("\":")
                      .Append(JSONValueFromDataRowObject(row[i], cols[i].DataType));

                colDelimiter = ",";
            }
            result.Append("}");
            return result.ToString();
        }

        // possible types:
        // http://msdn.microsoft.com/en-us/library/system.data.datacolumn.datatype(VS.80).aspx
        private static Type[] numeric = new Type[] {typeof(byte), typeof(decimal), typeof(double), 
                                     typeof(Int16), typeof(Int32), typeof(SByte), typeof(Single),
                                     typeof(UInt16), typeof(UInt32), typeof(UInt64)};

        // I don't want to rebuild this value for every date cell in the table
        private static long EpochTicks = new DateTime(1970, 1, 1).Ticks;

        private static string JSONValueFromDataRowObject(object value, Type DataType)
        {
            // null
            if (value == DBNull.Value) return "null";

            // numeric
            if (Array.IndexOf(numeric, DataType) > -1)
                return value.ToString(); // TODO: eventually want to use a stricter format. Specifically: separate integral types from floating types and use the "R" (round-trip) format specifier

            // boolean
            if (DataType == typeof(bool))
                return ((bool)value) ? "true" : "false";

            // date -- see http://weblogs.asp.net/bleroy/archive/2008/01/18/dates-and-json.aspx
            if (DataType == typeof(DateTime))
                return "\"\\/Date(" + new TimeSpan(((DateTime)value).ToUniversalTime().Ticks - EpochTicks).TotalMilliseconds.ToString() + ")\\/\"";

            // TODO: add Timespan support
            // TODO: add Byte[] support

            //TODO: this would be _much_ faster with a state machine
            //TODO: way to select between double or single quote literal encoding
            //TODO: account for database strings that may have single \r or \n line breaks
            // string/char  
            return "\"" + value.ToString().Replace(@"\", @"\\").Replace(Environment.NewLine, @"\n").Replace("\"", @"\""") + "\"";
        }
    }
}
