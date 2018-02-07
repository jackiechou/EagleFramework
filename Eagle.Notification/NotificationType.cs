using System;

namespace Eagle.Notification
{
    [Flags]
    public enum NotificationType
    {
        ACCOUNT_CREATED = 1,
        DOCUMENT_CREATED = 2,
        STATUS_CHANGED = 3,
    }
}
