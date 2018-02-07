using System;
using Eagle.Core.Extension;
using Eagle.Repositories;
using Microsoft.AspNet.SignalR;

namespace Eagle.Services.Service.MessageBroadCaster
{
    /// <summary>
    /// BroadCaster class
    /// </summary>
    public class BroadCastService : BaseService, IBroadCastService
    {
        private EventHandler<BroadCastEventArgs> _messageListenedHandler;
        private readonly object _eventLocker = new object();
        public BroadCastService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region Public Methods 

        /// <summary>
        /// BroadCaster class
        /// </summary>
        /// <param name="messageRequest">MessageRequest value</param>
        public void BroadCast(MessageRequest messageRequest)
        {
            EventHandler<BroadCastEventArgs> handler;
            lock (_eventLocker)
            {
                handler = _messageListenedHandler;
                if (handler != null)
                {
                    handler(this, new BroadCastEventArgs(messageRequest));
                }
            }

        }

        #endregion

        #region Public Event 

        /// <summary>
        /// Message Listened event handler
        /// </summary>
        public event EventHandler<BroadCastEventArgs> MessageListened
        {
            add
            {
                lock (_eventLocker)
                {
                    _messageListenedHandler += value;
                }
            }
            remove
            {
                lock (_eventLocker)
                {
                    _messageListenedHandler -= value;
                }
            }
        }

        #endregion
    }
}
