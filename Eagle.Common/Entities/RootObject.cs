using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;


namespace Eagle.Common.Entities
{
    public class RootObject
    {
        public string name { get; set; }
        public object index { get; set; }
        public string description { get; set; }
        public bool join_table { get; set; }
        public List<Column> columns { get; set; }

        //NewtonJson
        //public string ParseJsonFileToText(string physical_json_file_path)
        //{
        //    using (StreamReader r = new StreamReader(physical_json_file_path))
        //    {
        //        string str = string.Empty;
        //        string json = r.ReadToEnd();
        //        List<RootObject> items = JsonConvert.DeserializeObject<List<RootObject>>(json);
        //        Dictionary<string, object> dict = new Dictionary<string, object>();
        //        JavaScriptSerializer serializer = new JavaScriptSerializer();

        //        foreach (var item in items)
        //        {
        //            dict.Add(item.name, item);
        //            str += string.Join(",", serializer.Serialize(dict));
        //        }
        //        return str;
        //    }
        //}
    }
}
