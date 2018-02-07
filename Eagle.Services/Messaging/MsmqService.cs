using System;
using System.Globalization;
using System.IO;
using System.Messaging;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using Eagle.Common.Settings;
using Eagle.EntityFramework.Repositories;
using Eagle.Repositories;
using Eagle.Services.Dtos.Services.Message;

namespace Eagle.Services.Messaging
{
    public class MsmqService : BaseService, IMsmqService
    {
        private static MessageQueue _msmQ;
        private IMailService MailService { get; set; }
        public MsmqService(IUnitOfWork unitOfWork, IMailService mailService)
            : base(unitOfWork, new IRepository[] {}, new IBaseService[] { mailService })
        {
            MailService = mailService;
        }


        /// <summary>
        /// Setting is set in app config of window service or webconfig
        /// If the setting vaule is null, the default value is used
        /// </summary>
        /// <returns></returns>
        public string GetQueuePath()
        {
            string queueName = ConfigSettings.ReadSetting("MSMQName");
            if (string.IsNullOrEmpty(queueName))
            {
                queueName = @".\Private$\EmailQueue";
            }
            return queueName;
        }
        public string QueueMessage(EmailMessage mailMessage)
        {
            try
            {
                if (mailMessage == null){ 
                    return "Mail message is null";
                }

                var message = new System.Messaging.Message {
                    Label = $"{DateTime.UtcNow.ToString("yyyyMMdd", CultureInfo.InvariantCulture)}-{DateTime.UtcNow.Millisecond}",
                    Body = mailMessage,
                    Recoverable = true,
                    Formatter = new XmlMessageFormatter(new[] { typeof(EmailMessage) })
                };

                string queueName = GetQueuePath();
                _msmQ = !MessageQueue.Exists(queueName) ? MessageQueue.Create(queueName) : new MessageQueue(queueName);
                _msmQ.Formatter = new XmlMessageFormatter(new[] {typeof (EmailMessage)});
                _msmQ.Send(message);
                return "Create message successfully";
            }
            catch (MessageQueueException mqe)
            {
                return mqe.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {
                _msmQ.Close();
            }
        }
        public string SendMessageToQueue(string queuePath, EmailMessage mailMessage)
        {
            if (string.IsNullOrEmpty(queuePath))
            {
                throw new Exception("Message queue path has been not defined in app.config.");
            }

            if (mailMessage == null)
            {
                return "Mail message is null";
            }

            // check if queue exists, if not create it
            _msmQ = !MessageQueue.Exists(queuePath) ? MessageQueue.Create(queuePath) : new MessageQueue(queuePath);

            try
            {
                var message = new System.Messaging.Message
                {
                    Label = $"{DateTime.UtcNow.ToString("yyyyMMdd", CultureInfo.InvariantCulture)}-{DateTime.UtcNow.Millisecond}",
                    Body = mailMessage,
                    Recoverable = true,
                    Formatter = new XmlMessageFormatter(new[] { typeof(EmailMessage) })
                };

                _msmQ.Send(message);
                return "Create message successfully";
            }
            catch (MessageQueueException mqe)
            {
                return mqe.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {
                _msmQ.Close();
            }
        }
        public string ReceiveMessageFromQueue(out string message)
        {
            string queuePath = GetQueuePath();
            var msMq = new MessageQueue(queuePath);
            string result = string.Empty;
            message = string.Empty;

            try
            {
                msMq.Formatter = new XmlMessageFormatter(new[] { typeof(EmailMessage) });
                var receive = msMq.Receive();
                if (receive != null)
                {
                    var messageInfo = (EmailMessage)receive.Body;

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(messageInfo.From);
                    sb.AppendLine(messageInfo.To);
                    sb.AppendLine(messageInfo.Subject);
                    sb.AppendLine(messageInfo.Body);
                    result = sb.ToString();

                    sb.Insert(0, "Message received ......\n\n");
                    message = sb.ToString();

                   // MailService mailService= new MailService();
                    MailService.SendMail(messageInfo, out message);
                    message = "Mail is sent successfuly";
                }
            }
            catch (MessageQueueException mqe)
            {
                message = mqe.Message;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                msMq.Close();
            }
            return result;
        }
        public string ReceiveMessageFromQueue(string queuePath, out string message)
        {
            if (string.IsNullOrEmpty(queuePath))
            {
                throw new Exception("Message queue path not defined in app.config.");
            }

            var msMq = new MessageQueue(queuePath);
            string result = string.Empty;
            message = string.Empty;

            try
            {
                msMq.Formatter = new XmlMessageFormatter(new[] { typeof(EmailMessage) });
                var receive = msMq.Receive();
                if (receive != null)
                {
                    var messageInfo = (EmailMessage)receive.Body;

                    StringBuilder sb= new StringBuilder();
                    sb.AppendLine(messageInfo.From);
                    sb.AppendLine(messageInfo.To);
                    sb.AppendLine(messageInfo.Subject);
                    sb.AppendLine(messageInfo.Body);
                    result = sb.ToString();

                    sb.Insert(0, "Message received ......\n\n");
                    message = sb.ToString();

                    MailService.SendMail(messageInfo, out message);
                    message = "Mail is sent successfuly";
                }
            }
            catch (MessageQueueException mqe)
            {
                message = mqe.ToString();
            }
            catch (Exception ex)
            {
                message = ex.ToString();
            }
            finally
            {
                msMq.Close();
            }
            return result;
        }
        public bool ReceiveMessagesFromQueue(string queuePath, out string message)
        {
            if (string.IsNullOrEmpty(queuePath))
            {
                throw new Exception("Message queue path not defined in app.config.");
            }

            var msMq = new MessageQueue(queuePath);
            message = string.Empty;
            bool result = false;
            try
            {
                var msgs = msMq.GetAllMessages();
                foreach (var msg in msgs)
                {
                    msg.Formatter = new XmlMessageFormatter(new[] { typeof(EmailMessage) });
                    var messageInfo = (EmailMessage) msg.Body;

                    result = MailService.SendMail(messageInfo, out message);
                }
                message = "Mail is sent successfuly";

                //var sb = new StringBuilder();
                //foreach (var msg in msgs)
                //{
                //    var reader = new StreamReader(msg.BodyStream);
                //    var msGtext = reader.ReadToEnd();
                //    string text = (string)XmlDeserializeFromString(msGtext);
                //    sb.AppendLine(text);
                //}
                //message = sb.ToString();
            }
            catch (MessageQueueException mqe)
            {
                message =mqe.ToString();
            }
            catch (Exception ex)
            {
                message = ex.ToString();
            }
            finally
            {
                msMq.Close();
            }
            return result;
        }
        public string DeteleteMessageQueue()
        {
            string result = string.Empty;
            string queueName = GetQueuePath();
            if (!string.IsNullOrEmpty(queueName))
            {
                if (!MessageQueue.Exists(queueName))
                {
                    result = "Message queue does not exist";
                }

                MessageQueue.Delete(queueName);
                result = "Message queue is deleted";
            }
            return result;
        }
        public void DeteleteMessageQueues()
        {
            var msmques = MessageQueue.GetPrivateQueuesByMachine(".");
            foreach (var item in msmques)
            {
                MessageQueue.Delete(".\\" + item.QueueName);
            }
        }
        public object XmlDeserializeFromString(string objectData)
        {
            var serializer = new XmlSerializer(typeof(string));
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }
            return result;
        }

        #region Message Queue Console
        public void GetMailMessages()
        {
            //try
            //{
                string queueName = GetQueuePath();
                if (!MessageQueue.Exists(queueName)) return;
                _msmQ = new MessageQueue(queueName) { Formatter = new XmlMessageFormatter(new[] { typeof(EmailMessage) }) };
                _msmQ.MessageReadPropertyFilter.SetAll();
                _msmQ.ReceiveCompleted += new ReceiveCompletedEventHandler(msgQ_ReceiveCompleted);
                _msmQ.BeginReceive();

                Console.WriteLine("Enter 'A' to Exit");
                while (Console.ReadKey().Key != ConsoleKey.A)
                {
                    Thread.Sleep(0);
                }

            //}
            //catch (Exception oEx)
            //{
            //    Console.WriteLine(oEx.Message);
            //    Console.ReadKey();
            //}
        }

        /// <summary>
        /// Handles the ReceiveCompleted event of the msmq Queue request.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Messaging.ReceiveCompletedEventArgs"/> instance containing the event data.</param>
        public void msgQ_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            //try
            //{

            //MessageQueue msQueue = (MessageQueue)sender;
            //System.Messaging.Message msMessage = null;
            //msMessage = msQueue.EndReceive(e.AsyncResult);

            EmailMessage mailMsg = (EmailMessage)e.Message.Body;
            Console.WriteLine("Received message is " + mailMsg.Body);

            // Assuming user enters messge in proper format of mail.
            string result;

           // MailService mailService = new MailService();
            MailService.SendMail(mailMsg, out result);

            Console.WriteLine("Message received: " + result);
            Console.ReadLine();

            ////begin receiving again
            //msQueue.BeginReceive();

            _msmQ.Close();
            
            //}
            //catch (Exception oEx)
            //{
            //    Console.WriteLine(oEx.Message);
            //}
        }
        #endregion

        #region Dipose

        private bool _disposed = false;
        protected override void Dispose(bool isDisposing)
        {
            if (!this._disposed)
            {
                if (isDisposing)
                {
                    MailService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
