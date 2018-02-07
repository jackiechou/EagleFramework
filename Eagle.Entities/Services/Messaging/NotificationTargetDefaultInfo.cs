using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Services.Messaging
{
    [NotMapped]
    public class NotificationTargetDefaultInfo : NotificationTargetDefault
    {
        public string MailAccount { get; set; }
        public virtual MailServerProvider MailServerProvider { get; set; }
    }
}
