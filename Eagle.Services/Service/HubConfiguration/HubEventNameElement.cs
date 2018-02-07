//|---------------------------------------------------------------|
//|                  SIGNALR BROADCAST LISTENER                   |
//|---------------------------------------------------------------|
//|                     Developed by Wonde Tadesse                |
//|                        Copyright ©2015 - Present              |
//|---------------------------------------------------------------|
//|                  SIGNALR BROADCAST LISTENER                   |
//|---------------------------------------------------------------|

using System.Configuration;

namespace Eagle.Services.Service.HubConfiguration
{
    /// <summary>
    /// Hub EventName configuration element
    /// </summary>
    internal class HubEventNameElement : ConfigurationElement
    {
        /// <summary>
        /// Get or set EventName configuration property
        /// </summary>
        [ConfigurationProperty(HubConfigurationConstants.HUB_EVENT_NAME_PROPERTY, IsRequired = true)]
        public string EventName
        {
            get
            {
                return (string)this[HubConfigurationConstants.HUB_EVENT_NAME_PROPERTY];
            }
            set
            {
                this[HubConfigurationConstants.HUB_EVENT_NAME_PROPERTY] = value;
            }
        }
    }
}
