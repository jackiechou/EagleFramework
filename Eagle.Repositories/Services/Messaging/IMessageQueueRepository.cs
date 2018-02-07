using System;
using System.Collections.Generic;
using Eagle.Entities.Services.Messaging;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Services.Messaging
{
    public interface IMessageQueueRepository : IRepositoryBase<MessageQueue>
    {
        IEnumerable<MessageQueue> GetList(ref int? recordCount, int? page = null, int? pageSize = null);
        IEnumerable<MessageQueue> GetListBySender(string fromSender, ref int? recordCount, int? page = null, int? pageSize = null);
        IEnumerable<MessageQueue> GetEmailMessagesInQueue();        
    }
}
