using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eagle.Core.Extension;
using Eagle.Services.Service.HubConfiguration;

namespace Eagle.Services.Service.MessageBroadCaster
{
    /// <summary>
    /// IBroadCastListener interface
    /// </summary>
    public interface IBroadCastListener
    {
        /// <summary>
        /// Get IHubConfiguration value
        /// </summary>
        IHubConfiguration HubConfiguration { get; set; }

        /// <summary>
        /// Get SignalR connection status
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Listen hub event and attach the message to the client event
        /// </summary>
        /// <param name="hubEvent">Client hub event</param>
        /// <returns>string value that shows status</returns>
        string ListenHubEvent(Action<object, BroadCastEventArgs> hubEvent);

        /// <summary>
        /// Listen hub event and attach the message to the client event asynchronously
        /// </summary>
        /// <param name="hubEvent">Client hub event</param>
        /// <returns>string task object that shows status</returns>
        Task<string> ListenHubEventAsync(Action<object, BroadCastEventArgs> hubEvent);
    }
}
