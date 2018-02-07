using System;

namespace Eagle.Infrastructure.Messaging
{
    public interface IServiceBusPublisherBase : IDisposable
    {
        void PublishAddRetryMessage<T>(ServiceBusQueueType serviceBusQueue, T @event)
            where T : MessageBase;
    }
}
