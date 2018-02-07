using Eagle.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Eagle.Common.Data
{
    public class SchemaHelper
    {
        #region START SHCEMA ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public DataTable GetAllSchemas()
        {
            SQLHandler sqlHandler = new SQLHandler();
            string SQL = "SELECT distinct TABLE_SCHEMA FROM INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_SCHEMA ASC";
            DataTable dt = sqlHandler.ExecuteSQL(SQL);
            return dt;
        }

        public string GetSchemaJsonList()
        {
            DataTable dt = GetAllSchemas();
            string result = JsonUtils.SerializeToJSon(dt);
            return result;
        }
        #endregion END SCHEMA ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        #region START TABLE ---------------------------------------------------------------------------------------
        public DataTable GetAllTablesBySchema(string Selected_Schema_Name)
        {
            string SQL = "SELECT TABLE_SCHEMA,TABLE_NAME FROM INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'BASE TABLE' AND TABLE_SCHEMA ='" + Selected_Schema_Name + "' ORDER BY TABLE_SCHEMA ASC";
            SQLHandler sqlHandler = new SQLHandler();
            DataTable dt = sqlHandler.ExecuteSQL(SQL);
            return dt;
        }
        public string GetTableJsonListBySchema(string Selected_Schema_Name)
        {
            DataTable dt = GetAllTablesBySchema(Selected_Schema_Name);
            string jsonString = JsonUtils.SerializeToJSon(dt);
            return jsonString;
        }

        #endregion END TABLE --------------------------------------------------------------------------------------

        #region START COLUMS FROM JSON FILE ------------------------------------------------------------------------
        private DataTable GetColumInfoByTableName(string Selected_TableName)
        {
            string SQL = "SELECT COLUMN_NAME, DATA_TYPE FROM information_schema.columns where TABLE_SCHEMA +'.' +TABLE_NAME ='" + Selected_TableName + "'";
            SQLHandler sqlHandler = new SQLHandler();
            DataTable dt = sqlHandler.ExecuteSQL(SQL);
            return dt;
        }

        //public string GetColumnList(string table_schema, string table_name, string physical_json_file_path)
        //{
        //    string result = string.Empty, s = string.Empty;
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    using (StreamReader r = new StreamReader(physical_json_file_path))
        //    {
        //        string jsonString = r.ReadToEnd();
        //        //Install-Package Newtonsoft.Json =>JsonConvert
        //        List<RootObject> items = JsonConvert.DeserializeObject<List<RootObject>>(jsonString);
        //        if (items.Count > 0)
        //        {
        //            foreach (var item in items)
        //            {
        //                if (item.name == table_name)
        //                {
        //                    s += serializer.Serialize(item.columns) + ",";
        //                }
        //            }
        //            result = s.Remove(s.LastIndexOf(","), 1);
        //        }
        //    }
        //    return result;
        //}
        #endregion END SCHEMA
    }
}
