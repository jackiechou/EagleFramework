using System.Runtime.Serialization;

namespace Eagle.Services.Exceptions
{
    public class NotificationException : System.Exception
    {
        public NotificationException()
        {
            // Add implementation (if required)
        }

        public NotificationException(string message)
            : base(message)
        {
            // Add implementation (if required)
        }

        public NotificationException(string message, System.Exception inner)
            : base(message, inner)
        {
            // Add implementation (if required)
        }

        protected NotificationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Add implementation (if required)
        }
    }

    public class NotificationChannelException : NotificationException
    { 
        public NotificationChannelException()
        {
            // Add implementation (if required)
        }

        public NotificationChannelException(string message)
            : base(message)
        {
            // Add implementation (if required)
        }

        public NotificationChannelException(string message, System.Exception inner)
            : base(message, inner)
        {
            // Add implementation (if required)
        }

        protected NotificationChannelException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Add implementation (if required)
        }
    }

    public class EmailNotificationException : NotificationChannelException
    {
          public EmailNotificationException()
            : base()
        {
            // Add implementation (if required)
        }

        public EmailNotificationException(string message)
            : base(message)
        {
            // Add implementation (if required)
        }

        public EmailNotificationException(string message, System.Exception inner)
            : base(message, inner)
        {
            // Add implementation (if required)
        }

        protected EmailNotificationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Add implementation (if required)
        }
    }
    public class SmsNotificationException : NotificationChannelException
    {
          public SmsNotificationException()
            : base()
        {
            // Add implementation (if required)
        }

        public SmsNotificationException(string message)
            : base(message)
        {
            // Add implementation (if required)
        }

        public SmsNotificationException(string message, System.Exception inner)
            : base(message, inner)
        {
            // Add implementation (if required)
        }

        protected SmsNotificationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Add implementation (if required)
        }
    }
    public class InboxNotificationException : NotificationChannelException
    {
          public InboxNotificationException()
          {
            // Add implementation (if required)
        }

        public InboxNotificationException(string message)
            : base(message)
        {
            // Add implementation (if required)
        }

        public InboxNotificationException(string message, System.Exception inner)
            : base(message, inner)
        {
            // Add implementation (if required)
        }

        protected InboxNotificationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Add implementation (if required)
        }
       
    }
}
