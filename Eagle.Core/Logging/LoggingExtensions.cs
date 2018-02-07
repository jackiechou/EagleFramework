using System;
using System.Xml;
using Newtonsoft.Json;

namespace Eagle.Core.Logging
{
    public static class LoggingExtensions
    {
        public static string ToXml(this Exception ex, string applicationName)
        {
            var doc = new XmlDocument();
            var rootNode = doc.CreateElement("error");
            doc.AppendChild(rootNode);

            XmlAttribute userAttr = doc.CreateAttribute("user");
            userAttr.Value = applicationName;
            rootNode.Attributes.Append(userAttr);
            XmlAttribute detailAttr = doc.CreateAttribute("detail");
            detailAttr.Value = ex.StackTrace;
            rootNode.Attributes.Append(detailAttr);
            XmlAttribute sourceAttr = doc.CreateAttribute("source");
            sourceAttr.Value = ex.Source;
            rootNode.Attributes.Append(sourceAttr);
            XmlAttribute messageAttr = doc.CreateAttribute("message");
            messageAttr.Value = ex.Message;
            rootNode.Attributes.Append(messageAttr);
            XmlAttribute typeAttr = doc.CreateAttribute("type");
            typeAttr.Value = ex.GetType().FullName;
            rootNode.Attributes.Append(typeAttr);
            XmlAttribute hostAttr = doc.CreateAttribute("host");
            hostAttr.Value = System.Net.Dns.GetHostName();
            rootNode.Attributes.Append(hostAttr);
            XmlAttribute appAttr = doc.CreateAttribute("application");
            appAttr.Value = applicationName;
            rootNode.Attributes.Append(appAttr);
            XmlAttribute timeAttr = doc.CreateAttribute("time");
            timeAttr.Value = JsonConvert.SerializeObject(DateTime.UtcNow).Replace("\"", "");
            rootNode.Attributes.Append(timeAttr);

            return doc.InnerXml.ToString();

        }
    }
}
