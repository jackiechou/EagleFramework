using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Services.Messaging
{
    [Table("NotificationMessageType", Schema = "Messaging")]
    public class NotificationMessageType: EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationMessageTypeId { get; set; }
        public int NotificationTypeId { get; set; }
        public int MessageTypeId { get; set; }
    }
}
