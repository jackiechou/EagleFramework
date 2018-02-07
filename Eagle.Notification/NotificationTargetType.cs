using System;

namespace Eagle.Notification
{
    [Flags]
    public enum NotificationTargetType
    {
        Member = 1,
        Group = 2,
        Network = 3
    }
}
