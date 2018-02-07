using System;

namespace Eagle.Entities.Services.Messaging
{
    public class SmsOutLog : EntityBase
    {
       public int SmsOutId { get; set; }
       public Guid SentByUserId { get; set; }
       public DateTime SentDate { get; set; }
       public Guid RecipientUserId { get; set; }
       public string RecipientPhoneNo { get; set; }
       public int MessageParts { get; set; }
       public string TextMessage { get; set; }
       public string ConfirmationId { get; set; }
       public int AcctSmsid { get; set; }
    }
}
