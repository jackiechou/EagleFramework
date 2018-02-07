using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Eagle.Core.Common
{
   public static class TypeSerializerExtensions
    {
        /// <summary>
        /// Returns the target namespace for the serializer.
        /// </summary>
        public static string TargetNamespace
        {
            get
            {
                return "http://www.w3.org/2001/XMLSchema";
            }
        }
        /// <summary>
        /// Returns the set of included namespaces for the serializer.
        /// </summary>
        /// <returns>
        /// The set of included namespaces for the serializer.
        /// </returns>
        public static XmlSerializerNamespaces GetNamespaces()
        {
            XmlSerializerNamespaces ns;
            ns = new XmlSerializerNamespaces();
            ns.Add("xs", "http://www.w3.org/2001/XMLSchema");
            ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            return ns;
        }
        /// <summary>
        /// Serializes the <i>Obj</i> to an XML string.
        /// </summary>
        /// <param name="Obj">
        /// The object to serialize.</param>
        /// <param name="ObjType">The object type.</param>
        /// <returns>
        /// The serialized object XML string.
        /// </returns>
        /// <remarks>
        /// The <see cref="PrettyPrint" /> property provides
        /// an easy-to-read formatted XML string.
        /// </remarks>
        public static string ToXml<T>(this T obj)
        {
            var ser = new XmlSerializer(typeof(T), TypeSerializerExtensions.TargetNamespace);
            var memStream = new MemoryStream();
            var xmlWriter = new XmlTextWriter(memStream, Encoding.UTF8);
            if (TypeSerializerExtensions.PrettyPrint)
            {
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.Indentation = 1;
                xmlWriter.IndentChar = Convert.ToChar(9);
            }
            xmlWriter.Namespaces = true;
            ser.Serialize(xmlWriter, obj, TypeSerializerExtensions.GetNamespaces());
            xmlWriter.Close();
            memStream.Close();
            string xml;
            xml = Encoding.UTF8.GetString(memStream.GetBuffer());
            xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
            xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));
            return xml;
        }
        /// <summary>
        /// Creates an object from an XML string.
        /// </summary>
        /// <param name="Xml">
        /// XML string to serialize.</param>
        /// <param name="ObjType">The object type to create.</param>
        /// <returns>
        /// An object of type <i>ObjType</i>.
        /// </returns>
        public static object XmlToObject<T>(this T type , object Xml, System.Type ObjType)
        {
            var ser = new XmlSerializer(ObjType);
            var stringReader = new StringReader(Xml.ToString());
            var  xmlReader = new XmlTextReader(stringReader);
            object obj;
            obj = ser.Deserialize(xmlReader);
            xmlReader.Close();
            stringReader.Close();
            return obj;
        }
        /// <summary>
        /// The member for the <see cref="PrettyPrint" />
        /// property.
        /// </summary>
        private static bool @__PrettyPrint;
        /// <summary>
        /// Get or sets the pretty print property.
        /// </summary>
        /// <remarks>
        /// If <b>true</b>, the XML output by the
        /// <see cref="ToXml" /> method is indented in
        /// a hierarchichal manner using one tab character
        /// per level, each level being on a new line. 
        /// If <b>false</b>, no indentation or newline
        /// characters are set in the output.
        /// </remarks>
        public static bool PrettyPrint
        {
            get
            {
                return TypeSerializerExtensions.@__PrettyPrint;
            }
            set
            {
                TypeSerializerExtensions.@__PrettyPrint = value;
            }
        }
    }
}
