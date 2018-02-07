using Eagle.Common.Services.Logs;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Text;

namespace Eagle.Common.Services.Installer
{
    public class RegistryManager
    {
        public static string GetValueFromRegistryKey(string keyName)
        {
            RegistryKey myKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\" + keyName, false);
            string myValue = (string)myKey.GetValue(keyName);
            return myValue;
        }


        #region Software Registry =================================================================================
        public static bool IsSoftwareInstalled(string p_name)
        {
            string displayName;
            RegistryKey key;

            // search in: CurrentUser
            key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            foreach (String keyName in key.GetSubKeyNames())
            {
                RegistryKey subkey = key.OpenSubKey(keyName);
                displayName = subkey.GetValue("DisplayName") as string;
                if (p_name.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    return true;
                }
            }

            // search in: LocalMachine_32
            key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            foreach (String keyName in key.GetSubKeyNames())
            {
                RegistryKey subkey = key.OpenSubKey(keyName);
                displayName = subkey.GetValue("DisplayName") as string;
                if (p_name.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    return true;
                }
            }

            // search in: LocalMachine_64
            key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall");
            foreach (String keyName in key.GetSubKeyNames())
            {
                RegistryKey subkey = key.OpenSubKey(keyName);
                displayName = subkey.GetValue("DisplayName") as string;
                if (p_name.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    return true;
                }
            }

            // NOT FOUND
            return false;

        }
        public static void CreateSoftwareRegistry(string softwareName)
        {        
            RegistryKey reg = Registry.LocalMachine.OpenSubKey("Software", true);
            if (reg != null)
            {
                RegistryKey subKey =reg.OpenSubKey(softwareName);
                if (subKey == null)                
                    reg.CreateSubKey(softwareName);                 
            }else
                EventLogManager.WriteEntryInEventLog(softwareName, "Error trying to install " + softwareName, EventLogEntryType.Error);               
            reg.Close();
        }

        public static void EditSoftwareRegistry(string softwareName, string content)
        {
            RegistryKey myKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\"+softwareName, true);
            myKey.SetValue(softwareName, content, RegistryValueKind.DWord);
        }

        public static void DeleteSoftwareRegistry(string softwareName)
        {
            RegistryKey reg = Registry.LocalMachine.OpenSubKey("Software", true);
            if (reg != null)
            {
                RegistryKey subKey =reg.OpenSubKey(softwareName);
                if(subKey != null)                
                    Registry.LocalMachine.OpenSubKey("Software", true).DeleteSubKeyTree(softwareName);               
                else
                    EventLogManager.WriteEntryInEventLog(softwareName, "Error unable to uninstall the application " + softwareName, EventLogEntryType.Error);               
            }
        }
        #endregion ================================================================================================

        #region Service Registry ==================================================================================
        public static void CreateServiceInstallerInRegistry(string serviceName)
        {
            string keyName = @"SYSTEM\CurrentControlSet\Services\" + serviceName;
            RegistryKey rk = Registry.LocalMachine.OpenSubKey(keyName);
            if (rk == null)
                Registry.LocalMachine.CreateSubKey(keyName);            
        }

        public static void RemoveServiceInstallerInRegistry(string serviceName)
        {
            string keyName = @"SYSTEM\CurrentControlSet\Services\" + serviceName;
            RegistryKey rk = Registry.LocalMachine.OpenSubKey(keyName);
            if (rk != null)
                Registry.LocalMachine.DeleteSubKey(keyName);
        }

        public static void CreateEventLogInstallerInRegistry(string serviceName, string eventLogName)
        {
            // Check whether registry key for source exists
            string keyName = @"SYSTEM\CurrentControlSet\Services\EventLog\" + eventLogName + @"\" + serviceName;
            RegistryKey rkEventSource = Registry.LocalMachine.OpenSubKey(keyName);
            // Check whether keys exists
            if (rkEventSource == null)
            {
                // Key doesnt exist. Create key which represents source
                Registry.LocalMachine.CreateSubKey(keyName + @"\" + serviceName);
                //Process Proc = new Process();
                //ProcessStartInfo ProcStartInfo = new ProcessStartInfo("Regedit.exe");
                //ProcStartInfo.Arguments = @"add HKLM\" + keyName;
                //ProcStartInfo.UseShellExecute = true;
                //ProcStartInfo.Verb = "runas";
                //Proc.StartInfo = ProcStartInfo;
                //Proc.Start();
            }
        }

        public static void RemoveEventLogInstallerOutOfRegistry(string serviceName, string eventLogName)
        {
            // Check whether registry key for source exists
            string keyName = @"SYSTEM\CurrentControlSet\Services\EventLog\" + eventLogName + @"\" + serviceName;
            RegistryKey rkEventSource = Registry.LocalMachine.OpenSubKey(keyName);
            // Check whether keys exists
            if (rkEventSource != null)
                Registry.LocalMachine.DeleteSubKey(keyName + @"\" + serviceName);
        }

        public static void WriteEntryToEventViewLogWithRegistry(int eventId, string sourceName, string eventLogName, EventLogEntryType type, string logMessage)
        {
            System.Diagnostics.EventLog eventLog;
            eventLog = new System.Diagnostics.EventLog();
            eventLog.Log = eventLogName;

            // set default event source (to be same as event log name) if not passed in
            if ((sourceName == null) || (sourceName.Trim().Length == 0))
            {
                sourceName = eventLogName;
            }

            eventLog.Source = sourceName;

            // Extra Raw event data can be added (later) if needed
            byte[] rawEventData = Encoding.ASCII.GetBytes("");

            /// Check whether the Event Source exists. It is possible that this may
            /// raise a security exception if the current process account doesn't
            /// have permissions for all sub-keys under 
            /// HKEY_LOCAL_MACHINE\System\CurrentControlSet\Services\EventLog

            // Check whether registry key for source exists

            string keyName = @"SYSTEM\CurrentControlSet\Services\EventLog\" + eventLogName;

            RegistryKey rkEventSource = Registry.LocalMachine.OpenSubKey(keyName + @"\" + sourceName);

            // Check whether key exists
            if (rkEventSource == null)
            {
                /// Key does not exist. Create key which represents source
                Registry.LocalMachine.CreateSubKey(keyName + @"\" + sourceName);
            }

            /// Now validate that the .NET Event Message File, EventMessageFile.dll (which correctly
            /// formats the content in a Log Message) is set for the event source
            object eventMessageFile = rkEventSource.GetValue("EventMessageFile");

            /// If the event Source Message File is not set, then set the Event Source message file.
            if (eventMessageFile == null)
            {
                /// Source Event File Doesn't exist - determine .NET framework location,
                /// for Event Messages file.
                RegistryKey dotNetFrameworkSettings = Registry.LocalMachine.OpenSubKey(
                    @"SOFTWARE\Microsoft\.NetFramework\");

                if (dotNetFrameworkSettings != null)
                {

                    object dotNetInstallRoot = dotNetFrameworkSettings.GetValue(
                        "InstallRoot",
                        null,
                        RegistryValueOptions.None);

                    if (dotNetInstallRoot != null)
                    {
                        string eventMessageFileLocation =
                                dotNetInstallRoot.ToString() +
                                "v" +
                                System.Environment.Version.Major.ToString() + "." +
                                System.Environment.Version.Minor.ToString() + "." +
                                System.Environment.Version.Build.ToString() +
                                @"\EventLogMessages.dll";

                        /// Validate File exists
                        if (System.IO.File.Exists(eventMessageFileLocation))
                        {
                            /// The Event Message File exists in the anticipated location on the
                            /// machine. Set this value for the new Event Source

                            // Re-open the key as writable
                            rkEventSource = Registry.LocalMachine.OpenSubKey(keyName + @"\" + sourceName, true);

                            // Set the "EventMessageFile" property
                            rkEventSource.SetValue("EventMessageFile", eventMessageFileLocation, RegistryValueKind.String);
                        }
                    }
                }

                dotNetFrameworkSettings.Close();
            }

            rkEventSource.Close();

            /// Log the message
            eventLog.WriteEntry(logMessage, type, eventId, 0, rawEventData);
        }
        #endregion =========================================================================================

    }
}
