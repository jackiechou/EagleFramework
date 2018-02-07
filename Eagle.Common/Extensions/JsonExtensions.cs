using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Data;
//using Newtonsoft.Json;
using System.Collections;
using Newtonsoft.Json;

namespace Eagle.Common.Extensions
{
    /// <summary>
    /// Json Extensions based on the JavaScript Serializer in System.web
    /// </summary>
    public static class JsonExtensions
    {

        /// <summary> 
        /// Serializes a type to Json. Note the type must be marked Serializable 
        /// or include a DataContract attribute. 
        /// </summary> 
        /// <param name="value"></param> 
        /// <returns></returns>
        public static string ToJsonString(object value)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = ser.Serialize(value);
            return json;
        }

        /// <summary> 
        /// Extension method on object that serializes the value to Json. 
        /// Note the type must be marked Serializable or include a DataContract attribute. 
        /// </summary> 
        /// <param name="value"></param> 
        /// <returns></returns> 
        public static string ToJson(this object value)
        {
            return ToJsonString(value);
        }

        /// <summary> 
        /// Deserializes a json string into a specific type. 
        /// Note that the type specified must be serializable. 
        /// </summary> 
        /// <param name="json"></param> 
        /// <param name="type"></param> 
        /// <returns></returns> 
        public static object FromJsonString(string json, Type type)
        {
            // *** Have to use Reflection with a 'dynamic' non constant type instance 
            JavaScriptSerializer ser = new JavaScriptSerializer();

            object result = ser.GetType().GetMethod("Deserialize").MakeGenericMethod(type).Invoke(ser, new object[1] { json });
            return result;
        }

        /// <summary> 
        /// Extension method to string that deserializes a json string 
        /// into a specific type. 
        /// Note that the type specified must be serializable. 
        /// </summary> 
        /// <param name="json"></param> 
        /// <param name="type"></param> 
        /// <returns></returns> 
        public static object FromJson(this string json, Type type)
        {
            return FromJsonString(json, type);
        }

        public static TType FromJson<TType>(this string json)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();

            TType result = ser.Deserialize<TType>(json);
            return result;
        }

        public static object DeserializerJsonString(string json)
        {
            JavaScriptSerializer deserializer = new JavaScriptSerializer();
            Dictionary<string, object> deserializedDictionary1 = (Dictionary<string, object>)deserializer.Deserialize(json, typeof(object));
            Dictionary<string, object> deserializedDictionary2 = deserializer.Deserialize<Dictionary<string, object>>(json);
            object objDeserialized = deserializer.DeserializeObject(json);
            return objDeserialized;
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


        public static string DatatableToJson(DataTable dt)
        {
            List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, string> row = new Dictionary<string, string>();
                foreach (DataColumn dc in dt.Columns)
                {
                    row.Add(Convert.ToString(dc.ColumnName), Convert.ToString(dr[dc]));
                }
                rows.Add(row);
            }
            string serializedResult = serializer.Serialize(rows);
            return serializedResult;
        }

        public static string DisplayDictionary(Dictionary<string, object> dict)
        {
            int indentLevel = 0;
            string strOutput = "";
            foreach (string strKey in dict.Keys)
            {
                strOutput = "".PadLeft(indentLevel * 8) + strKey + ":";

                object o = dict[strKey];
                if (o is Dictionary<string, object>)
                {
                    DisplayDictionary((Dictionary<string, object>)o);
                }
                else if (o is ArrayList)
                {
                    foreach (object oChild in ((ArrayList)o))
                    {
                        if (oChild is string)
                        {
                            strOutput = ((string)oChild) + ",";
                        }
                        else if (oChild is Dictionary<string, object>)
                        {
                            DisplayDictionary((Dictionary<string, object>)oChild);
                        }
                    }
                }
                else
                {
                    strOutput = o.ToString();
                }
            }
            return strOutput;
        }
        public static Dictionary<string, object> DeserializeToDictionary(string json)
        {
            Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            Dictionary<string, object> values2 = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> d in values)
            {
                values2.Add(d.Key,
                    d.Value.GetType().FullName.Contains("Newtonsoft.Json.Linq.JObject")
                        ? DeserializeToDictionary(d.Value.ToString())
                        : d.Value);
            }
            return values2;
        }
        public static Dictionary<string, object> ParseJson(string json)
        {
            int end;
            return ParseJson(json, 0, out end);
        }
        private static Dictionary<string, object> ParseJson(string json, int start, out int end)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            bool escbegin = false;
            bool escend = false;
            bool inquotes = false;
            string key = null;
            int cend;
            StringBuilder sb = new StringBuilder();
            Dictionary<string, object> child = null;
            List<object> arraylist = null;
            Regex regex = new Regex(@"\\u([0-9a-z]{4})", RegexOptions.IgnoreCase);
            int autoKey = 0;
            for (int i = start; i < json.Length; i++)
            {
                char c = json[i];
                if (c == '\\') escbegin = !escbegin;
                if (!escbegin)
                {
                    if (c == '"')
                    {
                        inquotes = !inquotes;
                        if (!inquotes && arraylist != null)
                        {
                            arraylist.Add(DecodeString(regex, sb.ToString()));
                            sb.Length = 0;
                        }
                        continue;
                    }
                    if (!inquotes)
                    {
                        switch (c)
                        {
                            case '{':
                                if (i != start)
                                {
                                    child = ParseJson(json, i, out cend);
                                    if (arraylist != null) arraylist.Add(child);
                                    else
                                    {
                                        dict.Add(key, child);
                                        key = null;
                                    }
                                    i = cend;
                                }
                                continue;
                            case '}':
                                end = i;
                                if (key != null)
                                {
                                    if (arraylist != null) dict.Add(key, arraylist);
                                    else dict.Add(key, DecodeString(regex, sb.ToString()));
                                }
                                return dict;
                            case '[':
                                arraylist = new List<object>();
                                continue;
                            case ']':
                                if (key == null)
                                {
                                    key = "array" + autoKey.ToString();
                                    autoKey++;
                                }
                                if (arraylist != null && sb.Length > 0)
                                {
                                    arraylist.Add(sb.ToString());
                                    sb.Length = 0;
                                }
                                dict.Add(key, arraylist);
                                arraylist = null;
                                key = null;
                                continue;
                            case ',':
                                if (arraylist == null && key != null)
                                {
                                    dict.Add(key, DecodeString(regex, sb.ToString()));
                                    key = null;
                                    sb.Length = 0;
                                }
                                if (arraylist != null && sb.Length > 0)
                                {
                                    arraylist.Add(sb.ToString());
                                    sb.Length = 0;
                                }
                                continue;
                            case ':':
                                key = DecodeString(regex, sb.ToString());
                                sb.Length = 0;
                                continue;
                        }
                    }
                }
                sb.Append(c);
                if (escend) escbegin = false;
                if (escbegin) escend = true;
                else escend = false;
            }
            end = json.Length - 1;
            return dict; //theoretically shouldn't ever get here
        }
        private static string DecodeString(Regex regex, string str)
        {
            return Regex.Unescape(regex.Replace(str, match => char.ConvertFromUtf32(Int32.Parse(match.Groups[1].Value, System.Globalization.NumberStyles.HexNumber))));
        }
    }
}
