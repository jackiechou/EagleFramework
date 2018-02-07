using System;

namespace Eagle.Services.Dtos.Services.Message
{
    public class NotificationLog<T> : ServiceBusLog<T>
    {
        public Guid TargetMemberId { get; set; }
        public NotificationTypeModel NotificationType { get; set; }
    }
}
