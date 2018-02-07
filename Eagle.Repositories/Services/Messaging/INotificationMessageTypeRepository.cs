using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eagle.Entities.Services.Messaging;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Services.Messaging
{
    public interface INotificationMessageTypeRepository : IRepositoryBase<NotificationMessageType>
    {
        IEnumerable<NotificationMessageType> GetNotificationMessageTypes(int notificationTypeId);
        IEnumerable<NotificationMessageTypeInfo> GetNotificationMessageTypeList(int notificationTypeId);
        NotificationMessageType GetDetails(int notificationTypeId, int messageTypeId);
    }
}
