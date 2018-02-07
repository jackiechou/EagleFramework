using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eagle.Entities.Services.Chat;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Services.Chat
{
    public class ChatPrivateMessageRepository: RepositoryBase<ChatPrivateMessage>, IChatPrivateMessageRepository
    {
        public ChatPrivateMessageRepository(IDataContext dataContext) : base(dataContext) { }

    }
}
