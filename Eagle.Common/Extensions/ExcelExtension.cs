using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;

namespace Eagle.Common.Extensions
{
    public class ExcelExtension
    {
        /// <summary>

        /// Imports worksheet from an excel file (xls, xlsx or csv). Guesses data types and picks the first worksheet found.

        /// </summary>

        /// <param name="excelFile">The excel file path.</param>

        /// <param name="hasHeaderRow">if set to <c>true</c>, the first row is a header row.</param>

        public static DataTable ImportWorkSheet(string excelFile, bool hasHeaderRow)
        {

            return ImportWorkSheet(excelFile, hasHeaderRow, false, null);

        }



        /// <summary>

        /// Imports worksheet from an excel file (xls, xlsx or csv).

        /// </summary>

        /// <param name="excelFile">The excel file path.</param>

        /// <param name="hasHeaderRow">if set to <c>true</c>, the first row is a header row.</param>

        /// <param name="allText">Indicates all columns will be treated as text strings</param>

        /// <param name="tabName">Name of the sheet (or null if unknown - uses GetSchema to derive it)</param>

        public static DataTable ImportWorkSheet(string excelFile, bool hasHeaderRow, bool allText, string tabName)
        {
            string connectionString = GetExcelConnectionString(excelFile, hasHeaderRow, allText);

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                var extension = Path.GetExtension(excelFile);
                if (extension != null && extension.Equals(".csv", StringComparison.OrdinalIgnoreCase))
                    tabName = Path.GetFileName(excelFile);
                else
                    tabName = EnsureTableName(connection, tabName);

                string selectString = $"SELECT * FROM [{tabName}]";
                DataTable table = new DataTable(tabName);
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(selectString, connection))
                    adapter.Fill(table);
                return table;
            }
        }



        /// <summary>
        /// Imports all sheets from an Excel file
        /// </summary>
        /// <param name="excelFile">The excel file path.</param>
        /// <returns></returns>
        public static DataSet ImportAll(string excelFile)
        {
            DataSet ds = new DataSet();
            string connectionString = GetExcelConnectionString(excelFile, false, true);
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                //csv doesn't have worksheets, and table name == file name
                var extension = Path.GetExtension(excelFile);
                if (extension != null && extension.Equals(".csv", StringComparison.OrdinalIgnoreCase))
                {
                    LoadTableIntoDataSet(connection, Path.GetFileName(excelFile), ds);
                    return ds;
                }

                //xls and xlsx have worksheets, so load each one as a datatable
                DataTable worksheets = connection.GetSchema("Tables");

                foreach (DataRow row in worksheets.Rows)
                {
                    //this can also return Excel named ranges
                    string tabName = (string)row["TABLE_NAME"];

                    //so look for sheets (excel puts $ after the name and may single-quote the name)
                    if (tabName.EndsWith("$") || tabName.EndsWith("$'"))
                        LoadTableIntoDataSet(connection, tabName, ds);
                }
            }
            return ds;
        }
        
        private static void LoadTableIntoDataSet(OleDbConnection connection, string tabName, DataSet ds)
        {

            string selectString = string.Format("SELECT * FROM [{0}]", tabName);



            DataTable table = new DataTable(tabName);

            using (OleDbDataAdapter adapter = new OleDbDataAdapter(selectString, connection))

                adapter.Fill(table);

            ds.Tables.Add(table);

        }
        
        private static string EnsureTableName(DbConnection connection, string tabName)
        {

            if (string.IsNullOrEmpty(tabName))
            {

                //use GetSchema to find the first sheet == table name

                DataTable worksheets = connection.GetSchema("Tables");

                foreach (DataRow row in worksheets.Rows)
                {

                    //this can also return Excel named ranges

                    tabName = (string)row["TABLE_NAME"];

                    //so look for sheets (excel puts $ after the name and may single-quote the name)

                    if (tabName.EndsWith("$") || tabName.EndsWith("$'"))

                        return tabName;

                    //otherwise we'll fall through with whatever we find

                }

            }

            //they supplied a worksheet name; ensure always has $ suffix

            else if (!tabName.EndsWith("$"))

                tabName += "$";

            return tabName;

        }
        
        private static string GetExcelConnectionString(string filePath, bool hasHeaderRow, bool allText)
        {
            //http://www.connectionstrings.com/?carrier=excel
            string connectionString;
            string ext = Path.GetExtension(filePath);

            if (ext != null && ext.Equals(".csv", StringComparison.OrdinalIgnoreCase))
            {
                //uses directory path, not file path for format definition, write a schema.ini in the folder: http://msdn.microsoft.com/en-us/library/ms709353.aspx
                connectionString = string.Format(
                    @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}; Extended Properties=""text;FMT=Delimited;{1}{2}""",
                    filePath.Remove(filePath.IndexOf(Path.GetFileName(filePath), StringComparison.Ordinal)),
                    hasHeaderRow ? "HDR=YES;" : "HDR=NO;",
                    allText ? "IMEX=1" : string.Empty);
            }

            else if (ext != null && ext.Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                //if this fails, install 2007 providers from http://www.microsoft.com/downloads/details.aspx?FamilyID=7554F536-8C28-4598-9B72-EF94E038C891&displaylang=en
                connectionString = string.Format(
                    @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}; Extended Properties=""Excel 12.0 Xml;{1}{2}""",
                    filePath,
                    hasHeaderRow ? "HDR=YES;" : "HDR=NO;",
                    allText ? "IMEX=1" : string.Empty);
            }

            else //assume normal excel
            {
                connectionString = string.Format(
                    @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}; Extended Properties=""Excel 8.0;{1}{2}""",
                    filePath,
                    hasHeaderRow ? "HDR=YES;" : "HDR=NO;",
                    allText ? "IMEX=1" : string.Empty);
            }
            return connectionString;
        }

        public static List<string> GetSheetNames(string excelFilePath)
        {
            List<string> sheets = new List<string>();
            string sourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + excelFilePath + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=2'";
            DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");
            DbConnection connection = factory.CreateConnection();
            if (connection != null)
            {
                connection.ConnectionString = sourceConstr;
                connection.Open();
                DataTable tbl = connection.GetSchema("Tables");
                connection.Close();

                foreach (DataRow row in tbl.Rows)
                {
                    string sheetName = (string)row["TABLE_NAME"];
                    if (sheetName.EndsWith("$"))
                        sheetName = sheetName.Substring(0, sheetName.Length - 1);
                    sheets.Add(sheetName);
                }
            }
            return sheets;
        }

        public static int CreateTableFromExcelFile(string connectionString, string tableName, string columnList)
        {
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                string strCreateTable = "CREATE TABLE [dbo].[" + tableName + "](" + columnList + ")";

                SqlCommand command = new SqlCommand(strCreateTable, sqlcon);
                command.CommandType = CommandType.Text;
                sqlcon.Open();
                int result = command.ExecuteNonQuery();
                command.Dispose();
                sqlcon.Close();
                return result;
            }
        }

        public static bool DbTableExists(string connectionString, string strTableName)
        {
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                string strCheckTable =
                   String.Format(
                      "IF OBJECT_ID('{0}', 'U') IS NOT NULL SELECT 'true' ELSE SELECT 'false'",
                      strTableName);

                SqlCommand command = new SqlCommand(strCheckTable, sqlcon);
                command.CommandType = CommandType.Text;
                sqlcon.Open();
                return Convert.ToBoolean(command.ExecuteScalar());
            }
        }
        
    }
}
