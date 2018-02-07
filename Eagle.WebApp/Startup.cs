using System;
using Eagle.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

//[assembly: OwinStartupAttribute(typeof(Eagle.WebApp.Startup))]
[assembly: OwinStartup(typeof(Eagle.WebApp.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Log4Net.config", Watch = true)]
namespace Eagle.WebApp
{
    public partial class Startup : ServiceStartup
    {
        public void Configuration(IAppBuilder app)
        { 
            GlobalHost.Configuration.MaxIncomingWebSocketMessageSize = null; 
            // Unlimited incoming message size. Represents the amount of time to leave a connection open before timing out. Default is 110 seconds.
            GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(50);
            //Represents the amount of time to wait after a connection goes away before raising the disconnect event. Default is 30 seconds.
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(30);
            //Representing the amount of time to wait before sending a keep alive packet over an idle connection.
            //Set to null to disable keep alive. This is set to 30 seconds by default. When this is on, the ConnectionTimeout will have no effect.
            GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(10);
            GlobalHost.Configuration.TransportConnectTimeout = TimeSpan.FromSeconds(15); //5s
            //Setting up the message buffer size
            GlobalHost.Configuration.DefaultMessageBufferSize = 500;

            //app.UseCors(CorsOptions.AllowAll);
            app.Map("/signalr", map =>
            {
                // Setup the CORS middleware to run before SignalR.
                // By default this will allow all origins. You can 
                // configure the set of origins and/or http verbs by
                // providing a cors options with a different policy.
                map.UseCors(CorsOptions.AllowAll);
                map.RunSignalR(new HubConfiguration
                {
                    EnableDetailedErrors = true,
                    EnableJavaScriptProxies = true,
                    //EnableJSONP = true
                });
            });


        }

        
    }
}
