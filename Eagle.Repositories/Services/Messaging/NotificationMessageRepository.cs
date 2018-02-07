using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Eagle.Entities.Services.Messaging;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Services.Messaging
{
    public class NotificationMessageRepository : RepositoryBase<NotificationMessage>, INotificationMessageRepository
    {
        public NotificationMessageRepository(IDataContext context) : base(context)
        {
        }

        public IEnumerable<NotificationMessage> GetReadyToSendNotificationMessages()
        {
            var result = from t in DataContext.Get<NotificationMessage>()
                         where (t.SentStatus == NotificationSentStatus.Pending || t.SentStatus == NotificationSentStatus.Retry) &&
                         t.PublishDate <= DateTime.UtcNow
                         select t;

            return result.AsEnumerable();
        }
    }
}
