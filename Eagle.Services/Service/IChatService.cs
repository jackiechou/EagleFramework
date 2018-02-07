using System.Collections.Generic;
using Eagle.Services.Dtos.Services.Chat;

namespace Eagle.Services.Service
{
    public interface IChatService : IBaseService
    {
        #region Chat User
        IEnumerable<ChatUserDetail> GetChatUsers();
        ChatUserDetail GetChatUserDetailByEmail(string email);
        ChatUserDetail GetChatUserDetailByConnection(string connectionId);
        ChatUserDetail AddChatUser(ChatUserEntry entry);
        void RemoveChatUser(ChatUserDetail chatUser);

        #endregion

        #region Chat Message

        string AddNote(string note);
        string Broadcast(string note);
        IEnumerable<ChatMessageDetail> GetChatMessages();
        ChatMessageDetail AddMessage(ChatMessageEntry entry);
        ChatPrivateMessageMasterDetail AddChatPrivateMessageMaster(ChatPrivateMessageMasterEntry entry);
        ChatPrivateMessageDetail AddChatPrivateMessage(ChatPrivateMessageEntry entry);

        #endregion

        #region Chat Private Message Master - Details

        IEnumerable<PrivateChatMessageDetail> GetPrivateMessage(string fromEmailId, string toEmailId, int take);

        IEnumerable<PrivateChatMessageDetail> GetScrollingChatData(string fromEmailId, string toEmailId, int start = 10,
            int length = 1);
        void AddPrivateMessage(PrivateMessageEntry entry);

        #endregion
    }
}
