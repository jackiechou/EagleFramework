namespace Eagle.Infrastructure.Messaging
{
    public interface IEventHandler<T>
        where T : IEvent
    {
        void Handle(T @event);

        void Retry(ServiceBusQueueType serviceBusQueue, T @event);
    }
}
