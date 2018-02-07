#region "References"
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
#endregion

namespace Eagle.Common.Utilities
{
    public class XmlUtils
    {
         #region Methods
        /// <summary>
        /// XML Encode
        /// </summary>
        /// <param name="s">String</param>
        /// <returns>Encoded string</returns>
        public static string XmlEncode(string s)
        {
            if (s == null)
                return null;
            s = Regex.Replace(s, @"[^\u0009\u000A\u000D\u0020-\uD7FF\uE000-\uFFFD]", "", RegexOptions.Compiled);
            return XmlEncodeAsIs(s);
        }

        /// <summary>
        /// XML Encode as is
        /// </summary>
        /// <param name="s">String</param>
        /// <returns>Encoded string</returns>
        public static string XmlEncodeAsIs(string s)
        {
            if (s == null)
                return null;
            using (StringWriter sw = new StringWriter())
            using (XmlTextWriter xwr = new XmlTextWriter(sw))
            {
                xwr.WriteString(s);
                String sTmp = sw.ToString();
                return sTmp;
            }
        }

        /// <summary>
        /// Encodes an attribute
        /// </summary>
        /// <param name="s">Attribute</param>
        /// <returns>Encoded attribute</returns>
        public static string XmlEncodeAttribute(string s)
        {
            if (s == null)
                return null;
            s = Regex.Replace(s, @"[^\u0009\u000A\u000D\u0020-\uD7FF\uE000-\uFFFD]", "", RegexOptions.Compiled);
            return XmlEncodeAttributeAsIs(s);
        }

        /// <summary>
        /// Encodes an attribute as is
        /// </summary>
        /// <param name="s">Attribute</param>
        /// <returns>Encoded attribute</returns>
        public static string XmlEncodeAttributeAsIs(string s)
        {
            return XmlEncodeAsIs(s).Replace("\"", "&quot;");
        }

        /// <summary>
        /// Decodes an attribute
        /// </summary>
        /// <param name="s">Attribute</param>
        /// <returns>Decoded attribute</returns>
        public static string XmlDecode(string s)
        {
            StringBuilder sb = new StringBuilder(s);
            return sb.Replace("&quot;", "\"").Replace("&apos;", "'").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&").ToString();
        }

        /// <summary>
        /// Serializes a datetime
        /// </summary>
        /// <param name="dateTime">Datetime</param>
        /// <returns>Serialized datetime</returns>
        public static string SerializeDateTime(DateTime dateTime)
        {
            XmlSerializer xmlS = new XmlSerializer(typeof(DateTime));
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                xmlS.Serialize(sw, dateTime);
                return sb.ToString();
            }
        }

        /// <summary>
        /// Deserializes a datetime
        /// </summary>
        /// <param name="dateTime">Datetime</param>
        /// <returns>Deserialized datetime</returns>
        public static DateTime DeserializeDateTime(string dateTime)
        {
            XmlSerializer xmlS = new XmlSerializer(typeof(DateTime));
            StringBuilder sb = new StringBuilder();
            using (StringReader sr = new StringReader(dateTime))
            {
                object test = xmlS.Deserialize(sr);
                return (DateTime)test;
            }
        }

        //"~/Areas/Administration/Reports/ReportSettings.xml"
        public static DataTable GetDataFromXmlFile(string virtualFilePath)
        {
            DataSet dsResult = new DataSet();
            if (!string.IsNullOrEmpty(virtualFilePath))
            {
                string physicalFilePath = HttpContext.Current.Server.MapPath(virtualFilePath);
                if (File.Exists(physicalFilePath))
                    dsResult.ReadXml(physicalFilePath);                
            }
            return dsResult.Tables[0];
        }

        public static void DeleteNodes(string virtualFilePath, string nodeName)
        {
            if (!string.IsNullOrEmpty(virtualFilePath))
            {
                string physicalFilePath = HttpContext.Current.Server.MapPath(virtualFilePath);
                if (File.Exists(physicalFilePath))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(physicalFilePath);
                    XmlNode node = doc.SelectSingleNode("resourcemaps");
                    node.RemoveAll();
                    doc.Save(physicalFilePath);
                }
            }
        }


        public XmlDocument CreateXSD(string physicalXmlFileNPath, string physicalXsdFilePath)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(physicalXmlFileNPath);

            XmlSchemaInference inference = new XmlSchemaInference();
            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(xml.InnerXml));
            XmlTextReader reader = new XmlTextReader(stream);

            XmlSchemaSet schemaSet = inference.InferSchema(reader);
            foreach (XmlSchema schema in schemaSet.Schemas())
            {
                TextWriter target = new StringWriter();
                schema.Write(target);
                target.Close();

                StreamWriter sw = new StreamWriter(physicalXsdFilePath);
                sw.Write(target.ToString());
                sw.Close();
            }
            return xml;
        }
        #endregion
    }
   
    /// <summary>
    /// validates xml
    /// </summary>
    public class ValidateXml
    {
        private List<Exception> validationExceptions;

        /// <summary>
        /// Gets or sets the validation exceptions.
        /// </summary>
        /// <value>The validation exceptions.</value>

        public List<Exception> ValidationExceptions
        {
            get { return validationExceptions; }
        }



        /// <summary>

        /// Validates the specified XML.
        /// </summary>

        /// <param name="xml">The XML.</param>
        /// <param name="xsd">The XSD.</param>

        public bool Validate(string xml, string xsd)
        {
            validationExceptions = new List<Exception>();
            XmlParserContext context = new XmlParserContext(null, new XmlNamespaceManager(new NameTable()), null, XmlSpace.None);


            XmlTextReader xmlReader = new XmlTextReader(xml, XmlNodeType.Document, context);
            XmlTextReader xsdReader = new XmlTextReader(xsd, XmlNodeType.Document, context);

            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.ValidationType = ValidationType.Schema;
            readerSettings.Schemas.Add("", xsdReader);
            readerSettings.ConformanceLevel = ConformanceLevel.Auto;

            XmlReader objValidator = XmlReader.Create(xmlReader, readerSettings);
            try
            {
                while (objValidator.Read()) { }
            }
            catch (Exception ex)
            {
                validationExceptions.Add(ex);
            }

            finally
            {
                objValidator.Close();
                xmlReader.Close();
                xsdReader.Close();
            }
            return (validationExceptions.Count == 0);
        }

    }

}





