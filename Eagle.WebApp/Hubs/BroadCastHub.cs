using System;
using Eagle.Common.Extensions.EnumHelper;
using Eagle.Core.Extension;
using Eagle.Services.Service.MessageBroadCaster;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Eagle.WebApp.Hubs
{
    /// <summary>
    /// BroadCastHub class
    /// </summary>
    public class BroadCastHub : Hub
    {
        #region Constructor

        /// <summary>
        /// BroadCastHub class
        /// </summary>
        public BroadCastHub(IBroadCastService broadCast)
        {
            if (broadCast == null)
                throw new ArgumentNullException("BroadCast object is null !");

            BeginBroadCast(broadCast); // This will avoid an explicit call to initialize a hub by message broadcaster client
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Begin broadCast message
        /// </summary>
        /// <param name="broadCast">IBroadCast value</param>
        private void BeginBroadCast(IBroadCastService broadCast)
        {
            // Register/Attach broadcast listener event
            broadCast.MessageListened += (sender, broadCastArgs)
                =>
            {
                RegisterMessageEvents(broadCastArgs);
            };

            // Unregister/detach broadcast listener event
            broadCast.MessageListened -= (sender, broadCastArgs) => {
                RegisterMessageEvents(broadCastArgs);
            };
        }

        /// <summary>
        /// Register broadcasted message to SignalR events
        /// </summary>
        /// <param name="broadCastArgs">BroadCastEventArgs value</param>
        private void RegisterMessageEvents(BroadCastEventArgs broadCastArgs)
        {
            if (broadCastArgs != null)
            {
                MessageRequest messageRequest = broadCastArgs.MessageRequest;

                IClientProxy clientProxy = Clients.Caller;
                if (messageRequest.EventName != EventNameEnum.UNKNOWN)
                {
                    clientProxy.Invoke(messageRequest.EventName.EnumDescription(), messageRequest.Message);
                }
                else
                {
                    string errorMessage = "Unknown or empty event name is requested!";
                    clientProxy.Invoke(EventNameEnum.ON_EXCEPTION.EnumDescription(), errorMessage); // Goes to the listener
                    throw new Exception(errorMessage); // Goes to the broadcaster
                }
            }
        }

        #endregion
    }
}