using System.Collections.Generic;
using System.Linq;
using Eagle.Entities.Services.Chat;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Services.Chat
{
    public class ChatUserRepository: RepositoryBase<ChatUser>, IChatUserRepository
    {
        public ChatUserRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<ChatUser> GetChatUsers()
        {
            return (from x in DataContext.Get<ChatUser>()
                    select x).AsEnumerable();
        }
        public ChatUser FindByConnection(string connectionId)
        {
            return (from x in DataContext.Get<ChatUser>()
                    where x.ConnectionId.ToLower() == connectionId.ToLower()
                    select x).FirstOrDefault();
        }
        public ChatUser FindByEmail(string email)
        {
            return (from x in DataContext.Get<ChatUser>()
                    where x.EmailId.ToLower() == email.ToLower()
                    select x).FirstOrDefault();
        }

        public bool HasUserExistedInChatList(string email)
        {
            var query = (from x in DataContext.Get<ChatUser>()
                    where x.EmailId.ToLower() == email.ToLower()
                    select x).FirstOrDefault();

            return query != null;
        }
    }
}
