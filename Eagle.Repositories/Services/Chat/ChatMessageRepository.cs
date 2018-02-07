using System.Collections.Generic;
using System.Linq;
using Eagle.Entities.Services.Chat;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Services.Chat
{
    public class ChatMessageRepository: RepositoryBase<ChatMessage>, IChatMessageRepository
    {
        public ChatMessageRepository(IDataContext dataContext) : base(dataContext)
        {
        }
        public IEnumerable<ChatMessage> GetChatMessages()
        {
            return (from x in DataContext.Get<ChatMessage>()
                    select x).AsEnumerable();
        }
        
    }
}
