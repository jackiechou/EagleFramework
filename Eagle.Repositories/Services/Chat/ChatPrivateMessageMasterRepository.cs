using System.Collections.Generic;
using System.Linq;
using Eagle.Entities.Services.Chat;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Services.Chat
{
    public class ChatPrivateMessageMasterRepository : RepositoryBase<ChatPrivateMessageMaster>,
        IChatPrivateMessageMasterRepository
    {
        public ChatPrivateMessageMasterRepository(IDataContext dataContext) : base(dataContext)
        {
        }
        public IEnumerable<ChatPrivateMessageMaster> GetChatPrivateMessageMasters()
        {
            return (from x in DataContext.Get<ChatPrivateMessageMaster>()
                    select x).AsEnumerable();
        }
        public List<PrivateChatMessage> GetPrivateMessage(string fromEmailId, string toEmailId, int take)
        {
            var v = (from a in DataContext.Get<ChatPrivateMessageMaster>()
                join b in DataContext.Get<ChatPrivateMessage>() on a.EmailId equals b.MasterEmailId into cc
                from c in cc
                where (c.MasterEmailId.Equals(fromEmailId) && c.ChatToEmailId.Equals(toEmailId)) || (c.MasterEmailId.Equals(toEmailId)
                                                                                           &&
                                                                                           c.ChatToEmailId.Equals(fromEmailId))
                orderby c.Id descending
                select new PrivateChatMessage
                {
                    Id = c.Id,
                    UserName = a.UserName,
                    Message = c.Message
                }).Take(take).ToList();
            v = v.OrderBy(s => s.Id).ToList();

            return v.Select(a => new PrivateChatMessage()
            {
                UserName = a.UserName,
                Message = a.Message
            }).ToList();
        }

        public List<PrivateChatMessage> GetScrollingChatData(string fromEmailId, string toEmailId, int start = 10, int length = 1)
        {
            int takeCounter = (length * start); // 20
            int skipCounter = ((length - 1) * start); // 10

            var v = (from a in DataContext.Get<ChatPrivateMessageMaster>()
                      join b in DataContext.Get<ChatPrivateMessage>() on a.EmailId equals b.MasterEmailId into cc
                      from c in cc
                     where (c.MasterEmailId.Equals(fromEmailId) && c.ChatToEmailId.Equals(toEmailId)) || (c.MasterEmailId.Equals(toEmailId)
                     && c.ChatToEmailId.Equals(fromEmailId))
                     orderby c.Id descending
                     select new PrivateChatMessage
                     {
                         Id = c.Id,
                         UserName = a.UserName,
                         Message = c.Message
                     }).Take(takeCounter).Skip(skipCounter).ToList();

            return v.Select(a => new PrivateChatMessage()
            {
                UserName = a.UserName,
                Message = a.Message
            }).ToList();
        }

        public bool HasEmailExistedInPrivateMessageMaster(string fromEmail)
        {
            var query = (from x in DataContext.Get<ChatPrivateMessageMaster>()
                         where x.EmailId.ToLower() == fromEmail.ToLower()
                         select x).FirstOrDefault();

            return query != null;
        }
    }
}
