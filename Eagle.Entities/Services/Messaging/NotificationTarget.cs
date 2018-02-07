using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Services.Messaging
{
    [Table("NotificationTarget", Schema = "Messaging")]
    public class NotificationTarget: EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationTargetId { get; set; }
        public int NotificationTypeId { get; set; }
        public NotificationTargetType NotificationTargetTypeId { get; set; }
    }
}