using System;
using System.Linq;
using System.Messaging;

namespace Eagle.EntityFramework
{
    public class MsmqContext : DataContext, IMsmqContext
    {
        public MsmqContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        private MessageQueue _messageQueue;
        
        public void CreateMessageQueue(string connectionString)
        {
            _messageQueue = new MessageQueue(connectionString.Trim());
        }

        public void Insert<T>(T item, string label, bool isRetry)
        {
            var msgTx = new MessageQueueTransaction();
            try
            {
                msgTx.Begin();
                var message = new Message();
                var typeHandler = new Type[] { typeof(T) };
                message.Formatter = new XmlMessageFormatter(typeHandler);
                message.UseDeadLetterQueue = true;
                message.Body = item;
                message.Label = label;
                message.Priority = isRetry ? MessagePriority.Low : MessagePriority.Normal;
                _messageQueue.Send(message, msgTx);

                msgTx.Commit();
            }
            catch (Exception ex)
            {
                msgTx.Abort();
                throw ex;
            }

        }

        public new T Get<T>()
        {
            var typeHandler = new[] { typeof(T) };
            _messageQueue.Formatter = new XmlMessageFormatter(typeHandler);
            var result = _messageQueue.Receive();

            if (result == null) return default(T);

            return (T)result.Body;
        }

        public int GetMessageCount<T>()
        {
            var typeHandler = new Type[] { typeof(T) };
            _messageQueue.Formatter = new XmlMessageFormatter(typeHandler);

            var result = _messageQueue.GetAllMessages().Count();

            return result;
        }

        public void EnQueueItem<T>(T item, string label, string path) where T : class
        {
            MessageQueue messageQueue = null;

            if (MessageQueue.Exists(path))
                messageQueue = new MessageQueue(path);
            else
                messageQueue = MessageQueue.Create(path);

            Message message = new Message();
            var typeHandler = new Type[] { typeof(T) };
            message.Formatter = new XmlMessageFormatter(typeHandler);

            message.Body = item;
            message.Label = label;
            messageQueue.Send(message);
        }

        public T DeQueueItem<T>(string path)
        {
            var msgQ = new MessageQueue(path);
            var typeHandler = new Type[] { typeof(T) };

            msgQ.Formatter = new XmlMessageFormatter(typeHandler);

            var receive = msgQ.Receive(new TimeSpan(0, 0, 3));

            if (receive == null) return default(T);

            var msg = (T)receive.Body;

            return msg;
        }

        private bool disposed = false;

        protected override void Dispose(bool isDisposing)
        {
            if (!this.disposed)
            {
                if (isDisposing)
                {
                    if (_messageQueue != null)
                    {
                        _messageQueue.Dispose();
                        _messageQueue = null;
                    }
                }
                disposed = true;
            }
            base.Dispose(isDisposing);
        }
    }
}
