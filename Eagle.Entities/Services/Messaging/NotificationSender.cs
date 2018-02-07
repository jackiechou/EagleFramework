using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Services.Messaging
{
    [Table("NotificationSender", Schema = "Messaging")]
    public class NotificationSender : EntityBase
    {
        public NotificationSender()
        {
            SenderNo = Guid.NewGuid().ToString();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationSenderId { get; set; }
        public int MailServerProviderId { get; set; }
        public string SenderNo { get; set; }
        public string SenderName { get; set; }
        public string ContactName { get; set; }
        public string Mobile { get; set; }
        public string MailAddress { get; set; }
        public string Password { get; set; }
        public string Signature { get; set; }
        public bool IsSelected { get; set; }
        public bool IsActive { get; set; }

    }
}