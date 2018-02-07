using System;

namespace Eagle.Entities.Services.Messaging
{
    [Serializable]
    public abstract class MessageInfoBase : EntityBase
    {
        public dynamic MessageId { get; set; }

        public string Label { get; set; }
        public int RetryAttempts { get; set; }
        public bool ResendEmail { get; set; }
        public bool ResendSms { get; set; }
        public bool ResendInbox { get; set; }
    }
}
