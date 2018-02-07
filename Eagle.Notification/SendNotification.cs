using System;
using Eagle.Infrastructure.Messaging;

namespace Eagle.Notification
{
    [Serializable]
    public class SendNotification : CommandMessage
    {
        public Guid NetworkId { get; set; }
        public Guid MemberId { get; set; }
        public Guid TargetMemberId { get; set; }
        public MessageType MessageTemplateType { get; set; }
        public NotificationType NotificationType { get; set; }
        public NotificationTarget NotificationTarget { get; set; }
        public bool IgnoreMemberPreference { get; set; }

        protected SendNotification()
        {
            Version = 1;
            MaxRetryAttempt = 5;
        }
    }
}
