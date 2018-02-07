using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.Services.Messaging;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Services.Messaging
{
    public interface IMessageTemplateRepository : IRepositoryBase<MessageTemplate>
    {
        IEnumerable<MessageTemplateInfo> GetMessageTemplates(string searchText, int? messageTypeId, bool? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<MessageTemplate> GetMessageTemplatesByNotificationTypeId(int notificationTypeId);
        MessageTemplate GetMessageTemplateDetail(int notificationTypeId, int messageTypeId);
        MessageTemplate FindByName(string typeName);
        MessageTemplate GetDetails(int id);
        string GetTemplateContent(int id);
        bool HasDataExists(string templateName);
        SelectList PopulateMessageTemplateStatus(bool? selectedValue = null, bool? isShowSelectText = true);
        string ParseTemplateByTemplateId(Hashtable templateVariables, int templateId);

    }
}
