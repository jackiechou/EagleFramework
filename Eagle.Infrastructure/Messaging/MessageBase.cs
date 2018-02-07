using System;

namespace Eagle.Infrastructure.Messaging
{
    [Serializable]
    public abstract class MessageBase
    {
        #region public properties
        public Guid MessageId { get; set; }
        public string CorrelationId { get; set; }
        public DateTime ExpiredDate { get; set; }
        public DateTime CreatedUtcDateTime { get; set; }
        public DateTime? LastAttemptedUtcDateTime { get; set; }
        public int RetryAttempt { get; set; }
        public int MaxRetryAttempt { get; set; }
        #endregion

        protected MessageBase()
        {
            MessageId = Guid.NewGuid();
            CreatedUtcDateTime = DateTime.UtcNow;
        }
    }
}
