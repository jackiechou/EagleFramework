using System;
using System.Collections.Generic;
using Eagle.Entities.Services.Messaging;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Services.Messaging
{
    public interface INotificationMessageRepository : IRepositoryBase<NotificationMessage>
    {
        IEnumerable<NotificationMessage> GetReadyToSendNotificationMessages();

       

    }
}
