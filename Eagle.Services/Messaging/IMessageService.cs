using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Services.Dtos.Services.Message;

namespace Eagle.Services.Messaging
{
    public interface IMessageService : IBaseService
    {
        #region Message Queue
        MessageQueueDetail CreateMessageQueue(MessageQueueEntry entry);
        void UpdateMessageQueue(MessageQueueEditEntry entry);
        #endregion

        #region Message Type

        IEnumerable<MessageTypeDetail> GetMessageTypes(bool? status);
        MessageTypeDetail GetMessageTypeDetail(int id);

        MultiSelectList PopulateMessageTypeMultiSelectList(bool? status = null, int[] selectedValues = null,
            bool? isShowSelectText = false);
        SelectList PopulateMessageTypeSelectList(int? selectedValue = null, bool? isShowSelectText = true);
        SelectList PopulateMessageTypeStatus(bool? selectedValue = null, bool? isShowSelectText = true);
        MessageTypeDetail InsertMessageType(MessageTypeEntry entry);

        void UpdateMessageType(MessageTypeEditEntry entry);

        void UpdateMessageTypeStatus(int id, bool status);

        #endregion

        #region Message Template

        IEnumerable<MessageTemplateInfoDetail> GetMessageTemplates(MessageTemplateSearchEntry entry, out int recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<MessageTemplateDetail> GetMessageTemplatesByNotificationTypeId(int notificationTypeId);
        MessageTemplateDetail GetMessageTemplateDetail(int id);
        MessageTemplateDetail GetMessageTemplateDetail(int notificationTypeId, int messageTypeId);
        SelectList PopulateMessageTemplateStatus(bool? selectedValue = null, bool? isShowSelectText = true);
        MessageTemplateDetail InsertMessageTemplate(MessageTemplateEntry entry);
        void UpdateMessageTemplate(MessageTemplateEditEntry entry);
        void UpdateMessageTemplateStatus(int id, bool status);


        #endregion

       
    }
}
