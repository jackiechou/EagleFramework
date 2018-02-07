using System;

namespace Eagle.Services.Dtos.Services.Message
{
    public interface IServiceBusLog<T>
    {
        int NetworkId { get; set; }
        MessageStatusType Status { get; set; }
        int RetryAttempt { get; set; }
        DateTime CreatedUtcDateTime { get; set; }
        DateTime? LastAttemptedUtcDateTime { get; set; }
        DateTime? SentUtcDateTime { get; set; }
        Guid MessageId { get; set; }
        T Message { get; set; }
    }
}
