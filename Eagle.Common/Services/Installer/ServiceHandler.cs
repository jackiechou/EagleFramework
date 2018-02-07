using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Eagle.Common.Services.Installer
{
    public class ServiceHandler
    {
        //Get list of installed windows services
        public static ServiceController[] GetServiceList()
        {
            ServiceController[] services = ServiceController.GetServices();
            return services;
        }

        //Check whether a service is installed
        public static bool IsServiceInstalled(string serviceName)
        {
            // get list of Windows services
            ServiceController[] services = ServiceController.GetServices();

            // try to find service name
            foreach (ServiceController service in services)
            {
                if (service.ServiceName == serviceName)
                    return true;
            }
            return false;
        }
        
        public static bool IsInstalled(string serviceName)
        {
            bool status = false;
            using (ServiceController controller = new ServiceController(serviceName))
            {
                try
                {
                    ServiceControllerStatus serviceStatus = controller.Status;
                    status = true;
                }
                catch (Exception ex) { ex.ToString(); status = false; }
            }
            return status;
        }

        public static bool IsRunning(string serviceName)
        {
            using (ServiceController controller =
                new ServiceController(serviceName))
            {
                if (!IsInstalled(serviceName)) return false;
                return (controller.Status == ServiceControllerStatus.Running);
            }
        }

        public static void RefreshService(string serviceName)
        {
            ServiceController svc = new ServiceController(serviceName);
            svc.Refresh();
        }

        public static void StartService(string serviceName, int timeoutMilliseconds)
        {
            bool exist = IsInstalled(serviceName);
            if (exist)
            {
                ServiceController svc = new ServiceController(serviceName);
                svc.MachineName = System.Environment.MachineName;
                try
                {
                    svc.Start();
                    if (timeoutMilliseconds > 0)
                    {
                        TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
                        svc.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    }
                    svc.Refresh();
                }
                catch (Exception ex)
                {                   
                    if (!EventLog.SourceExists(serviceName))
                    {
                        EventLog.CreateEventSource(serviceName, serviceName);
                    }

                    EventLog eLog = new EventLog();
                    eLog.Source = serviceName;
                    eLog.WriteEntry(@"The service could not be started. Please start the service manually. Error: " + ex.Message, EventLogEntryType.Error);
                }
            }
        }

        public static void StopService(string serviceName, int timeoutMilliseconds)
        {
            bool exist = IsInstalled(serviceName);
            if (exist)
            {
                ServiceController svc = new ServiceController(serviceName);
                if (svc.Status == ServiceControllerStatus.Running)
                {
                    svc.Stop();
                    if (timeoutMilliseconds > 0)
                    {
                        TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
                        svc.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                    }
                    svc.Refresh();
                }
            }
        }

        public static void RestartService(string serviceName, int timeoutMilliseconds)
        {
            bool exist = IsInstalled(serviceName);
            if (exist)
            {
                ServiceController svc = new ServiceController(serviceName);
                if (svc.Status == ServiceControllerStatus.Running)
                {
                    svc.Stop();                   
                    int counter = 0;
                    while (svc.Status != ServiceControllerStatus.Stopped)
                    {
                        System.Threading.Thread.Sleep(2500);
                        svc.Refresh();
                        counter++;
                        if (counter > 1000)
                        {
                            throw new System.TimeoutException("Could not stop service " + serviceName);
                        }
                    }
                    if (timeoutMilliseconds > 0)
                    {
                        int millisec1 = Environment.TickCount;
                        TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
                        svc.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                        // count the rest of the timeout
                        int millisec2 = Environment.TickCount;
                        timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));
                        svc.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    }              

                    svc.Start();
                }
            }
        }
    }
}
