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
    /// HubURL configuration element
    /// </summary>
    internal class HubURLElement : ConfigurationElement
    {
        /// <summary>
        /// URL configuration property
        /// </summary>
        [ConfigurationProperty(HubConfigurationConstants.HUB_URL_PROPERTY, IsRequired = true)]
        internal string URL
        {
            get
            {
                return (string)this[HubConfigurationConstants.HUB_URL_PROPERTY];
            }
            set
            {
                this[HubConfigurationConstants.HUB_URL_PROPERTY] = value;
            }
        }
    }
}
