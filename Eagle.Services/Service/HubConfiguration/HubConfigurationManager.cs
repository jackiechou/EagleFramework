using System.Configuration;
using Eagle.Services.Service.MessageBroadCaster;

namespace Eagle.Services.Service.HubConfiguration
{
    /// <summary>
    /// Database Hub configuration section reader class
    /// </summary>
    public class HubConfigurationManager : IDbListener
    {
        #region Constants 

        const string INSERT_CONFIGURATION_NAME = "hubConfigurations/insertListenerConfiguration";
        const string UPDATE_CONFIGURATION_NAME = "hubConfigurations/updateListenerConfiguration";
        const string DELETE_CONFIGURATION_NAME = "hubConfigurations/deleteListenerConfiguration";

        #endregion

        #region Public Properties 

        /// <summary>
        /// Get Insert Listener
        /// </summary>
        public IBroadCastListener InsertListener { get; private set; }

        /// <summary>
        /// Get Update Listener
        /// </summary>
        public IBroadCastListener UpdateListener { get; private set; }

        /// <summary>
        /// Get Delete Listener
        /// </summary>
        public IBroadCastListener DeleteListener { get; private set; }

        #endregion

        #region Constructor 

        /// <summary>
        /// Database Hub configuration section reader class
        /// </summary>
        public HubConfigurationManager()
        {
            // Bind hub configurations to the respective listeners
            InsertListener = new BroadCastListener((IHubConfiguration)ConfigurationManager.GetSection(INSERT_CONFIGURATION_NAME));
            UpdateListener = new BroadCastListener((IHubConfiguration)ConfigurationManager.GetSection(UPDATE_CONFIGURATION_NAME));
            DeleteListener = new BroadCastListener((IHubConfiguration)ConfigurationManager.GetSection(DELETE_CONFIGURATION_NAME));
        }

        #endregion
    }
}
