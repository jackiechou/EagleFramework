using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Serialization;
using Eagle.Common.Services.Mail;
using Eagle.Common.Services.Parse;
using Eagle.Common.Utilities;
using Eagle.Core.Configuration;
using Eagle.Core.Permission;
using Eagle.Entities.Services.Messaging;
using Eagle.Repositories;
using Eagle.Resources;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Message;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.Messaging.Validations;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using Microsoft.AspNet.Identity;
using Attachment = System.Net.Mail.Attachment;
using MailMessage = System.Net.Mail.MailMessage;

namespace Eagle.Services.Messaging
{
    public class MailService : BaseService, IMailService, IIdentityMessageService
    {
        public IApplicationService ApplicationService { get; set; }
        private IMessageService MessageService { get; set; }
        public MailService(IUnitOfWork unitOfWork, IApplicationService applicationService, IMessageService messageService) : base(unitOfWork)
        {
            ApplicationService = applicationService;
            MessageService = messageService;
        }


        #region Send Mail with Message Queue
        private string SerializeMessage(EmailMessage message)
        {

            var outStream = new StringWriter();
            var s = new XmlSerializer(typeof(EmailMessage));
            s.Serialize(outStream, message);
            return outStream.ToString();
        }
        public bool SendMail(EmailMessage mailMsg, out string message)
        {
            message = string.Empty;
            var violations = new List<RuleViolation>();
            var senderMailProfile = UnitOfWork.NotificationSenderRepository.FindByEmailAddress(mailMsg.From);
            if (senderMailProfile == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundNotificationSender,
                                       "NotificationSender", null,
                                       ErrorMessage.Messages[ErrorCode.NotFoundNotificationSender]));
                throw new ValidationError(violations);
            }
            
            var result = MailHandler.Send_Mail_By_Gmail(senderMailProfile.MailAddress, senderMailProfile.Password, mailMsg.To, mailMsg.Subject, mailMsg.Body);
            if (result)
            {
                message = "Send mail successfully";
            }
            return result;
        }

        public bool SendMailQueue(Guid applicationId, MessageQueue message)
        {
            Logger.Debug($"message:{message}");
            if (message == null) return false;

            var violations = new List<RuleViolation>();
            var mailServerInfo = GetDefaultSmtpInfo(applicationId);
            if (mailServerInfo == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundForMailSystem, "MailSystem"));
                throw new ValidationError(violations);
            }

            string smtpServer = mailServerInfo.SmtpServer;
            string smtpAuthentication = mailServerInfo.SmtpAuthentication;
            string smtpUsername = mailServerInfo.SmtpUsername;
            string smtpPassword = mailServerInfo.SmtpPassword;
            bool smtpEnableSsl = mailServerInfo.EnableSsl;

            Logger.Debug($"smtp:{smtpServer}, user: {smtpUsername}");

            string outResult;
            bool result = MailHandler.SendMail(message.From, message.To, message.From.Split('@')[0], message.To.Split('@')[0], message.Cc, message.Bcc, null,
                MailPriority.Normal, message.Subject,
                true, Encoding.UTF8, message.Body, null, smtpServer, smtpAuthentication, smtpUsername, smtpPassword,
                smtpEnableSsl, out outResult);

            Logger.Debug($"smtp-result:{result}");
            return result;
        }
        #endregion

        #region Send Mail through Template
        public bool SendGMailByTemplate(Guid applicationId, Hashtable templateVariables, int templateId, string mailTo, string cc, string bcc)
        {
            var messageTemplateEntity = UnitOfWork.MessageTemplateRepository.FindById(templateId);
            string subject = messageTemplateEntity.TemplateName;
            string body = ParseTemplateHandler.ParseTemplate(templateVariables, messageTemplateEntity.TemplateBody);

            //STMP Info
            var hostSettings = GetDefaultSmtpInfo(applicationId);
            if (hostSettings == null) return false;
            string result;
           
            string mailFromName = hostSettings.SmtpmEmail.Split('@')[0];
            string mailToName = mailTo.Split('@')[0];
            return MailHandler.SendMail(hostSettings.SmtpmEmail, mailTo, mailFromName, mailToName, cc, bcc, null, MailPriority.Normal, subject, true, Encoding.UTF8, body, null,
                hostSettings.SmtpServer, SmtpAuthentication.Basic, hostSettings.SmtpUsername, hostSettings.SmtpPassword, hostSettings.EnableSsl, out result);
        }
        public bool SendMailWithTlsByTemplate(Guid applicationId, Hashtable templateVariables, int templateId, string mailTo, string cc, string bcc, out string message)
        {
            var messageTemplateentity = UnitOfWork.MessageTemplateRepository.FindById(templateId);
            string subject = messageTemplateentity.TemplateName;
            string body = ParseTemplateHandler.ParseTemplate(templateVariables, messageTemplateentity.TemplateBody);

            //STMP Info
            var hostSettings = GetDefaultSmtpInfo(applicationId);
            var smtpSetting = ApplicationService.GetSmtpSetting(applicationId, Smtp.EnableSsl);
            bool enableSsl = Convert.ToBoolean(smtpSetting.Setting.KeyValue);
          
            return MailHandler.SendMailWithTls(hostSettings.SmtpmEmail, mailTo, cc, bcc, null, MailPriority.Normal, subject, true,
             Encoding.UTF8, body, null, hostSettings.SmtpServer, SmtpAuthentication.Basic, hostSettings.SmtpUsername, hostSettings.SmtpPassword, enableSsl, out message);
        }

        #endregion

        #region Mail

        public SmtpInfo GetDefaultSmtpInfo(Guid applicationId)
        {
            var enableSslSetting = ApplicationService.GetSmtpSetting(applicationId, Smtp.EnableSsl);
            bool enableSsl = Convert.ToBoolean(enableSslSetting.Setting.KeyValue);
            
            var authenticationSetting = ApplicationService.GetSmtpSetting(applicationId, Smtp.Authentication);
            var authentication = authenticationSetting.Setting.KeyValue;
            
            var sender = UnitOfWork.NotificationSenderRepository.GetDefaultNotificationSender();
            if (sender == null) return null;

            var smtpInfo = new SmtpInfo
            {
                SmtpServer = sender.MailServerProvider.OutgoingMailServerHost + ":" + sender.MailServerProvider.OutgoingMailServerPort,
                SmtpAuthentication = authentication,
                SmtpmEmail = sender.MailAddress,
                SmtpUsername = sender.MailAddress.Split('@')[0],
                SmtpPassword = sender.Password,
                MailSignature = sender.Signature,
                EnableSsl = enableSsl,
            };

            return smtpInfo;
        }

        public void ProcessMailInQueueFromSystem(Guid applicationId, string to, string bcc, string cc, DateTime? predefinedDate, int templateId, Hashtable templateVariables)
        {
            var violations = new List<RuleViolation>();
            var mailSystemInfo = GetDefaultSmtpInfo(applicationId);
            if (mailSystemInfo == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundForMailSystem, "MailSystem", null, ErrorMessage.Messages[ErrorCode.NotFoundForMailSystem]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(to)) return;

            var messageTemplateEntity = UnitOfWork.MessageTemplateRepository.FindById(Convert.ToInt32(templateId));
            if (messageTemplateEntity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundForMessageTemplate, "MessageTemplate", templateId, ErrorMessage.Messages[ErrorCode.NotFoundForMessageTemplate]));
                throw new ValidationError(violations);
            }

            string body = ParseTemplateHandler.ParseTemplate(templateVariables, messageTemplateEntity.TemplateBody);
            var entry = new MessageQueueEntry
            {
                From = mailSystemInfo.SmtpmEmail,
                To = to,
                Subject = messageTemplateEntity.TemplateSubject,
                Body = body,
                Bcc = bcc,
                Cc = cc,
                PredefinedDate = predefinedDate
            };

            MessageService.CreateMessageQueue(entry);
        }

        public async Task SendAsync(IdentityMessage message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            //await configSendGridasync(message);
        }

        //private async Task configSendGridasync(IdentityMessage message)
        //{
        //    if (message == null)
        //        throw new ArgumentNullException("message");

        //    var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY",
        //        EnvironmentVariableTarget.User);
        //    //var credentials = new NetworkCredential(BaseConstants.CONFIGURATION_EMAIL_SERVICE_ACCOUNT,
        //    //                                       BaseConstants.CONFIGURATION_EMAIL_SERVICE_PASSWORD);
        //    //var transportWeb = new Web(credentials);
        //    //await transportWeb.DeliverAsync(myMessage);
        //    //Content content = new Content("text/html", message.Body);
        //    var client = new SendGridClient(apiKey);
        //    var mailFrom = new EmailAddress(BaseConstants.CONFIGURATION_EMAIL_SERVICE_EMAIL_ADDRESS,
        //        BaseConstants.CONFIGURATION_EMAIL_SERVICE_DISPLAY_NAME);
        //    var mailTo = message.Destination;

        //    var msg = new SendGridMessage();
        //    msg.SetFrom(mailFrom);
        //    msg.SetSubject(message.Subject);
        //    msg.AddTo(mailTo);

        //    //string plainTextContent = message.Body;
        //    //if (!string.IsNullOrEmpty(message.Body))
        //    //{
        //    //    msg.AddContent(MimeType.Text, message.Body);
        //    //}
        //    string htmlContent = message.Body;
        //    if (!string.IsNullOrEmpty(htmlContent))
        //    {
        //        msg.AddContent(MimeType.Html, htmlContent);
        //    }

        //    var response = await client.SendEmailAsync(msg);
        //}

        //public void SendResetPasswordEmail(string userEmail, string userName, string callbackUrl)
        //{
        //    Logger.Info($"email of user = {userEmail}, username = {userEmail}, callbackUrl = {callbackUrl}");

        //    if (string.IsNullOrEmpty(userEmail))
        //    {
        //        Logger.Error("email of user is null");
        //        throw new ArgumentNullException("userEmail");
        //    }
        //    if (string.IsNullOrEmpty(userName))
        //    {
        //        Logger.Error("username is null");
        //        throw new ArgumentNullException("userName");
        //    }
        //    if (string.IsNullOrEmpty(callbackUrl))
        //    {
        //        Logger.Error("callbackUrl is null");
        //        throw new ArgumentNullException("callbackUrl");
        //    }

        //    int emailTemplateId = Convert.ToInt32(MessageTemplateType.ResetPassword);
        //    MessageTemplate template = UnitOfWork.MessageTemplateRepository.FindById(emailTemplateId);
        //    if (template == null)
        //        throw new InvalidOperationException(string.Format("Email Template {0} does not exist", emailTemplateId));
        //    Hashtable templateVariables = new Hashtable();
        //    templateVariables.Add("Username", userName);
        //    templateVariables.Add("CallbackUrl", callbackUrl);

        //    string from = template.TemplateFrom;
        //    string to = userEmail;
        //    string subject = template.TemplateSubject;
        //    string body = ParseTemplateHandler.ParseTemplate(templateVariables, template.TemplateBody);
        //    notificationService.CreateEmailMessageQueue(from, to, subject, body);

        //    UnitOfWork.SaveChanges();
        //}

        public void SendEmail(Guid applicationId, string fromAddress, string senderAddress, string toAddress, string subject, string body)
        {
            var hostSettings = GetDefaultSmtpInfo(applicationId);
            if ((string.IsNullOrEmpty(hostSettings.SmtpServer)))
            {
                //throw new InvalidOperationException("SMTP Server not configured");
                return;
            }


            MailMessage emailMessage = new MailMessage(fromAddress, toAddress, subject, body);
            emailMessage.Sender = new MailAddress(senderAddress);

            if (HtmlUtils.IsHtml(body))
            {
                emailMessage.IsBodyHtml = true;
            }

            SmtpClient smtpClient = new SmtpClient(hostSettings.SmtpServer);

            string[] smtpHostParts = hostSettings.SmtpServer.Split(':');
            if (smtpHostParts.Length > 1)
            {
                smtpClient.Host = smtpHostParts[0];
                smtpClient.Port = Convert.ToInt32(smtpHostParts[1]);
            }


            switch (hostSettings.SmtpAuthentication)
            {
                case "":
                case "0":
                    // anonymous
                    break;
                case "1":
                    // basic
                    if (!string.IsNullOrEmpty(hostSettings.SmtpUsername) & !string.IsNullOrEmpty(hostSettings.SmtpPassword))
                    {
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential(hostSettings.SmtpUsername, hostSettings.SmtpPassword);
                    }
                    break;
                case "2":
                    smtpClient.UseDefaultCredentials = true;
                    break;
            }

            smtpClient.EnableSsl = hostSettings.EnableSsl;

            //Retry up to 5 times to send the message
            for (int index = 0; index < 5; index++)
            {
                try
                {
                    smtpClient.Send(emailMessage);
                    return;
                }
                catch (System.Exception ex)
                {
                    if (index == 5)
                    {
                        ex.ToString();
                    }
                    Thread.Sleep(1000);
                }
            }
        }

        public void SendEmail(Guid applicationId, string fromAddress, string toAddress, string subject, string body)
        {
            SendEmail(applicationId, fromAddress, fromAddress, toAddress, subject, body);
        }
        public void SendMail(Guid applicationId, string mailFrom, string mailFromPass, string mailTo, string subject, string body)
        {
            if (string.IsNullOrEmpty(mailFrom))
            {
                throw new System.Exception("Sender email used to send  notifications is not defined in app.config.");
            }
            MailHandler.Send_Mail_By_Gmail(mailFrom, mailFromPass, mailTo, subject, body);
        }
        public string SendMail(Guid applicationId, string mailFrom, string mailTo, string cc, string bcc, string replyTo, MailPriority priority, string subject, bool isBodyHtml, Encoding bodyEncoding, string body,
        List<Attachment> attachments, string smtpServer, string smtpAuthentication, string smtpUsername, string smtpPassword, bool smtpEnableSsl)
        {
            var hostSettings = GetDefaultSmtpInfo(applicationId);
            string retValue = "";

            if (!ValidatorUtils.IsValidEmailAddress(mailFrom))
            {
                ArgumentException ex = new ArgumentException(mailFrom);
                return ex.Message;
            }

            if (string.IsNullOrEmpty(smtpServer) && !string.IsNullOrEmpty(hostSettings.SmtpServer))
            {
                smtpServer = hostSettings.SmtpServer;
            }
            if (string.IsNullOrEmpty(smtpAuthentication) && !string.IsNullOrEmpty(hostSettings.SmtpAuthentication))
            {
                smtpAuthentication = hostSettings.SmtpAuthentication;
            }
            if (string.IsNullOrEmpty(smtpUsername) && !string.IsNullOrEmpty(hostSettings.SmtpUsername))
            {
                smtpUsername = hostSettings.SmtpUsername;
            }
            if (string.IsNullOrEmpty(smtpPassword) && !string.IsNullOrEmpty(hostSettings.SmtpPassword))
            {
                smtpPassword = hostSettings.SmtpPassword;
            }
            mailTo = mailTo.Replace(";", ",");
            cc = cc.Replace(";", ",");
            bcc = bcc.Replace(";", ",");


            MailMessage mailMessage = new MailMessage();
            if (!String.IsNullOrEmpty(mailFrom))
            {
                mailMessage.From = new MailAddress(mailFrom);
                //mailMessage.From = new MailAddress(MailFrom, SmtpUsername, System.Text.UTF8Encoding.UTF8);
            }
            if (!String.IsNullOrEmpty(mailTo))
            {
                mailMessage.To.Add(mailTo);
                //mailMessage.To.Add(new MailAddress(MailTo, StringUtils.GetEmailAccount(MailTo), System.Text.UTF8Encoding.UTF8));
            }
            if (!String.IsNullOrEmpty(cc))
                mailMessage.CC.Add(cc);
            if (!String.IsNullOrEmpty(bcc))
                mailMessage.Bcc.Add(bcc);
            if (replyTo != string.Empty)
                mailMessage.ReplyToList.Add(new MailAddress(replyTo));
            //mailMessage.Priority = (System.Net.Mail.MailPriority)Priority;
            mailMessage.IsBodyHtml = isBodyHtml;

            if (attachments != null)
            {
                foreach (Attachment myAtt in attachments)
                {
                    mailMessage.Attachments.Add(myAtt);
                }
            }
            mailMessage.HeadersEncoding = bodyEncoding;
            mailMessage.SubjectEncoding = bodyEncoding;
            mailMessage.Subject = HtmlUtils.StripWhiteSpace(subject, true);
            mailMessage.BodyEncoding = bodyEncoding;
            mailMessage.Body = body;

            //if (HtmlUtils.IsHtml(Body))
            if (isBodyHtml)
            {
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8, "text/html");
                mailMessage.AlternateViews.Add(htmlView);
            }
            else
            {
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(HtmlUtils.ConvertToText(body), Encoding.UTF8, "text/plain");
                mailMessage.AlternateViews.Add(plainView);
            }
            if (mailMessage != null)
            {
                int smtpPort = Null.NullInteger;
                int portPos = smtpServer.IndexOf(":", StringComparison.Ordinal);
                if (portPos > -1)
                {
                    smtpPort = Int32.Parse(smtpServer.Substring(portPos + 1, smtpServer.Length - portPos - 1));
                    smtpServer = smtpServer.Substring(0, portPos);
                }

                /* Set the SMTP server and send the email*/
                SmtpClient smtpClient = new SmtpClient();

                try
                {
                    if (!String.IsNullOrEmpty(smtpServer))
                    {
                        smtpClient.Host = smtpServer;
                        if (smtpPort > Null.NullInteger)
                            smtpClient.Port = smtpPort;
                        smtpClient.Timeout = 100000;
                        //smtpClient.EnableSsl = SMTPEnableSSL;
                        smtpClient.EnableSsl = true;
                        // ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                        //Add this line to bypass the certificate validation
                        ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                                System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                System.Security.Cryptography.X509Certificates.X509Chain chain,
                                System.Net.Security.SslPolicyErrors sslPolicyErrors)
                        {
                            return true;
                        };

                        var smtpUserInfo = new NetworkCredential(smtpUsername, smtpPassword);
                        smtpClient.Credentials = smtpUserInfo;

                        switch (smtpAuthentication)
                        {
                            case "":
                            case "0":
                                break;
                            case "1":
                                if (!String.IsNullOrEmpty(smtpUsername) && !String.IsNullOrEmpty(smtpPassword))
                                {
                                    smtpClient.UseDefaultCredentials = true;
                                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                                }
                                break;
                            case "2":
                                smtpClient.UseDefaultCredentials = true;
                                break;
                        }
                        smtpClient.Send(mailMessage);
                    }
                    else
                    {
                        retValue = "SmtpServer is empty";
                    }
                }
                catch (SmtpFailedRecipientException exc)
                {
                    retValue = "FailedRecipient :" + exc.FailedRecipient;
                }
                catch (SmtpException exc)
                {
                    retValue = "SMTPConfigurationProblem" + exc;
                }
                catch (System.Exception objException)
                {
                    retValue = objException.InnerException != null ? string.Concat(objException.Message, Environment.NewLine, objException.InnerException.Message) : objException.Message;
                }
                finally
                {
                    mailMessage.Dispose();
                }
            }
            return retValue;
        }
        public string SendEmail(Guid applicationId, string fromAddress, string senderAddress, string toAddress, string subject, string body, List<Attachment> attachments)
        {
            var hostSettings = GetDefaultSmtpInfo(applicationId);

            if ((string.IsNullOrEmpty(hostSettings.SmtpServer)))
            {
                return "SMTP Server not configured";
            }

            MailMessage emailMessage = new MailMessage(fromAddress, toAddress, subject, body);
            emailMessage.Sender = new MailAddress(senderAddress);

            if ((HtmlUtils.IsHtml(body)))
            {
                emailMessage.IsBodyHtml = true;
            }

            foreach (Attachment myAtt in attachments)
            {
                emailMessage.Attachments.Add(myAtt);
            }

            SmtpClient smtpClient = new SmtpClient(hostSettings.SmtpServer);

            string[] smtpHostParts = hostSettings.SmtpServer.Split(':');
            if (smtpHostParts.Length > 1)
            {
                smtpClient.Host = smtpHostParts[0];
                smtpClient.Port = Convert.ToInt32(smtpHostParts[1]);
            }


            switch (hostSettings.SmtpAuthentication)
            {
                case "":
                case "0":
                    // anonymous
                    break;
                case "1":
                    // basic
                    if (!string.IsNullOrEmpty(hostSettings.SmtpUsername) & !string.IsNullOrEmpty(hostSettings.SmtpPassword))
                    {
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential(hostSettings.SmtpUsername, hostSettings.SmtpPassword);
                    }
                    break;
                case "2":
                    // NTLM
                    smtpClient.UseDefaultCredentials = true;
                    break;
            }

            smtpClient.EnableSsl = hostSettings.EnableSsl;

            //'Retry up to 5 times to send the message
            for (int index = 1; index <= 5; index++)
            {
                try
                {
                    smtpClient.Send(emailMessage);
                    return "";
                }
                catch (System.Exception ex)
                {
                    if ((index == 5))
                    {
                        ex.ToString();
                    }
                    Thread.Sleep(1000);
                }
            }

            return "";
        }
        public string SendMailWithTls(Guid applicationId, string mailFrom, string mailTo, string cc, string bcc, string replyTo, MailPriority priority, string subject, bool isBodyHtml, Encoding bodyEncoding, string body,
      List<Attachment> attachments)
        {
            var hostSettings = GetDefaultSmtpInfo(applicationId);

            string retValue = "";

            if (!ValidatorUtils.IsValidEmailAddress(mailFrom))
            {
                ArgumentException ex = new ArgumentException(mailFrom);
                return ex.Message;
            }

            string smtpServer = hostSettings.SmtpServer;
            string smtpUsername = hostSettings.SmtpUsername;
            string smtpPassword = hostSettings.SmtpPassword;
            string smtpAuthentication = hostSettings.SmtpAuthentication;

            mailTo = mailTo.Replace(";", ",");
            cc = cc.Replace(";", ",");
            bcc = bcc.Replace(";", ",");


            MailMessage mailMessage = new MailMessage();
            if (!String.IsNullOrEmpty(mailFrom))
            {
                mailMessage.From = new MailAddress(mailFrom);
                //mailMessage.From = new MailAddress(MailFrom, SmtpUsername, System.Text.UTF8Encoding.UTF8);
            }
            if (!String.IsNullOrEmpty(mailTo))
            {
                mailMessage.To.Add(mailTo);
                //mailMessage.To.Add(new MailAddress(MailTo, StringUtils.GetEmailAccount(MailTo), System.Text.UTF8Encoding.UTF8));
            }
            if (!String.IsNullOrEmpty(cc))
                mailMessage.CC.Add(cc);
            if (!String.IsNullOrEmpty(bcc))
                mailMessage.Bcc.Add(bcc);
            if (replyTo != string.Empty)
                mailMessage.ReplyToList.Add(new MailAddress(replyTo));
            //mailMessage.Priority = (System.Net.Mail.MailPriority)Priority;
            mailMessage.IsBodyHtml = isBodyHtml;

            if (attachments != null)
            {
                foreach (Attachment myAtt in attachments)
                {
                    mailMessage.Attachments.Add(myAtt);
                }
            }
            mailMessage.HeadersEncoding = bodyEncoding;
            mailMessage.SubjectEncoding = bodyEncoding;
            mailMessage.Subject = HtmlUtils.StripWhiteSpace(subject, true);
            mailMessage.BodyEncoding = bodyEncoding;
            mailMessage.Body = body;

            //if (HtmlUtils.IsHtml(Body))
            if (isBodyHtml)
            {
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8, "text/html");
                mailMessage.AlternateViews.Add(htmlView);
            }
            else
            {
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(HtmlUtils.ConvertToText(body), Encoding.UTF8, "text/plain");
                mailMessage.AlternateViews.Add(plainView);
            }
            int smtpPort = Null.NullInteger;
            int portPos = smtpServer.IndexOf(":", StringComparison.Ordinal);
            if (portPos > -1)
            {
                smtpPort = Int32.Parse(smtpServer.Substring(portPos + 1, smtpServer.Length - portPos - 1));
                smtpServer = smtpServer.Substring(0, portPos);
            }

            /* Set the SMTP server and send the email*/
            var smtpClient = new SmtpClient();

            try
            {
                if (!String.IsNullOrEmpty(smtpServer))
                {
                    smtpClient.Host = smtpServer;
                    if (smtpPort > Null.NullInteger)
                        smtpClient.Port = smtpPort;
                    smtpClient.Timeout = 100000;
                    //smtpClient.EnableSsl = SMTPEnableSSL;
                    smtpClient.EnableSsl = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

                    NetworkCredential smtpUserInfo = new NetworkCredential(smtpUsername, smtpPassword);
                    smtpClient.Credentials = smtpUserInfo;

                    switch (smtpAuthentication)
                    {
                        case "":
                        case "0":
                            break;
                        case "1":
                            if (!String.IsNullOrEmpty(smtpUsername) && !String.IsNullOrEmpty(smtpPassword))
                            {
                                smtpClient.UseDefaultCredentials = true;
                                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                            }
                            break;
                        case "2":
                            smtpClient.UseDefaultCredentials = true;
                            break;
                    }

                    //Add this line to bypass the certificate validation
                    ServicePointManager.ServerCertificateValidationCallback = delegate
                    {
                        return true;
                    };
                    smtpClient.Send(mailMessage);
                }
                else
                {
                    retValue = "SmtpServer is empty";
                }
            }
            catch (SmtpFailedRecipientException exc)
            {
                retValue = "FailedRecipient :" + exc.FailedRecipient;
            }
            catch (SmtpException exc)
            {
                retValue = "SMTPConfigurationProblem" + exc;
            }
            catch (System.Exception objException)
            {
                retValue = objException.InnerException != null ? string.Concat(objException.Message, Environment.NewLine, objException.InnerException.Message) : objException.Message;
            }
            finally
            {
                mailMessage.Dispose();
            }
            return retValue;
        }

        #endregion

        #region Mail Server Provider
        public IEnumerable<MailServerProviderDetail> GetMailServerProviders(out int recordCount, string orderBy, int? page, int? pageSize)
        {
            var entityList = UnitOfWork.MailServerProviderRepository.GetMailServerProviders(out recordCount, orderBy, page, pageSize);
            return entityList.ToDtos<MailServerProvider, MailServerProviderDetail>();
        }
        public MailServerProviderDetail GetMailServerProviderDetail(int id)
        {
            var item = UnitOfWork.MailServerProviderRepository.FindById(id);
            return item.ToDto<MailServerProvider, MailServerProviderDetail>();
        }
        public SelectList PopulateMailServerProviderSelectList(int? selectedValue = null,
         bool? isShowSelectText = true)
        {
            return UnitOfWork.MailServerProviderRepository.PopulateMailServerProviderSelectList(selectedValue, isShowSelectText);
        }
        public SelectList PopulateMailServerProtocol(string selectedValue = null, bool isShowSelectText = true)
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.POP3, Value = "POP3", Selected = (selectedValue!=null && selectedValue=="POP3") },
                new SelectListItem {Text = LanguageResource.IMAP, Value = "IMAP", Selected = (selectedValue!=null && selectedValue=="IMAP") }
            };
            if (isShowSelectText)
                lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.Select} ---", Value = "" });
            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        public MailServerProviderDetail InsertMailServerProvider(MailServerProviderEntry entry)
        {
            ISpecification<MailServerProviderEntry> validator = new MailServerProviderEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            if (!validator.IsSatisfyBy(entry, violations)) throw new ValidationError(violations);

            var entity = entry.ToEntity<MailServerProviderEntry, MailServerProvider>();
            UnitOfWork.MailServerProviderRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<MailServerProvider, MailServerProviderDetail>();
        }

        public void UpdateMailServerProvider(MailServerProviderEditEntry entry)
        {
            //Check validation
            ISpecification<MailServerProviderEditEntry> validator = new MailServerProviderEditEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            if (!validator.IsSatisfyBy(entry, violations)) throw new ValidationError(violations);

            var entity = UnitOfWork.MailServerProviderRepository.Find(entry.MailServerProviderId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMailServerProvider, "MailServerProvider"));
                throw new ValidationError(violations);
            }

            if (entity.MailServerProviderName != entry.MailServerProviderName)
            {
                var isDataDuplicate = UnitOfWork.MailServerProviderRepository.HasDataExisted(entry.MailServerProviderName);
                if (isDataDuplicate)
                {
                    violations.Add(new RuleViolation(ErrorCode.DuplicateMailServerProviderName,
                          "MailServerProviderName",
                          entry.MailServerProviderName,
                          ErrorMessage.Messages[ErrorCode.DuplicateMailServerProviderName]));
                    throw new ValidationError(violations);
                }
            }

            //Assign data
            entity.MailServerProviderName = entry.MailServerProviderName;
            entity.MailServerProtocol = entry.MailServerProtocol;
            entity.IncomingMailServerHost = entry.IncomingMailServerHost;
            entity.IncomingMailServerPort = entry.IncomingMailServerPort;
            entity.OutgoingMailServerHost = entry.OutgoingMailServerHost;
            entity.OutgoingMailServerPort = entry.OutgoingMailServerPort;
            entity.Ssl = entry.Ssl;
            entity.Tls = entry.Tls;

            UnitOfWork.MailServerProviderRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateSslStatus(int id, bool status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.MailServerProviderRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMailServerProviderId, "MailServerProviderId", id, ErrorMessage.Messages[ErrorCode.NotFoundMailServerProviderId]));
                throw new ValidationError(violations);
            }
            if (entity.Ssl == status) return;

            entity.Ssl = status;

            UnitOfWork.MailServerProviderRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateTlsStatus(int id, bool status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.MailServerProviderRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMailServerProviderId, "MailServerProviderId", id, ErrorMessage.Messages[ErrorCode.NotFoundMailServerProviderId]));
                throw new ValidationError(violations);
            }
            if (entity.Tls == status) return;

            entity.Tls = status;

            UnitOfWork.MailServerProviderRepository.Update(entity);
            UnitOfWork.SaveChanges();
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
                    ApplicationService = null;
                    MessageService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
