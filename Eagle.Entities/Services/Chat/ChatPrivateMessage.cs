using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Services.Chat
{
    [Table("ChatPrivateMessage", Schema = "Chat")]
    public class ChatPrivateMessage: EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string MasterEmailId { get; set; }
        public string ChatToEmailId { get; set; }
        public string Message { get; set; }
    }
}
