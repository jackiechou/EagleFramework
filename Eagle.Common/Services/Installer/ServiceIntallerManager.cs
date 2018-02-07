using Eagle.Common.Services.Logs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;

namespace Eagle.Common.Services.Installer
{
    /// <summary>
    /// Installs and provides functionality for handling windows services
    /// </summary>
    public class ServiceIntallerManager : System.Configuration.Install.Installer
    {        
        //public static ServiceProcessInstaller CreateServiceProcessInstaller(string serviceName, string serviceDescription)
        //{
        //    ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();
        //    serviceProcessInstaller.Account = ServiceAccount.LocalService;
        //    //serviceProcessInstaller.Password = null;
        //    //serviceProcessInstaller.Username = null;
        //    return serviceProcessInstaller;
        //}

        //public static ServiceInstaller CreateServiceInstaller(string serviceName, string serviceDescription)
        //{
        //    ServiceInstaller serviceInstaller = new ServiceInstaller();
        //    serviceInstaller.Description = serviceDescription;
        //    serviceInstaller.DisplayName = serviceName;
        //    serviceInstaller.ServiceName = serviceName;                                    
        //    serviceInstaller.StartType = ServiceStartMode.Automatic;
        //    return serviceInstaller;
        //}

        public static string GetExecutableServicePath(string serviceName)
        {
            Microsoft.Win32.RegistryKey services = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services");
            string executableServicePath = string.Empty;
            if (services != null)
            {
                object pathtoexecutable = services.OpenSubKey(serviceName).GetValue("ImagePath");
                executableServicePath = pathtoexecutable.ToString();
            }
            return executableServicePath;
        }

        //public static void Install(ServiceAccount serviceAccount, string userName, string passWord, string serviceName, string serviceDescription,string executableServicePath, string logFilePath)
        //{
        //    //string path = string.Format("/assemblypath={0}", executableServicePath);
        //    //string[] cmdline = { path };
           
        //    //if (!File.Exists(logFilePath))
        //    //    File.Create(logFilePath);

        //    //ServiceProcessInstaller processInstaller = new ServiceProcessInstaller();
        //    //processInstaller.Account = serviceAccount;
        //    //processInstaller.Username = userName;
        //    //processInstaller.Password = passWord;

        //    //ServiceInstaller serviceInstaller = new ServiceInstaller();
        //    //InstallContext installContext = new System.Configuration.Install.InstallContext();
        //    //installContext = new System.Configuration.Install.InstallContext(logFilePath, cmdline);
        //    //serviceInstaller.Context = installContext; 

        //    //serviceInstaller.DisplayName = serviceName;
        //    //serviceInstaller.Description = serviceDescription;
        //    //serviceInstaller.ServiceName = serviceName;
        //    //serviceInstaller.StartType = ServiceStartMode.Automatic;
        //    //serviceInstaller.Parent = processInstaller;

        //    //System.Collections.Specialized.ListDictionary state = new System.Collections.Specialized.ListDictionary();
        //    //serviceInstaller.Install(state);

    



        //    System.Threading.Thread.Sleep(2500);
        //    ServiceController serviceController = new ServiceController(serviceInstaller.ServiceName);
        //    serviceController.Start();           
        //}

      

        //    //serviceInstaller.Context = new System.Configuration.Install.InstallContext(logFilePath, cmdline);
        //    //serviceInstaller.Uninstall(null);

        //    System.Configuration.Install.AssemblyInstaller appInstaller = new System.Configuration.Install.AssemblyInstaller(executableServicePath);
        //    appInstaller.UseNewContext = true
        //    appInstaller.Uninstall(null);
        //}


        public static void Install(ServiceAccount serviceAccount, string userName, string passWord, string serviceName, string serviceDescription, string executableServicePath, string logFilePath)
        {
            ServiceProcessInstaller processInstaller = new ServiceProcessInstaller();
            processInstaller.Account = ServiceAccount.LocalSystem;
            processInstaller.Username = null;
            processInstaller.Password = null;

            ServiceInstaller serviceInstaller =
            new ServiceInstaller();
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.ServiceName = serviceName;
            serviceInstaller.DisplayName = serviceName;
            serviceInstaller.Description = serviceDescription;
            serviceInstaller.Parent = processInstaller;

            String path = String.Format("/assemblypath={0}", executableServicePath);
            String[] cmdline = { path };
            serviceInstaller.Context = new System.Configuration.Install.InstallContext(logFilePath, cmdline);

            System.Collections.Specialized.ListDictionary state =
            new System.Collections.Specialized.ListDictionary();
            serviceInstaller.Install(state);

            ServiceController serviceController =
            new ServiceController(serviceInstaller.ServiceName);

            serviceController.Start();
        }
        public static void Uninstall(string serviceName, string executableServicePath, string logFilePath)
        {
            string path = string.Format("/assemblypath={0}", executableServicePath);
            string[] cmdline = { path };

            if (File.Exists(logFilePath))
                File.Delete(logFilePath);

            EventLogManager.RemoveEventLog(serviceName);

            ServiceInstaller serviceInstaller = new ServiceInstaller();
            serviceInstaller.ServiceName = serviceName;

            ServiceController serviceController = new ServiceController(serviceInstaller.ServiceName);
            if ((serviceController.Status == ServiceControllerStatus.Running)
            || (serviceController.Status == ServiceControllerStatus.Paused))
            {
                serviceController.Stop();
                serviceController.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 0, 15));
                serviceController.Close();
            }
        } 
          
    }
}
