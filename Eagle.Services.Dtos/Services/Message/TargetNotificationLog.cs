using System;

namespace Eagle.Services.Dtos.Services.Message
{
    public class TargetNotificationLog<T> : ServiceBusLog<T>
    {
        public Guid MemberId { get; set; }
        public NotificationTypeModel NotificationTypeModel { get; set; }
        public NotificationTargetDetail NotificationTargetModel { get; set; }
    }
}
