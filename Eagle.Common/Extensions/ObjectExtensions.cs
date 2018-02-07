using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;

namespace Eagle.Common.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// A safe version of ToString() that does not break at null objects.
        /// </summary>
        /// <param name="object">object</param>
        /// <returns>object's ToString() if not null; otherwise, empty string</returns>
        public static string ToStringEx(this object @object)
        {
            return @object != null ? @object.ToString() : String.Empty;
        }

        /// <summary>
        /// Builds property/value dictionary from anonymously-typed object
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public static IDictionary<string, object> BuildDictionaryFromProperties(this object @object)
        {
            IDictionary<string, object> dictionary = new Dictionary<string, object>();

            if (@object != null)
            {
                foreach (var propertyDescriptor in TypeDescriptor.GetProperties(@object).Cast<PropertyDescriptor>())
                {
                    dictionary.Add(propertyDescriptor.Name, propertyDescriptor.GetValue(@object));
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Creates a clone (in terms of public accessors) object graph from the given source. Targets:
        /// 1. POCO objects (opt-out with [IgnoreDataMember])
        /// 2. [Serializable] objects (opt-out with [NonSerialized])
        /// 3. [DataContract] objects (opt-in with [DataMember])        
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="source">Source</param>
        /// <returns>Cloned object graph</returns>
        public static T Clone<T>(this T source)
        {
            T clone = Copy<T>(source, int.MaxValue);

            return SyncReferences(clone, source);
        }

        public static T CloneWithoutReference<T>(this T source)
        {
            T clone = Copy<T>(source, int.MaxValue);
            return clone;
        }

        /// <summary>
        /// Makes a copy of the given object graph
        /// </summary>
        /// <typeparam name="T">Host object's type</typeparam>
        /// <param name="source">Host object</param>
        /// <param name="maxItemsInObjectGraph">Self-describing. You don't understand, you go to Japan.</param>
        /// <returns>A copy of the given object graph</returns>
        private static T Copy<T>(T source, int maxItemsInObjectGraph)
        {
            using (var stream = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(
                    typeof(T),
                    null,
                    maxItemsInObjectGraph,
                    false,
                    true,
                    null);

                serializer.WriteObject(stream, source);

                stream.Seek(0, SeekOrigin.Begin);

                return (T)serializer.ReadObject(stream);
            }
        }

        private static T SyncReferences<T>(T copy, T source)
        {
            return (T)SyncReferences(copy, source, new List<object>());
        }

        /// <summary>
        /// Synchronizes uncopied references from the source object graph to the copied object graph
        /// </summary>
        /// <param name="copy">Self-describing</param>
        /// <param name="source">Self-describing</param>
        /// <param name="ignoredObjects">List of objects that should be ignored</param>
        /// <returns>The copy object (graph)</returns>
        private static object SyncReferences(object copy, object source, IList<object> ignoredObjects)
        {
            if (copy == null || source == null || ignoredObjects == null || ignoredObjects.Contains(copy))
            {
                return copy;
            }

            // mark
            ignoredObjects.Add(copy);

            foreach (var pd in TypeDescriptor.GetProperties(copy).Cast<PropertyDescriptor>()
                                                .Where(pd => !pd.IsReadOnly))
            {
                if (!pd.PropertyType.IsValueType)
                {
                    object copyValue = pd.GetValue(copy);
                    object sourceValue = pd.GetValue(source);

                    if (copyValue == null)
                    {
                        // if marked with IgnoreDataMember                        
                        if (pd.Attributes.OfType<IgnoreDataMemberAttribute>().FirstOrDefault() != null)
                        {
                            // update refererences
                            pd.SetValue(copy, sourceValue);
                        }
                    }
                    else if (typeof(IEnumerable).IsAssignableFrom(pd.PropertyType)) // traverse IEnumerables
                    {
                        List<object> copyList = ((IEnumerable)copyValue).Cast<object>().ToList();
                        List<object> sourceList = ((IEnumerable)sourceValue).Cast<object>().ToList();

                        // the order of the cloned list (technically, array) is not maintained. That's why...
                        foreach (var copyItem in copyList)
                        {
                            var sourceItem = sourceList.FirstOrDefault(item => item.Equals(copyItem));

                            SyncReferences(copyItem, sourceItem, ignoredObjects);
                        }
                    }
                    else // visit children
                    {
                        // do recursive update
                        SyncReferences(copyValue, sourceValue, ignoredObjects);
                    }
                }
            }

            return copy;
        }
    }
}
