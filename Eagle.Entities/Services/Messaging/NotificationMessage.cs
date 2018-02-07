using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Services.Messaging
{
    [Table("NotificationMessage", Schema = "Messaging")]
    public class NotificationMessage : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationMessageId { get; set; }
        public string MessageInfo { get; set; }
        public string ExceptionMessage { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public NotificationSentStatus SentStatus { get; set; }

    }
}
