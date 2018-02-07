 using System;

namespace Eagle.Infrastructure.Messaging
{
    public interface IServiceBus : IDisposable
    {
        //void CreateBus(string queueName);

        void Publish<T>(T message) where T : MessageBase;

        void Publish(object message, Type messageType);
    }
}
