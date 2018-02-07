using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using Eagle.Common.Utilities;
using MailMessage = System.Net.Mail.MailMessage;

namespace Eagle.Common.Services.Mail
{
    public static class MailHandler
    {
        public static string GetEmailAccount(string strInput)
        {
            return strInput.Substring(0, strInput.IndexOf('@'));
        }

        public static string ConvertHtmlToText(string sHtml)
        {
            string sContent = sHtml;
            sContent = sContent.Replace("&nbsp;", " ");
            sContent = sContent.Replace("<br />", Environment.NewLine);
            sContent = sContent.Replace("<br>", Environment.NewLine);
            sContent = HtmlUtils.FormatText(sContent, true);
            sContent = HtmlUtils.StripTags(sContent, true);
            return sContent;
        }

        public static bool IsValidEmailAddress(string email)
        {
            const string glbEmailRegEx = "\\b[a-zA-Z0-9._%\\-+']+@[a-zA-Z0-9.\\-]+\\.[a-zA-Z]{2,4}\\b";
            return Regex.Match(email, glbEmailRegEx).Success;
        }

        #region Gmail

        public static bool Send_Mail_By_Gmail(string mailFrom, string mailFromPass, string mailTo, string subject,
            string body)
        {
            bool flag;
            string mailFromAccount = GetEmailAccount(mailFrom);
            string mailToAccount = GetEmailAccount(mailTo);
            var smtpUserInfo = new NetworkCredential(mailFromAccount, mailFromPass);

            var mailMessage = new MailMessage
            {
                From = new MailAddress(mailFrom, mailFromAccount, Encoding.UTF8)
            };
            mailMessage.To.Add(new MailAddress(mailTo, mailToAccount, Encoding.UTF8));
            mailMessage.Subject = subject;
            mailMessage.SubjectEncoding = Encoding.UTF8;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = Encoding.UTF8;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                Timeout = 100000,
                EnableSsl = true,
                Credentials = smtpUserInfo
            };

            try
            {
                smtp.Send(mailMessage);
                flag = true;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        public static bool Send_Mail_By_Gmail(string mailNameFrom, string mailFrom, string mailFromPass,
            string mailNameTo, string mailTo, string subject, string body)
        {
            bool flag = false;

            string mailFromAccount = mailFrom.Substring(0, mailFrom.IndexOf('@'));

            var smtpUserInfo = new NetworkCredential(mailFromAccount, mailFromPass);

            var mailMessage = new MailMessage
            {
                From = new MailAddress(mailFrom, mailNameFrom.Trim(), Encoding.UTF8)
            };
            mailMessage.To.Add(new MailAddress(mailTo, mailNameTo.Trim(), Encoding.UTF8));
            mailMessage.Subject = subject;
            mailMessage.SubjectEncoding = Encoding.UTF8;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = Encoding.UTF8;
            //mailMessage.Priority = MailPriority.High;

            /* Set the SMTP server and send the email - SMTP gmail ="smtp.gmail.com" port=587*/
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                Timeout = 100000,
                EnableSsl = true,
                Credentials = smtpUserInfo
            };

            try
            {
                smtp.Send(mailMessage);
                flag = true;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return flag;
        }

        public static bool Send_Mail_By_Gmail_With_Attachment(string mailFrom, string mailFromPass, string mailTo,
            string subject, string body, string attachmentPath)
        {
            bool flag = false;
            var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            string mailFromAccount = GetEmailAccount(mailFrom);
            string mailToAccount = GetEmailAccount(mailTo);
            //Danh sách email được ngăn cách nhau bởi dấu ";"               

            String[] allEmails = mailTo.Split(';');

            foreach (string emailaddress in allEmails)
            {
                flag = regex.IsMatch(emailaddress);
            }

            if (flag)
            {
                var smtpUserInfo = new NetworkCredential(mailFromAccount, mailFromPass);
                var attachment = new Attachment(attachmentPath);
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(mailFrom, mailFromAccount, Encoding.UTF8)
                };

                mailMessage.To.Add(new MailAddress(mailTo, mailToAccount, Encoding.UTF8));
                mailMessage.Subject = subject;
                mailMessage.SubjectEncoding = Encoding.UTF8;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.Attachments.Add(attachment);
                //mailMessage.Priority = MailPriority.High;


                /* Set the SMTP server and send the email - SMTP gmail ="smtp.gmail.com" port=587*/
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Timeout = 100000,
                    EnableSsl = true,
                    Credentials = smtpUserInfo
                };

                try
                {
                    smtp.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            return flag;
        }

        public static bool Send_Mail_By_Gmail_With_BCC_Attachment(string mailFrom, string mailFromPass, string mailTo,
            string bcc, string subject, string body, string attachmentPath)
        {
            bool flag = false;
            var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            string mailFromAccount = GetEmailAccount(mailFrom);
            string mailToAccount = GetEmailAccount(mailTo);

            String[] allEmails = mailTo.Split(';');

            foreach (string emailaddress in allEmails)
            {
                flag = regex.IsMatch(emailaddress);
            }

            if (flag)
            {
                var smtpUserInfo = new NetworkCredential(mailFromAccount, mailFromPass);
                var mailMessage = new MailMessage();
                if (!String.IsNullOrEmpty(bcc))
                    mailMessage.Bcc.Add(bcc);

                mailMessage.From = new MailAddress(mailFrom, mailFromAccount, Encoding.UTF8);
                mailMessage.To.Add(new MailAddress(mailTo, mailToAccount, Encoding.UTF8));
                mailMessage.Subject = subject;
                mailMessage.SubjectEncoding = Encoding.UTF8;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = Encoding.UTF8;

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Timeout = 100000,
                    EnableSsl = true,
                    Credentials = smtpUserInfo
                };

                try
                {
                    smtp.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            return flag;
        }

        public static bool Send_Mail_By_Gmail_With_CC_BCC_Attachment(string mailFrom, string mailFromPass, string mailTo,
            string cc, string bcc, string subject, string body, string attachmentPath)
        {
            bool flag = false;
            var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            string mailFromAccount = GetEmailAccount(mailFrom);
            string mailToAccount = GetEmailAccount(mailTo);

            String[] allEmails = mailTo.Split(';');

            foreach (string emailaddress in allEmails)
            {
                flag = regex.IsMatch(emailaddress);
            }

            if (flag)
            {
                var smtpUserInfo = new NetworkCredential(mailFromAccount, mailFromPass);
                var attachment = new Attachment(attachmentPath);
                var mailMessage = new MailMessage();
                if (!String.IsNullOrEmpty(cc))
                    mailMessage.CC.Add(cc);
                if (!String.IsNullOrEmpty(bcc))
                    mailMessage.Bcc.Add(bcc);


                mailMessage.From = new MailAddress(mailFrom, mailFromAccount, Encoding.UTF8);
                mailMessage.To.Add(new MailAddress(mailTo, mailToAccount, Encoding.UTF8));
                mailMessage.Subject = subject;
                mailMessage.SubjectEncoding = Encoding.UTF8;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.Attachments.Add(attachment);

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Timeout = 100000,
                    EnableSsl = true,
                    Credentials = smtpUserInfo
                };

                try
                {
                    smtp.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            return flag;
        }

        public static bool Send_Mail_By_Gmail_With_CC_BCC(string mailFrom, string mailFromPass, string mailTo, string cc,
            string bcc, string subject, string body)
        {
            bool flag = false;
            var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

            string mailFromAccount = GetEmailAccount(mailFrom);
            string mailToAccount = GetEmailAccount(mailTo);
            String[] allEmails = mailTo.Split(';');

            foreach (string emailaddress in allEmails)
            {
                flag = regex.IsMatch(emailaddress);
            }

            if (flag)
            {
                var smtpUserInfo = new NetworkCredential(mailFromAccount, mailFromPass);
                var mailMessage = new MailMessage();
                if (!String.IsNullOrEmpty(cc))
                    mailMessage.CC.Add(cc);
                if (!String.IsNullOrEmpty(bcc))
                    mailMessage.Bcc.Add(bcc);

                mailMessage.From = new MailAddress(mailFrom, mailFromAccount, Encoding.UTF8);
                mailMessage.To.Add(new MailAddress(mailTo, mailToAccount, Encoding.UTF8));
                mailMessage.Subject = subject;
                mailMessage.SubjectEncoding = Encoding.UTF8;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = Encoding.UTF8;

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Timeout = 100000,
                    EnableSsl = true,
                    Credentials = smtpUserInfo
                };

                try
                {
                    smtp.Send(mailMessage);
                    flag = true;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            return flag;
        }
        public static bool Send_GMail(string mailNameFrom, string mailFrom, string mailFromPass, string mailNameTo,
          string mailTo, string subject, string body)
        {
            bool flag = false;
            string mailFromAccount = mailFrom.Substring(0, mailFrom.IndexOf('@'));
            var smtpUserInfo = new NetworkCredential(mailFromAccount, mailFromPass);

            var mailMessage = new MailMessage { From = new MailAddress(mailFrom, mailNameFrom, Encoding.UTF8) };
            mailMessage.To.Add(new MailAddress(mailTo, mailNameTo.Trim(), Encoding.UTF8));
            mailMessage.Subject = subject;
            mailMessage.SubjectEncoding = Encoding.UTF8;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = Encoding.UTF8;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                Timeout = 100000,
                EnableSsl = true,
                UseDefaultCredentials = true,
                Credentials = smtpUserInfo,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            try
            {
                smtp.Send(mailMessage);
                flag = true;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return flag;
        }
        #endregion

        #region Yahoo

        public static bool Send_Mail_Yahoo(string mailNameFrom, string mailFrom, string mailFromPass, string mailNameTo,
            string mailTo, string subject, string body)
        {
            bool flag = false;
            string mailFromAccount = mailFrom.Substring(0, mailFrom.IndexOf('@'));
            var smtpUserInfo = new NetworkCredential(mailFromAccount, mailFromPass);

            var mailMessage = new MailMessage
            {
                From = new MailAddress(mailFrom, mailNameFrom.Trim(), Encoding.UTF8)
            };
            mailMessage.To.Add(new MailAddress(mailTo, mailNameTo.Trim(), Encoding.UTF8));
            mailMessage.Subject = subject;
            mailMessage.SubjectEncoding = Encoding.UTF8;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = Encoding.UTF8;

            var smtp = new SmtpClient
            {
                Host = "smtp.mail.yahoo.com",
                Port = 465,
                Timeout = 100000,
                EnableSsl = true,
                Credentials = smtpUserInfo
            };

            try
            {
                smtp.Send(mailMessage);
                flag = true;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return flag;
        }

        #endregion

        #region Mail by TLS

        public static bool SendMailWithTls(string mailFrom, string mailTo, string cc, string bcc, string replyTo,
            MailPriority priority, string subject, bool isBodyHtml, Encoding bodyEncoding, string body,
            List<Attachment> attachments, string smtpServer, string smtpAuthentication, string smtpUsername,
            string smtpPassword, bool smtpEnableSsl, out string message)
        {
            bool result = false;

            if (!ValidatorUtils.IsValidEmailAddress(mailFrom))
            {
                var ex = new ArgumentException(mailFrom);
                message = ex.Message;
                return false;
            }

            mailTo = mailTo.Replace(";", ",");
            cc = cc.Replace(";", ",");
            bcc = bcc.Replace(";", ",");


            var mailMessage = new MailMessage();
            if (!String.IsNullOrEmpty(mailFrom))
            {
                mailMessage.From = new MailAddress(mailFrom);
            }
            if (!String.IsNullOrEmpty(mailTo))
            {
                mailMessage.To.Add(mailTo);
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
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(HtmlUtils.ConvertToText(body),
                    Encoding.UTF8, "text/plain");
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

                    //Add this line to bypass the certificate validation
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    smtpClient.Send(mailMessage);
                    result = true;
                    message = "Mail is sent successfully";
                }
                else
                {
                    message = "SmtpServer is empty";
                }
            }
            catch (SmtpFailedRecipientException exc)
            {
                message = "FailedRecipient :" + exc.FailedRecipient;
            }
            catch (SmtpException exc)
            {
                message = "SMTPConfigurationProblem" + exc;
            }
            catch (Exception objException)
            {
                message = objException.InnerException != null
                    ? string.Concat(objException.Message, Environment.NewLine, objException.InnerException.Message)
                    : objException.Message;
            }
            finally
            {
                mailMessage.Dispose();
            }

            return result;
        }

        #endregion

        #region Common

        public static void SendMailClient(string emailAddresses, string from, string subject, string message,
            string contents, string filename)
        {
            var mm = new MailMessage();
            mm.To.Add(emailAddresses);
            mm.From = new MailAddress(from);
            mm.Subject = subject;
            mm.Body = message;
            mm.Attachments.Add(new Attachment(StringUtils.GenerateStreamFromString(contents), filename));
            var client = new SmtpClient();
            client.Send(mm);
        }

        public static string SendMail(string mailFrom, string mailTo, string cc, string bcc, string replyTo,
            MailPriority priority, string subject, bool isBodyHtml, Encoding bodyEncoding, string body,
            List<Attachment> attachments, string smtpServer, string smtpAuthentication, string smtpUsername,
            string smtpPassword, bool smtpEnableSsl)
        {
            string retValue = "";

            if (!ValidatorUtils.IsValidEmailAddress(mailFrom))
            {
                var ex = new ArgumentException(mailFrom);
                return ex.Message;
            }

            mailTo = mailTo.Replace(";", ",");
            cc = cc.Replace(";", ",");
            bcc = bcc.Replace(";", ",");


            var mailMessage = new MailMessage();
            if (!String.IsNullOrEmpty(mailFrom))
            {
                mailMessage.From = new MailAddress(mailFrom);
            }
            if (!String.IsNullOrEmpty(mailTo))
            {
                mailMessage.To.Add(mailTo);
            }
            if (!String.IsNullOrEmpty(cc))
                mailMessage.CC.Add(cc);
            if (!String.IsNullOrEmpty(bcc))
                mailMessage.Bcc.Add(bcc);
            if (replyTo != string.Empty)
                mailMessage.ReplyToList.Add(new MailAddress(replyTo));

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

            if (isBodyHtml)
            {
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8, "text/html");
                mailMessage.AlternateViews.Add(htmlView);
            }
            else
            {
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    HtmlUtils.ConvertHtmlToText(body), Encoding.UTF8, "text/plain");
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
                    smtpClient.EnableSsl = true;
                    // ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                    //Add this line to bypass the certificate validation
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

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
                    retValue = "SMTPServer is empty";
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
            catch (Exception objException)
            {
                if (objException.InnerException != null)
                {
                    retValue = string.Concat(objException.Message, Environment.NewLine,
                        objException.InnerException.Message);
                }
                else
                {
                    retValue = objException.Message;
                }
            }
            finally
            {
                mailMessage.Dispose();
            }
            return retValue;
        }
        public static bool SendMail(string mailFrom, string mailTo, string mailFromName, string mailToName, string cc, string bcc, string replyTo,
            MailPriority priority, string subject, bool isBodyHtml, Encoding bodyEncoding, string body,
            List<Attachment> attachments, string smtpServer, string smtpAuthentication, string smtpUsername,
            string smtpPassword, bool smtpEnableSsl, out string retValue)
        {
            var result = false;
            if (!ValidatorUtils.IsValidEmailAddress(mailFrom))
            {
                retValue = new ArgumentException(mailFrom).Message;
                return false;
            }

            if (!string.IsNullOrEmpty(mailTo))
            {
                mailTo = mailTo.Replace(";", ",");
            }

            if (!string.IsNullOrEmpty(cc))
            {
                cc = cc.Replace(";", ",");
            }
            if (!string.IsNullOrEmpty(bcc))
            {
                bcc = bcc.Replace(";", ",");
            }

            var mailMessage = new MailMessage();
            if (!string.IsNullOrEmpty(mailFrom))
            {
                mailMessage.From = string.IsNullOrEmpty(mailFromName) ? new MailAddress(mailFrom) : new MailAddress(mailFrom, mailFromName);
            }
            if (!string.IsNullOrEmpty(mailTo))
            {
                if (string.IsNullOrEmpty(mailToName))
                {
                    mailMessage.To.Add(mailTo);
                }
                else
                {
                    var mailMessageTo = new MailAddress(mailTo, mailToName);
                    mailMessage.To.Add(mailMessageTo);
                }
            }
            if (!string.IsNullOrEmpty(cc))
                mailMessage.CC.Add(cc);
            if (!string.IsNullOrEmpty(bcc))
                mailMessage.Bcc.Add(bcc);
            if (!string.IsNullOrEmpty(replyTo))
                mailMessage.ReplyToList.Add(new MailAddress(replyTo));

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

            if (isBodyHtml)
            {
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8, "text/html");
                mailMessage.AlternateViews.Add(htmlView);
            }
            else
            {
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    HtmlUtils.ConvertHtmlToText(body), Encoding.UTF8, "text/plain");
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
                    // ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                    //Add this line to bypass the certificate validation
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

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

                    retValue = "Send mail successfully";
                    result = true;
                }
                else
                {
                    retValue = "SMTPServer is empty";
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
            catch (Exception objException)
            {
                if (objException.InnerException != null)
                {
                    retValue = string.Concat(objException.Message, Environment.NewLine,
                        objException.InnerException.Message);
                }
                else
                {
                    retValue = objException.Message;
                }
            }
            finally
            {
                mailMessage.Dispose();
            }
            return result;
        }
        #endregion
    }
}
