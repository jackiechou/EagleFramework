using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Services.Messaging
{
    [Table("MessageType", Schema = "Messaging")]
    public class MessageType : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageTypeId { get; set; }
        public string MessageTypeName { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
    }
}
