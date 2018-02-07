using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Services.Chat
{
    public class PrivateMessageEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "FromEmail")]
        public string FromEmail { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ToEmail")]
        public string ToEmail { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "UserName")]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Message")]
        public string Message { get; set; }
    }
}
