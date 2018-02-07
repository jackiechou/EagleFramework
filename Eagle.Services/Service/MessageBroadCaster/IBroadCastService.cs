using System;
using Eagle.Core.Extension;

namespace Eagle.Services.Service.MessageBroadCaster
{
    /// <summary>
    /// IBroadCast interface
    /// </summary>
    public interface IBroadCastService : IBaseService
    {
        /// <summary>
        /// BroadCast messsage
        /// </summary>
        /// <param name="messageRequest">MessageRequest value</param>
        void BroadCast(MessageRequest messageRequest);

        /// <summary>
        /// Message Listener event handler
        /// </summary>
        event EventHandler<BroadCastEventArgs> MessageListened;
    }
}
