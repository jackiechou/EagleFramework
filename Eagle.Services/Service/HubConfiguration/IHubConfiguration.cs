using Eagle.Core.Extension;

namespace Eagle.Services.Service.HubConfiguration
{
    /// <summary>
    /// IHubConfiguation interface
    /// </summary>
    public interface IHubConfiguration
    {
        /// <summary>
        /// Get or set hub URL value
        /// </summary>
        string HubURL { get; set; }

        /// <summary>
        /// Get or set hub name value
        /// </summary>
        string HubName { get; set; }

        /// <summary>
        /// Get or set hub event name value
        /// </summary>
        EventNameEnum HubEventName { get; set; }

        /// <summary>
        /// Get or set hub listening enable value. Default is true
        /// </summary>
        bool IsHubListeningEnabled { get; set; }
    }
}
