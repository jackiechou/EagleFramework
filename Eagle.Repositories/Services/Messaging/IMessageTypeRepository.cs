using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.Services.Messaging;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Services.Messaging
{
    public interface IMessageTypeRepository: IRepositoryBase<MessageType>
    {
        IEnumerable<MessageType> GetMessageTypes(bool? status);
        MultiSelectList PopulateMessageTypeMultiSelectList(bool? status = null, int[] selectedValues = null, bool? isShowSelectText = true);
        SelectList PopulateMessageTypeSelectList(int? selectedValue = null, bool? isShowSelectText = true);
        SelectList PopulateMessageTypeStatus(bool? selectedValue=null, bool? isShowSelectText = true);
        bool HasDataExists(string messageTypeName);
    }
}
