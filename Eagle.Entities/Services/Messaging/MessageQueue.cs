using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Services.Messaging
{
    [Table("MessageQueue", Schema = "Messaging")]
    public class MessageQueue : EntityBase
    {
        public MessageQueue()
        {
            Status = true;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QueueId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Bcc { get; set; }
        public string Cc { get; set; }
        public string Body { get; set; }
        public DateTime? PredefinedDate { get; set; }
        public bool Status { get; set; }

        public int? ResponseStatus { get; set; }
        public string ResponseMessage { get; set; }
        public DateTime? SentDate { get; set; }

        //public List<string> Attachments { get; set; }
        //public string ReplyTo { get; set; }
        //public string Sender { get; set; }

    }
}
