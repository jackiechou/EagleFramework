using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eagle.Entities.Services.Messaging;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Services.Messaging
{
    public class NotificationMessageTypeRepository : RepositoryBase<NotificationMessageType>, INotificationMessageTypeRepository
    {
        public NotificationMessageTypeRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<NotificationMessageType> GetNotificationMessageTypes(int notificationTypeId)
        {
            return (from x in DataContext.Get<NotificationMessageType>()
                    where x.NotificationTypeId == notificationTypeId
                    select x).AsEnumerable();
        }

        public IEnumerable<NotificationMessageTypeInfo> GetNotificationMessageTypeList(int notificationTypeId)
        {
            return (from x in DataContext.Get<NotificationMessageType>()
                    join y in DataContext.Get<MessageType>() on x.MessageTypeId equals y.MessageTypeId into xycJoin
                    from type in xycJoin.DefaultIfEmpty()
                    join z in DataContext.Get<NotificationType>() on x.NotificationTypeId equals z.NotificationTypeId into xzJoin
                    from notificationType in xzJoin.DefaultIfEmpty()
                    where x.NotificationTypeId == notificationTypeId
                    select new NotificationMessageTypeInfo
                    {
                        NotificationMessageTypeId = x.NotificationMessageTypeId,
                        NotificationTypeId = x.NotificationTypeId,
                        MessageTypeId = x.MessageTypeId,
                        MessageType = type,
                        NotificationType = notificationType
                    }).AsEnumerable();
        }

        public NotificationMessageType GetDetails(int notificationTypeId, int messageTypeId)
        {
            return (from x in DataContext.Get<NotificationMessageType>()
                    where x.NotificationTypeId == notificationTypeId && x.MessageTypeId == messageTypeId
                    select x).FirstOrDefault();
        }

    }
}
