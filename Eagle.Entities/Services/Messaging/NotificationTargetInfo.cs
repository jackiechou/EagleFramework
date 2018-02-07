using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Resources;

namespace Eagle.Entities.Services.Messaging
{
    [NotMapped]
    public class NotificationTargetInfo: NotificationTarget
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TargetNo")]
        public string TargetNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TargetName")]
        public string TargetName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ContactName")]
        public string ContactName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PhoneNo")]
        public string Mobile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MailAddress")]
        public string MailAddress { get; set; }
        public virtual NotificationType NotificationType { get; set; }
    }
}
