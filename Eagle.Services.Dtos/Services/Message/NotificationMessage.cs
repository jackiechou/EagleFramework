using System;
using Eagle.Entities.Services.Messaging;

namespace Eagle.Services.Dtos.Services.Message
{
    public class NotificationMessageDetail : DtoBase
    {
        public int NotificationMessageId { get; set; }
        public string MessageInfo { get; set; }
        public string ExceptionMessage { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public NotificationSentStatus SentStatus { get; set; }
    }

    public class NotificationMessageEntry : DtoBase
    {
        public MessageDetail Message { get; set; }
        public DateTime? PublishDate { get; set; }
        public NotificationSentStatus? SentStatus { get; set; }
    }

   
}
