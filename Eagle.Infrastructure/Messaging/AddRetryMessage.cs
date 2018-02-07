
namespace Eagle.Infrastructure.Messaging
{
    public class AddRetryMessage<T> : CommandMessage
        where T : MessageBase
    {
        #region Public Properties
        public ServiceBusQueueType ServiceBusQueue { get; private set; }

        public T Message { get; private set; }
        #endregion

        public AddRetryMessage(ServiceBusQueueType serviceBusQueue, T message)
        {
            ServiceBusQueue = serviceBusQueue;
            Message = message;
        }
    }
}
