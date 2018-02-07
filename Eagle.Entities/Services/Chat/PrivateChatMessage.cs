using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Services.Chat
{
    [NotMapped]
    public class PrivateChatMessage: EntityBase
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
    }
}
