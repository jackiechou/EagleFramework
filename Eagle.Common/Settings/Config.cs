using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using System.Xml;

namespace Eagle.Common.Settings
{
    public class Config
    {
        public static string DataBaseOwner = GetDataBaseOwner();
        public static string ObjectQualifer = GetObjectQualifer();
        private string _installed = string.Empty;
        public static XmlDocument AddAppSetting(XmlDocument xmlDoc, string key, string value)
        {
            XmlElement xmlElement = default(XmlElement);

            // retrieve the appSettings node 
            XmlNode xmlAppSettings = xmlDoc.SelectSingleNode("//appSettings");

            if ((xmlAppSettings != null))
            {
                // get the node based on key
                XmlNode xmlNode = xmlAppSettings.SelectSingleNode(("//add[@key='" + key + "']"));

                if ((xmlNode != null))
                {
                    // update the existing element
                    xmlElement = (XmlElement)xmlNode;
                    xmlElement.SetAttribute("value", value);
                }
                else
                {
                    // create a new element
                    xmlElement = xmlDoc.CreateElement("add");
                    xmlElement.SetAttribute("key", key);
                    xmlElement.SetAttribute("value", value);
                    xmlAppSettings.AppendChild(xmlElement);
                }
            }

            // return the xml doc
            return xmlDoc;
        }

        public static void AddSiteMapProvider(string providerName, string providerType, string siteMapPath, string securityTrimmingEnabled)
        {
            XmlDocument xmlDoc = Load();
            XmlElement xmlElement = default(XmlElement);

            // retrieve the SiteMapProvider node 
            XmlNode xmlSiteMapSettings = xmlDoc.SelectSingleNode("//system.web/siteMap/providers");
            if ((xmlSiteMapSettings != null))
            {
                // create a new element
                XmlNode xmlNode = xmlSiteMapSettings.SelectSingleNode(("//add[@name='" + providerName + "']"));
                if (xmlNode != null)
                {
                    // update the existing element
                    xmlElement = (XmlElement)xmlNode;
                    xmlElement.SetAttribute("type", providerType);
                    xmlElement.SetAttribute("siteMapFile", siteMapPath);
                    xmlElement.SetAttribute("securityTrimmingEnabled", securityTrimmingEnabled);
                }
                else
                {
                    xmlElement = xmlDoc.CreateElement("add");
                    xmlElement.SetAttribute("name", providerName);
                    xmlElement.SetAttribute("type", providerType);
                    xmlElement.SetAttribute("siteMapFile", siteMapPath);
                    xmlElement.SetAttribute("securityTrimmingEnabled", securityTrimmingEnabled);
                    xmlSiteMapSettings.AppendChild(xmlElement);
                }
            }
            Save(xmlDoc);
        }

        public static void DeleteSiteMapProvider(string providerName)
        {
            XmlDocument xmlDoc = Load();
            // retrieve the SiteMapProvider node 
            XmlNode xmlSiteMapSettings = xmlDoc.SelectSingleNode("//system.web/siteMap/providers");
            if ((xmlSiteMapSettings != null))
            {
                // get the node based on key
                XmlNode xmlNode = xmlSiteMapSettings.SelectSingleNode(("//add[@name='" + providerName + "']"));

                if ((xmlNode != null))
                {
                    xmlSiteMapSettings.RemoveChild(xmlNode);
                }
            }
            Save(xmlDoc);
        }

        public static void BackupConfig()
        {
            string backupFolder = SystemSettings.GlbConfigFolder + "Backup_" + DateTime.UtcNow.Year.ToString() + DateTime.UtcNow.Month.ToString() + DateTime.UtcNow.Day.ToString() + DateTime.UtcNow.Hour.ToString() + DateTime.UtcNow.Minute.ToString() + "\\";

            //save the current config files
            try
            {
                Application.ApplicationInfo app = new Application.ApplicationInfo();
                if (!Directory.Exists(app.ApplicationMapPath + backupFolder))
                {
                    Directory.CreateDirectory(app.ApplicationMapPath + backupFolder);
                }

                if (File.Exists(app.ApplicationMapPath + "\\web.config"))
                {
                    File.Copy(app.ApplicationMapPath + "\\web.config", app.ApplicationMapPath + backupFolder + "web_old.config", true);
                }
            }
            catch (Exception ex)
            {
                //Error backing up old web.config 
                //This error is not critical, so can be ignored
                throw ex;
            }

        }

        public static void BackupVersionConfig()
        {
            string backupFolder = SystemSettings.GlbVersionConfigFolder + "BackupVersion_" + DateTime.UtcNow.Year.ToString() + DateTime.UtcNow.Month.ToString() + DateTime.UtcNow.Day.ToString() + DateTime.UtcNow.Hour.ToString() + DateTime.UtcNow.Minute.ToString() + "\\";

            //save the current config files
            try
            {
                Application.ApplicationInfo app = new Application.ApplicationInfo();
                if (!Directory.Exists(app.ApplicationMapPath + backupFolder))
                {
                    Directory.CreateDirectory(app.ApplicationMapPath + backupFolder);
                }

                if (File.Exists(app.ApplicationMapPath + "\\version.config"))
                {
                    File.Copy(app.ApplicationMapPath + "\\version.config", app.ApplicationMapPath + backupFolder + "version_old.config", true);
                }
            }
            catch (Exception ex)
            {
                //Error backing up old web.config 
                //This error is not critical, so can be ignored
                throw ex;
            }

        }

        public static void BackupConnStringConfig()
        {
            string backupFolder = SystemSettings.GlbConnStringConfigFolder + "BackupConnString_" + DateTime.UtcNow.Year.ToString() + DateTime.UtcNow.Month.ToString() + DateTime.UtcNow.Day.ToString() + DateTime.UtcNow.Hour.ToString() + DateTime.UtcNow.Minute.ToString() + "\\";

            //save the current config files
            try
            {
                Application.ApplicationInfo app = new Application.ApplicationInfo();
                if (!Directory.Exists(app.ApplicationMapPath + backupFolder))
                {
                    Directory.CreateDirectory(app.ApplicationMapPath + backupFolder);
                }

                if (File.Exists(app.ApplicationMapPath + "\\connectionstring.config"))
                {
                    File.Copy(app.ApplicationMapPath + "\\connectionstring.config", app.ApplicationMapPath + backupFolder + "connectionstring_old.config", true);
                }
            }
            catch (Exception ex)
            {
                //Error backing up old web.config 
                //This error is not critical, so can be ignored
                throw ex;
            }

        }

        public static string GetConnectionString(string name)
        {
            string connectionString = "";
            if (!string.IsNullOrEmpty(name))
            {
                connectionString = WebConfigurationManager.ConnectionStrings[name].ConnectionString;
            }
            return connectionString;
        }

        public static string GetSetting(string setting)
        {
            return WebConfigurationManager.AppSettings[setting];
        }

        public static object GetSection(string section)
        {
            return WebConfigurationManager.GetWebApplicationSection(section);
        }

        public static string GetNodeValue(XmlNode objNode, string nodeName)
        {
            string defaultValue = string.Empty;
            string strValue = defaultValue;

            if ((objNode[nodeName] != null))
            {
                strValue = objNode[nodeName].InnerText;

                if (string.IsNullOrEmpty(strValue) & !string.IsNullOrEmpty(defaultValue))
                {
                    strValue = defaultValue;
                }
            }

            return strValue;
        }

        public static XmlDocument Load()
        {
            return Load("web.config");
        }

        public static XmlDocument LoadVersionConfig()
        {
            return Load("version.config");
        }

        public static XmlDocument LoadConnStringConfig()
        {
            return Load("connectionstring.config");
        }

        public static XmlDocument Load(string filename)
        {
            Application.ApplicationInfo app = new Application.ApplicationInfo();
            // open the config file
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(app.ApplicationMapPath + "\\" + filename);
            // test for namespace added by Web Admin Tool
            if (!string.IsNullOrEmpty(xmlDoc.DocumentElement.GetAttribute("xmlns")))
            {
                // remove namespace
                string strDoc = xmlDoc.InnerXml.Replace("xmlns=\"http://schemas.microsoft.com/.NetConfiguration/v3.0\"", "");
                xmlDoc.LoadXml(strDoc);
            }
            return xmlDoc;
        }

        public static string Save(XmlDocument xmlDoc)
        {
            return Save(xmlDoc, "web.config");
        }

        public static string SaveConnStringConfig(XmlDocument xmlDoc)
        {
            return Save(xmlDoc, "connectionstring.config");
        }

        public static string SaveVersionConfig(XmlDocument xmlDoc)
        {
            return Save(xmlDoc, "version.config");
        }

        public static string Save(XmlDocument xmlDoc, string filename)
        {
            try
            {
                Application.ApplicationInfo app = new Application.ApplicationInfo();
                string strFilePath = app.ApplicationMapPath + "\\" + filename;
                FileAttributes objFileAttributes = FileAttributes.Normal;
                if (File.Exists(strFilePath))
                {
                    // save current file attributes
                    objFileAttributes = File.GetAttributes(strFilePath);
                    // change to normal ( in case it is flagged as read-only )
                    File.SetAttributes(strFilePath, FileAttributes.Normal);
                }
                // save the config file
                XmlTextWriter writer = new XmlTextWriter(strFilePath, null);
                writer.Formatting = Formatting.Indented;
                xmlDoc.WriteTo(writer);
                writer.Flush();
                writer.Close();
                // reset file attributes
                File.SetAttributes(strFilePath, objFileAttributes);
                return "";
            }
            catch (Exception exc)
            {
                // the file permissions may not be set properly
                return exc.Message;
            }
        }

        public static void Touch()
        {
            Application.ApplicationInfo app = new Application.ApplicationInfo();
            File.SetLastWriteTime(app.ApplicationMapPath + "\\web.config", DateTime.UtcNow);
        }

        public static void RestartApplication()
        {
            Application.ApplicationInfo app = new Application.ApplicationInfo();
            File.SetLastWriteTime(app.ApplicationMapPath + "\\version.config", DateTime.UtcNow);
        }

        public static void UpdateConnectionString(string conn)
        {
            //save the current config files
            BackupConnStringConfig();

            XmlDocument xmlConnString = LoadConnStringConfig();

            //Update ConnectionStrings
            string name = "ConnectionString";
            XmlNode xmlConnection = xmlConnString.SelectSingleNode("connectionStrings/add[@name='" + name + "']");
            UpdateAttribute(xmlConnection, "connectionString", conn);


            //Update AppSetting
            BackupVersionConfig();
            XmlDocument xmlVersion = LoadVersionConfig();
            string key = "IsInstalled";
            XmlNode xmlInstalled = xmlVersion.SelectSingleNode("appSettings/add[@key='" + key + "']");

            string dbAppKey = "DatabaseName";
            string dbName = SeparateDatabaseName(conn);
            XmlNode xmlAppSetting = xmlVersion.SelectSingleNode("appSettings/add[@key='" + dbAppKey + "']");
            UpdateAttribute(xmlAppSetting, "value", dbName);

            //save connection string then update install atrribute
            SaveConnStringConfig(xmlConnString);
            UpdateAttribute(xmlInstalled, "value", "true");
            SaveVersionConfig(xmlVersion);
            Touch();
        }

        public static string SeparateDatabaseName(string conn)
        {
            string dbName = string.Empty;
            string[] connectionParams = conn.Split(';');
            foreach (string connectionParam in connectionParams)
            {
                int index = connectionParam.IndexOf("=");
                if (index > 0)
                {
                    string key = connectionParam.Substring(0, index);
                    string value = connectionParam.Substring(index + 1);
                    switch (key.ToLower())
                    {
                        case "database":
                        case "initial catalog":
                            dbName = value;
                            break;
                    }
                }
            }
            return dbName;
        }

        public static string UpdateMachineKey()
        {
            string backupFolder = SystemSettings.GlbConfigFolder + "Backup_" + DateTime.UtcNow.Year.ToString() + DateTime.UtcNow.Month.ToString() + DateTime.UtcNow.Day.ToString() + DateTime.UtcNow.Hour.ToString() + DateTime.UtcNow.Minute.ToString() + "\\";
            XmlDocument xmlConfig = new XmlDocument();
            string strError = "";
            //save the current config files
            BackupVersionConfig();
            try
            {
                // open the version.config
                xmlConfig = LoadVersionConfig();
            }
            catch (Exception ex)
            {
                strError += ex.Message;
            }
            // save a copy of the version.config
            strError += Save(xmlConfig, backupFolder + "web_.config");

            // save the version.config
            strError += SaveVersionConfig(xmlConfig);
            return strError;
        }

        private static XmlDocument UpdateMachineKey(XmlDocument xmlConfig)
        {
            Config objSecurity = new Config();
            string validationKey = objSecurity.CreateKey(20);
            string decryptionKey = objSecurity.CreateKey(24);

            XmlNode xmlMachineKey = xmlConfig.SelectSingleNode("configuration/system.web/machineKey");
            UpdateAttribute(xmlMachineKey, "validationKey", validationKey);
            UpdateAttribute(xmlMachineKey, "decryptionKey", decryptionKey);

            xmlConfig = AddAppSetting(xmlConfig, "InstallationDate", DateTime.Today.ToShortDateString());
            xmlConfig = AddAppSetting(xmlConfig, "IsInstalled", "true");

            return xmlConfig;

        }

        public static void UpdateVersion(string version)
        {
            //save the current config files
            BackupVersionConfig();

            XmlDocument xmlConfig = LoadVersionConfig();
            string key = "Version";
            XmlNode xmlConnection = xmlConfig.SelectSingleNode("appSettings/add[@key='" + key + "']");
            UpdateAttribute(xmlConnection, "value", version);

            string key1 = "InstallationDate";
            XmlNode xmlInstalledDate = xmlConfig.SelectSingleNode("appSettings/add[@key='" + key1 + "']");
            UpdateAttribute(xmlInstalledDate, "value", DateTime.Today.ToShortDateString());

            SaveVersionConfig(xmlConfig);
        }

        public string CreateKey(int numBytes)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[numBytes];

            rng.GetBytes(buff);

            return BytesToHexString(buff);
        }

        private string BytesToHexString(byte[] bytes)
        {
            StringBuilder hexString = new StringBuilder(64);

            int counter = 0;
            for (counter = 0; counter <= bytes.Length - 1; counter++)
            {
                hexString.Append(String.Format("{0:X2}", bytes[counter]));
            }

            return hexString.ToString();
        }

        public static void UpdateAttribute(XmlNode node, string attName, string attValue)
        {
            if ((node != null))
            {
                XmlAttribute attrib = node.Attributes[attName];
                attrib.InnerText = attValue;
            }
        }

        public static string GetProviderPath(string type)
        {
            XmlNode section = (XmlNode)GetSection("eagle/" + type);
            string providerPath = section.Attributes["providerPath"].ToString();
            return providerPath;
        }

        public static string GetDataBaseOwner()
        {
            string databaseOwner = GetSetting("databaseOwner").ToString();
            if (databaseOwner != "" && databaseOwner.EndsWith(".") == false)
            {
                databaseOwner += ".";
            }
            return databaseOwner;
        }

        public static string GetObjectQualifer()
        {
            string objectQualifier = GetSetting("objectQualifier").ToString();
            if ((objectQualifier != "") && (objectQualifier.EndsWith("_") == false))
            {
                objectQualifier += "_";
            }
            return objectQualifier;
        }

        public static Version GetDatabaseVersion()
        {
            string databaseVersion = GetSetting("databaseVersion").ToString();
            return new Version(databaseVersion);
        }

        public static string GetInstallOrNot()
        {
            string installed = GetSetting("installed").ToString();
            return installed;
        }

    }
}
