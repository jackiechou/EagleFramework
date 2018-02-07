using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Services.Messaging
{
    [Table("NotificationTargetDefault", Schema = "Messaging")]
    public class NotificationTargetDefault: EntityBase
    {
        public NotificationTargetDefault()
        {
            TargetNo = Guid.NewGuid().ToString();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationTargetDefaultId { get; set; }
        public int? MailServerProviderId { get; set; }
        public string TargetNo { get; set; }
        public string TargetName { get; set; }
        public string ContactName { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string MailAddress { get; set; }
        public string Password { get; set; }
        public bool IsSelected { get; set; }
        public bool IsActive { get; set; }
    }
}
