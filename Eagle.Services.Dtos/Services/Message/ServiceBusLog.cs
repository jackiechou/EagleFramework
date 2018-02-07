using System;

namespace Eagle.Services.Dtos.Services.Message
{
    public class ServiceBusLog<T> : IServiceBusLog<T>
    {
        public int NetworkId { get; set; }
        public MessageStatusType Status { get; set; }
        public int RetryAttempt { get; set; }
        public DateTime CreatedUtcDateTime { get; set; }
        public DateTime? LastAttemptedUtcDateTime { get; set; }
        public DateTime? SentUtcDateTime { get; set; }
        public Guid MessageId { get; set; }
        public T Message { get; set; }
    }
}
