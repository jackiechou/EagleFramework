using System;

namespace Eagle.Entities.Services.Messaging
{
    [Flags]
    public enum NotificationSentStatus
    {
        Pending = 0,
        Ready = 1,
        Sent = 2,
        Retry = 4,
        Failed = 8,
        FailedException = 16
    }
}
