using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Services.Chat
{
    public class ChatPrivateMessageEntry: DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "MasterEmailId")]
        public string MasterEmailId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ChatToEmailId")]
        public string ChatToEmailId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Message")]
        public string Message { get; set; }
    }
}
