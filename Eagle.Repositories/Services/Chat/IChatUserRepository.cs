using System.Collections.Generic;
using Eagle.Entities.Services.Chat;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Services.Chat
{
    public interface IChatUserRepository: IRepositoryBase<ChatUser>
    {
        IEnumerable<ChatUser> GetChatUsers();
        ChatUser FindByConnection(string connectionId);
        ChatUser FindByEmail(string email);
        bool HasUserExistedInChatList(string email);
    }
}
