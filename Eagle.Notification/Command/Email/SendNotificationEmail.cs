using System;
using Eagle.Infrastructure.Messaging;

namespace Eagle.Notification.Command.Email
{
    [Serializable]
    public class SendNotificationEmail : CommandMessage
    {
        public int NetworkId { get; set; }
        public int MemberId { get; set; }
        public int TargetMemberId { get; set; }
        public MessageType MessageTemplateType { get; set; }
        public NotificationType NotificationType { get; set; }

        protected SendNotificationEmail()
        {
            Version = 1;
            MaxRetryAttempt = 5;
        }
    }
}
