using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Entities.Services.Messaging;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Services.Messaging
{
    public class MessageQueueRepository : RepositoryBase<MessageQueue>, IMessageQueueRepository
    {
        public MessageQueueRepository(IDataContext context) : base(context){}
        public IEnumerable<MessageQueue> GetList(ref int? recordCount, int? page = null, int? pageSize = null)
        {
            var queryable = DataContext.Get<MessageQueue>();

            if (recordCount != null)
            {
                recordCount = queryable.Count();
            }

            queryable = queryable.OrderBy(m => m.QueueId);

            if (page != null && pageSize != null)
            {
                queryable = queryable.ApplyPaging(page.Value, pageSize.Value);
            }

            return queryable.AsEnumerable();
        }

        public IEnumerable<MessageQueue> GetListBySender(string fromSender, ref int? recordCount, int? page = null, int? pageSize = null)
        {
            var queryable = DataContext.Get<MessageQueue>().Where(x => x.From == fromSender);
            if (recordCount != null)
            {
                recordCount = queryable.Count();
            }

            queryable = queryable.OrderBy(m => m.QueueId);

            if (page != null && pageSize != null)
            {
                queryable = queryable.ApplyPaging(page.Value, pageSize.Value);
            }

            return queryable.AsEnumerable();
        }
        
        public IEnumerable<MessageQueue> GetEmailMessagesInQueue()
        {
            var query = (from x in DataContext.Get<MessageQueue>()
                         //where y.Type == NotificationType.EMAIL && z.Status == TransmitStatus.NEW
                         select x);

            return query;
        }
    }
}
