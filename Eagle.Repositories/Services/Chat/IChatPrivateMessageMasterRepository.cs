using System.Collections.Generic;
using Eagle.Entities.Services.Chat;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Services.Chat
{
    public interface IChatPrivateMessageMasterRepository : IRepositoryBase<ChatPrivateMessageMaster>
    {
        IEnumerable<ChatPrivateMessageMaster> GetChatPrivateMessageMasters();
        List<PrivateChatMessage> GetPrivateMessage(string fromEmailId, string toEmailId, int take);
        List<PrivateChatMessage> GetScrollingChatData(string fromEmailId, string toEmailId, int start = 10, int length = 1);
        bool HasEmailExistedInPrivateMessageMaster(string fromEmail);
    }
}
