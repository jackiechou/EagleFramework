using System.Collections.Generic;
using Eagle.Entities.Services.Chat;
using Eagle.Repositories;
using Eagle.Services.Dtos.Services.Chat;
using Eagle.Services.EntityMapping.Common;

namespace Eagle.Services.Service
{
    public class ChatService : BaseService, IChatService
    {
        public ChatService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region Chat User
        public IEnumerable<ChatUserDetail> GetChatUsers()
        {
            var item = UnitOfWork.ChatUserRepository.GetChatUsers();
            return item.ToDtos<ChatUser, ChatUserDetail>();
        }

        public ChatUserDetail GetChatUserDetailByEmail(string email)
        {
            var item = UnitOfWork.ChatUserRepository.FindByEmail(email);
            return item.ToDto<ChatUser, ChatUserDetail>();
        }

        public ChatUserDetail GetChatUserDetailByConnection(string connectionId)
        {
            var item = UnitOfWork.ChatUserRepository.FindByConnection(connectionId);
            return item.ToDto<ChatUser, ChatUserDetail>();
        }

        public ChatUserDetail AddChatUser(ChatUserEntry entry)
        {
            var hasUserExistedInChatList = UnitOfWork.ChatUserRepository.HasUserExistedInChatList(entry.EmailId);
            if (hasUserExistedInChatList) return null;

            var entity = entry.ToEntity<ChatUserEntry, ChatUser>();
            UnitOfWork.ChatUserRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<ChatUser, ChatUserDetail>();
        }

        public void RemoveChatUser(ChatUserDetail chatUser)
        {
            var entity = UnitOfWork.ChatUserRepository.FindById(chatUser.Id);
            if (entity != null)
            {
                UnitOfWork.ChatUserRepository.Delete(entity);
            }
        }
        #endregion

        #region Chat Message
        public string AddNote(string note)
        {
            return note;
        }
        public string Broadcast(string note)
        {
            return note;
        }
        public IEnumerable<ChatMessageDetail> GetChatMessages()
        {
            var item = UnitOfWork.ChatMessageRepository.GetChatMessages();
            return item.ToDtos<ChatMessage, ChatMessageDetail>();
        }
       
        public ChatMessageDetail AddMessage(ChatMessageEntry entry)
        {
            var entity = entry.ToEntity<ChatMessageEntry, ChatMessage>();
            UnitOfWork.ChatMessageRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<ChatMessage, ChatMessageDetail>();
        }
        #endregion
       
        #region Chat Private Message Master - Details

        public IEnumerable<PrivateChatMessageDetail> GetPrivateMessage(string fromEmailId, string toEmailId, int take)
        {
            var item = UnitOfWork.ChatPrivateMessageMasterRepository.GetPrivateMessage(fromEmailId, fromEmailId, take);
            return item.ToDtos<PrivateChatMessage, PrivateChatMessageDetail>();
        }

        public IEnumerable<PrivateChatMessageDetail> GetScrollingChatData(string fromEmailId, string toEmailId, int start = 10, int length = 1)
        {
            var item = UnitOfWork.ChatPrivateMessageMasterRepository.GetScrollingChatData(fromEmailId, fromEmailId, start, length);
            return item.ToDtos<PrivateChatMessage, PrivateChatMessageDetail>();
        }

        public void AddPrivateMessage(PrivateMessageEntry entry)
        {
            // Save  message master
            var messageMater = new ChatPrivateMessageMasterEntry
            {
                Email = entry.FromEmail,
                UserName = entry.UserName
            };

            AddChatPrivateMessageMaster(messageMater);

            // Save message details
            var messageDetail = new ChatPrivateMessageEntry
            {
                MasterEmailId = entry.FromEmail,
                ChatToEmailId = entry.ToEmail,
                Message = entry.Message
            };
            AddChatPrivateMessage(messageDetail);
        }
        public ChatPrivateMessageMasterDetail AddChatPrivateMessageMaster(ChatPrivateMessageMasterEntry entry)
        {
            var hasEmailExisted =
                UnitOfWork.ChatPrivateMessageMasterRepository.HasEmailExistedInPrivateMessageMaster(entry.Email);

            var entity = entry.ToEntity<ChatPrivateMessageMasterEntry, ChatPrivateMessageMaster>();
            UnitOfWork.ChatPrivateMessageMasterRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<ChatPrivateMessageMaster, ChatPrivateMessageMasterDetail>();
        }

        public ChatPrivateMessageDetail AddChatPrivateMessage(ChatPrivateMessageEntry entry)
        {
            var entity = entry.ToEntity<ChatPrivateMessageEntry, ChatPrivateMessage>();
            UnitOfWork.ChatPrivateMessageRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<ChatPrivateMessage, ChatPrivateMessageDetail>();
        }

      
        #endregion
    }
}
