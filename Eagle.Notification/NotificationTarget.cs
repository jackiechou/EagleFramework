using System;

namespace Eagle.Notification
{
    [Serializable]
    public class NotificationTarget
    {
        public NotificationTargetType NotificationTargetType { get; set; }

        public Guid Id { get; set; }
    }
}
