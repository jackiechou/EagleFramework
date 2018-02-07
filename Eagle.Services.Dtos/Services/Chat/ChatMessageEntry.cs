using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Services.Chat
{
    public class ChatMessageEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "UserName")]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Message")]
        public string Message { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmailId")]
        public string Email { get; set; }
    }
}
