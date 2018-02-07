using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using Eagle.Common.Extensions.EnumHelper;

namespace Eagle.Common.Extensions.DataGrid
{
    public enum OPERATION
    {
        none,
        add,
        del,
        edit,
        excel
    }

    /// <summary>
    /// The supported operations in where-extension
    /// </summary>
    public enum WhereOperation
    {
        [StringValue("eq")]
        Equal,
        [StringValue("ne")]
        NotEqual,
        [StringValue("cn")]
        Contains
    }

    [ModelBinder(typeof(GridModelBinder))]
    public class GridSettings
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public string sortColumn { get; set; }
        public string sortOrder { get; set; }
        public bool isSearch { get; set; }
        public string id { get; set; }
        public string param { get; set; }
        public string editOper { get; set; }
        public string addOper { get; set; }
        public string delOper { get; set; }
        public Filter where { get; set; }
        public OPERATION operation { get; set; }
    }

    public class GridModelBinder : IModelBinder
    {

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            return new GridSettings()
            {
                isSearch = bool.Parse(request["_search"] ?? "false"),
                pageIndex = int.Parse(request["page"] ?? "1"),
                pageSize = int.Parse(request["rows"] ?? "10"),
                sortColumn = request["sidx"] ?? "",
                sortOrder = request["sord"] ?? "asc",
                id = request["id"] ?? "",
                param = request["oper"] ?? "",
                editOper = request["edit"] ?? "",
                addOper = request["add"] ?? "",
                delOper = request["del"] ?? "",
                where = Filter.Create(request["filters"] ?? ""),
                operation = (OPERATION)System.Enum.Parse(typeof(OPERATION), request["oper"] ?? "none"),
            };
        }

    }

    [DataContract]
    public class Filter
    {
        [DataMember]
        public string groupOp { get; set; }
        [DataMember]
        public Rule[] rules { get; set; }

        public static Filter Create(string jsonData)
        {
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(Filter));
                System.IO.StringReader reader = new System.IO.StringReader(jsonData);
                System.IO.MemoryStream ms = new System.IO.MemoryStream(Encoding.Unicode.GetBytes(jsonData.Replace("\t","")));
                return serializer.ReadObject(ms) as Filter;
            }
            catch
            {
                return null;
            }
        }
    }

    [DataContract]
    public class Rule
    {
        //private string _myField;
        string _data;

        [DataMember]
        public string field { get; set; }
        [DataMember]
        public string op { get; set; }
        [DataMember]
        public string data { 
            get {
                return _data.Replace("&", "&amp;")
                            .Replace("\"", "&quot;")
                            .Replace("'", "\'")
                            .Replace("<", "&lt;")
                            .Replace(">", "&gt;");
            } 
            set { 
                    _data = value; 
            } 
        }
        //public string data{
        //    get {
        //       return _myField.Trim();
        //     }
        //     set {
        //         _myField = value;
        //     }
        //}
    }
}