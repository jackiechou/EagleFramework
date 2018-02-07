using System;
using System.Diagnostics;
using Autofac.Features.Indexed;

namespace Eagle.Infrastructure.Messaging
{
    public class ServiceBusPublisherBase : IServiceBusPublisherBase
    {
        protected IIndex<ServiceBusQueueType, IServiceBus> ServiceBuses;

        public ServiceBusPublisherBase(IIndex<ServiceBusQueueType, IServiceBus> serviceBus)
        {
            ServiceBuses = serviceBus;
        }

        #region Public Mehtods
        public void PublishAddRetryMessage<T>(ServiceBusQueueType serviceBusQueue, T message)
            where T : MessageBase
        {
            try
            {
                message.RetryAttempt += 1;
                var addRetryMessage = new AddRetryMessage<T>(serviceBusQueue, message);
                ServiceBuses[ServiceBusQueueType.TaskSchedulerPublisher].Publish(addRetryMessage);
            }
            catch (Exception e)
            {
                Trace.TraceInformation("PublishAddRetryMessage Exception {0} {1} {2}", message.MessageId, e.Message, e.StackTrace);
                throw;
            }
            
        }
        #endregion

        #region Private Methods
        private bool _isDisposed;
        protected virtual void Dispose(bool isDisposing)
        {
            if (_isDisposed) return;

            ServiceBuses = null;

            _isDisposed = true;

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
        #endregion
    }
}
