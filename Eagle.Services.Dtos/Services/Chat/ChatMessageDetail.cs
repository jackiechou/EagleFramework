using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Services.Chat
{
    public class ChatMessageDetail: DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Id")]
        public int Id { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "UserName")]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Message")]
        public string Message { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        public string Email { get; set; }
    }
}
