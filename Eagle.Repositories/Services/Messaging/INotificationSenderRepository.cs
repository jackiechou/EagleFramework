using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.Services.Messaging;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Services.Messaging
{
    public interface INotificationSenderRepository : IRepositoryBase<NotificationSender>
    {
        IEnumerable<NotificationSender> GetNotificationSenders();
        IEnumerable<NotificationSenderInfo> GetNotificationSenders(int? mailServerProviderId);
        NotificationSenderInfo GetDefaultNotificationSender();
        NotificationSender FindByEmailAddress(string mailAddress);
        NotificationSenderInfo GetDetails(int senderId);
        SelectList PopulateNotificationSenderSelectList(int? mailServerProviderId, int? selectedValue = null,
            bool? isShowSelectText = true);
        bool HasSenderNameExisted(string senderName);
        bool HasMailAddressExisted(string mailAddress);
    }
}
