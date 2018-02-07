using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Services.Messaging
{
    [NotMapped]
    public class NotificationTypeTree : NotificationType
    {
        public List<NotificationTypeTree> Parents { get; set; }
        public List<NotificationTypeTree> Children { get; set; }

        public NotificationTypeTree()
        {
            Children = new List<NotificationTypeTree>();
            Parents = new List<NotificationTypeTree>();
        }
    }
}
