using System;
using Eagle.Infrastructure.Messaging;

namespace Eagle.Notification.Command.Inbox
{
    [Serializable]
    public class SendNotificationInbox : CommandMessage
    {
        public int NetworkId { get; set; }
        public int MemberId { get; set; }
        public int TargetMemberId { get; set; }
        public MessageType MessageTemplateType { get; set; }
        public NotificationType NotificationType { get; set; }

        protected SendNotificationInbox()
        {
            Version = 1;
            MaxRetryAttempt = 5;
        }
    }
}
