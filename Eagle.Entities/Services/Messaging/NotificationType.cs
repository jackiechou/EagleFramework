using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Services.Messaging
{
    [Table("NotificationType", Schema = "Messaging")]
    public class NotificationType : EntityBase
    {
        public NotificationType()
        {
            Status = NotificationTypeStatus.Active;
            CreatedDate = DateTime.UtcNow;
            ListOrder = 1;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationTypeId { get; set; }
        public int? ParentId { get; set; }
        public string NotificationTypeName { get; set; }
        public string Description { get; set; }
        public int Depth { get; set; }
        public string Lineage { get; set; }
        public bool? HasChild { get; set; }
        public int? ListOrder { get; set; }
        public NotificationTypeStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public NotificationSenderType NotificationSenderTypeId { get; set; }
    }
}
