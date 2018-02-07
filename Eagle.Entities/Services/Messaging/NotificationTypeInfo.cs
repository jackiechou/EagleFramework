using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Services.Messaging
{
    [NotMapped]
    public class NotificationTypeInfo : NotificationType
    {
        public NotificationTypeInfo()
        {
            Children = new List<NotificationTypeInfo>();
        }
        public List<NotificationTypeInfo> Parents { get; set; }
        public List<NotificationTypeInfo> Children { get; set; }
    }
}
