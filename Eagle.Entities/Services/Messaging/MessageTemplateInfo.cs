using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Services.Messaging
{
    [NotMapped]
    public class MessageTemplateInfo: MessageTemplate
    {
        public MessageType MessageType { get; set; }
    }
}
