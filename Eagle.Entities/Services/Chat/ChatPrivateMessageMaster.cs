using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Services.Chat
{
    [Table("ChatPrivateMessageMaster", Schema = "Chat")]
    public class ChatPrivateMessageMaster: EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string EmailId { get; set; }
    }
}
