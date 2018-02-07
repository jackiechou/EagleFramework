using System;

namespace Eagle.Infrastructure.Messaging
{
    public abstract class ServiceBusBase : IServiceBus
    {
        public virtual void CreateBus(string queueName)
        {
            //must override by inherit class
        }

        public virtual void Publish<T>(T message)
            where T : MessageBase
        {
            //must override by inherit class
        }

        public virtual void Publish(object message, Type messageType)
        {
            //must override by inherit class   
        }

        public virtual void CreateSubscriber<T>()
        {
            //must override by inherit class or new
        }

        private bool _isDisposed = false;
        protected virtual void Dispose(bool isDisposing)
        {
            //must override by inherit class
            //if (_isDisposed) return;

            //_isDisposed = true;

        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        void IDisposable.Dispose()
        {
            Dispose();
        }
    }
}
