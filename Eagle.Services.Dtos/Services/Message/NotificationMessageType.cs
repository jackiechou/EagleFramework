using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Services.Message
{
    public class NotificationMessageTypeDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "NotificationMessageTypeId")]
        public int NotificationMessageTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "NotificationTypeId")]
        public int NotificationTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MessageTypeId")]
        public int MessageTypeId { get; set; }
    }
    public class NotificationMessageTypeInfoDetail : NotificationMessageTypeDetail
    {
        public NotificationTypeDetail NotificationType { get; set; }
        public MessageTypeDetail MessageType { get; set; }
    }
}
