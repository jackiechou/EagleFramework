using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Services.Chat
{
    [Table("ChatMessage", Schema = "Chat")]
    public class ChatMessage: EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public string EmailId { get; set; }
    }
}
