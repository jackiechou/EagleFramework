using System.Collections.Generic;
using Eagle.Entities.Services.Chat;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Services.Chat
{
    public interface IChatMessageRepository: IRepositoryBase<ChatMessage>
    {
        IEnumerable<ChatMessage> GetChatMessages();
    }
}
