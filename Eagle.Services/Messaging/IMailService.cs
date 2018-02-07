using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.Services.Messaging;
using Eagle.Services.Dtos.Services.Message;

namespace Eagle.Services.Messaging
{
    public interface IMailService : IBaseService
    {
        #region Mail

        SmtpInfo GetDefaultSmtpInfo(Guid applicationId);
        void ProcessMailInQueueFromSystem(Guid applicationId, string to, string bcc, string cc, DateTime? predefinedDate, int templateId, Hashtable templateVariables);

        bool SendGMailByTemplate(Guid applicationId, Hashtable templateVariables, int templateId, string mailTo, string cc, string bcc);
        bool SendMailWithTlsByTemplate(Guid applicationId, Hashtable templateVariables, int templateId, string mailTo, string cc, string bcc,
            out string message);
        bool SendMail(EmailMessage mailMsg, out string message);
        bool SendMailQueue(Guid applicationId, MessageQueue message);

        #endregion
        
        #region MailServerProvider
        IEnumerable<MailServerProviderDetail> GetMailServerProviders(out int recordCount, string orderBy, int? page, int? pageSize);
        MailServerProviderDetail GetMailServerProviderDetail(int id);

        SelectList PopulateMailServerProviderSelectList(int? selectedValue = null,
            bool? isShowSelectText = true);
        SelectList PopulateMailServerProtocol(string selectedValue = null, bool isShowSelectText = true);
        MailServerProviderDetail InsertMailServerProvider(MailServerProviderEntry entry);

        void UpdateMailServerProvider(MailServerProviderEditEntry entry);
        void UpdateSslStatus(int id, bool status);
        void UpdateTlsStatus(int id, bool status);

        #endregion
    }
}
