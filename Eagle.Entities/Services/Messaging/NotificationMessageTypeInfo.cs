using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Services.Messaging
{
    [NotMapped]
    public class NotificationMessageTypeInfo: NotificationMessageType
    {
        public virtual NotificationType NotificationType { get; set; }
        public virtual MessageType MessageType { get; set; }
    }
}
