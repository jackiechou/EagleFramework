using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Services.Chat
{
    public class ChatUserEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ConnectionId")]
        public string ConnectionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "UserName")]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        public string EmailId { get; set; }
    }
}
