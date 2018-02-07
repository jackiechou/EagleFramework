#region "Copyright"
/*
5eagles® - http://www.5eagles.com.vn
Copyright (c) 2009-2012 by 5EAGLES
Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
#endregion

#region "References"

using System;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using Microsoft.Win32;

#endregion

namespace Eagle.Common.Application
{
    public class ApplicationInfo
    {
        private static ReleaseMode _status = ReleaseMode.None;

        public ApplicationInfo()
        {
            NetFrameworkIisVersion = GetNetFrameworkVersion();
            ApplicationPath = HttpContext.Current.Request.ApplicationPath;

            ApplicationMapPath = AppDomain.CurrentDomain.BaseDirectory.Substring(0, (AppDomain.CurrentDomain.BaseDirectory.Length - 1));
            ApplicationMapPath = ApplicationMapPath.Replace("/", "\\");
             
        }
        public static string CurrentDotNetVersion()
        {
            RegistryKey installedVersions = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP");
            
            string[] versionNames = installedVersions.GetSubKeyNames();
            //version names start with 'v', eg, 'v3.5' which needs to be trimmed off before conversion
            decimal framework = Convert.ToDecimal(versionNames[versionNames.Length - 1].Remove(0, 1), CultureInfo.InvariantCulture);
            int sp = Convert.ToInt32(installedVersions.OpenSubKey(versionNames[versionNames.Length - 1]).GetValue("SP", 0));
            return framework.ToString();
            
        }

        public Version NetFrameworkIisVersion { get; set; } = Environment.Version;

        public string ApplicationPath { get; set; }

        public string ApplicationMapPath { get; set; }

        public string Company => "5Eagles Co.Ltd";

        public string Description => "Eagle Community Edition";

        public string HelpUrl => "http://www.5eagles.com.vn/default.aspx";

        public string LegalCopyright => ("5Eagles Copyright 2010-"
                                         + (DateTime.Today.ToString("yyyy") + " by Eagle Corporation"));

        public string Name => "SFECORP.CE";

        public string Sku => "SFE";

        public ReleaseMode Status
        {
            get
            {
                if ((_status == ReleaseMode.None))
                {
                    Assembly assy = Assembly.GetExecutingAssembly();
                    if (Attribute.IsDefined(assy, typeof(AssemblyStatusAttribute)))
                    {
                        Attribute attr = Attribute.GetCustomAttribute(assy, typeof(AssemblyStatusAttribute));
                        if (attr != null)
                        {
                            _status = ((AssemblyStatusAttribute)(attr)).Status;
                        }
                    }
                }
                return _status;
            }
        }

        public string Title => "Eagle";

        public string Trademark => "Eagle";

        public string Type => "Framework";

        public string UpgradeUrl => "http://update.5eagles.com.vn";

        public string Url => "http://www.5eagles.com.vn";

        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;

        public string DataProvider => "SqlDataProvider";

        public string FormatVersion(Version version, bool includeBuild)
        {
            string strVersion = (version.Major.ToString("00") + ("." + (version.Minor.ToString("00") + ("." + version.Build.ToString("00")))));
            if (includeBuild)
            {
                strVersion = (strVersion + (" (" + (version.Revision + ")")));
            }
            return strVersion;
        }

        private static Version GetNetFrameworkVersion()
        {
            string version = Environment.Version.ToString(2);
            // Try and load a 3.0 Assembly
            Assembly assembly;
            try
            {
                assembly = AppDomain.CurrentDomain.Load("System.Runtime.Serialization, Version=3.0.0.0, Culture=neutral, PublicKeyToken=B77A5C561934E089");
                version = "3.0";
            }
            catch
            {
            }
            // Try and load a 3.5 Assembly
            try
            {
                assembly = AppDomain.CurrentDomain.Load("System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=B77A5C561934E089");
                version = "3.5";
            }
            catch
            {
            }
            // Try and load a 4.0 Assembly
            try
            {
                assembly = AppDomain.CurrentDomain.Load("System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=B77A5C561934E089");
                version = "4.0";
            }
            catch
            {
            }
            return new Version(version);
        }

        public string ServerIpAddress
        {
            get 
            {
                IPAddress[] ipList = Dns.GetHostEntry(DnsName).AddressList;
                StringBuilder strIpAddress = new StringBuilder();
                string strTemp = string.Empty;
                if (ipList.Length > 0)
                {
                    foreach (IPAddress ip in ipList)
                    {
                        strIpAddress.Append(ip + ", ");
                    }
                    strTemp = strIpAddress.ToString();
                    if (strTemp.Contains(","))
                        strTemp = strTemp.Remove(strTemp.LastIndexOf(",", StringComparison.Ordinal));
                }
                return strTemp;
            }
        }

        public string DnsName => Dns.GetHostName();
    }
}
