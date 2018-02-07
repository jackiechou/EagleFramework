using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Web.Script.Serialization;

namespace Eagle.Common.Utilities
{
    public class ExpandoConverter : JavaScriptConverter
    {
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        { return DictionaryToExpando(dictionary); }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        { return ((ExpandoObject)obj).ToDictionary(x => x.Key, x => x.Value); }

        public override IEnumerable<Type> SupportedTypes => new ReadOnlyCollection<Type>(new Type[] { typeof(System.Dynamic.ExpandoObject) });

        private ExpandoObject DictionaryToExpando(IDictionary<string, object> source)
        {
            var expandoObject = new ExpandoObject();
            var expandoDictionary = (IDictionary<string, object>)expandoObject;
            foreach (var kvp in source)
            {
                var objects = kvp.Value as IDictionary<string, object>;
                if (objects != null) expandoDictionary.Add(kvp.Key, DictionaryToExpando(objects));
                else if (kvp.Value is ICollection)
                {
                    var valueList = (from object value in (ICollection) kvp.Value let dictionary = value as IDictionary<string, object> select dictionary != null ? DictionaryToExpando(dictionary) : value).ToList();
                    expandoDictionary.Add(kvp.Key, valueList);
                }
                else expandoDictionary.Add(kvp.Key, kvp.Value);
            }
            return expandoObject;
        }
    }
}
