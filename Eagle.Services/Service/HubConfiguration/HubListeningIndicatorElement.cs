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
    /// Hub listening indicator configuration element
    /// </summary>
    internal class HubListeningIndicatorElement : ConfigurationElement
    {
        /// <summary>
        /// Get or set hub enable configuration property
        /// </summary>
        [ConfigurationProperty(HubConfigurationConstants.IS_ENABLED_PROPERTY, IsRequired = true)]
        internal string IsEnabled
        {
            get
            {
                return (string)this[HubConfigurationConstants.IS_ENABLED_PROPERTY];
            }
            set
            {
                this[HubConfigurationConstants.IS_ENABLED_PROPERTY] = value;
            }
        }
    }
}
