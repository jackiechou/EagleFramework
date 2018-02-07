using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eagle.Services.Dtos.Services.Chat;
using Eagle.Services.Service;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Hubs;

namespace Eagle.WebApp.Hubs
{
    [HubName("ChatHub")]
    public class ChatHub : Hub
    {
        #region Data Members
        public static string EmailIdLoaded = "";
        static readonly List<ChatUserDetail> ConnectedUsers = new List<ChatUserDetail>();
        static readonly List<ChatMessageDetail> CurrentMessage = new List<ChatMessageDetail>();

        #endregion
        private IChatService ChatService { get; set; }
        private IHubContext HubContext { get; set; }
        private HubConnection HubConnection { get; set; }
        private IHubProxy HubProxy { get; set; }
        public event EventHandler<dynamic> OnDataReceived;
        private bool _hasSubscribers = false;

        public ChatHub()
        {
            HubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
        }
        public ChatHub(IChatService chatService)
        {
            ChatService = chatService;
            HubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            //var uriBuilder = new UriBuilder(HttpContext.Current.Request.Url.Scheme,
            //        HttpContext.Current.Request.Url.Host,
            //        HttpContext.Current.Request.Url.Port);
            //var hubUrl = uriBuilder.Query + "/signalr";
            //HubConnection = new HubConnection(hubUrl, false) { Credentials = CredentialCache.DefaultCredentials };
            //HubProxy = HubConnection.CreateHubProxy("ChatHub");
        }

        #region Methods

        public async Task Login(string username)
        {
           // await HubConnection.Start(new WebSocketTransport());
            //HubProxy.On("userLoggedIn", data =>
            //{
            //    OnDataReceived?.Invoke(this, data);
            //});

            // Notify all clients that a user is logged in
            await HubContext.Clients.All.userLoggedIn(username);
        }

        public async Task Send(string name, string message)
        {
            //Connect();
            //var messageCotent = new ChatMessageEntry { UserName = name, Message = message };
            //HubConnection.Send(messageCotent);
            //HubProxy.On<string>("addMessage", (data) => HubConnection.TraceWriter.WriteLine(data));
            //HubProxy.Invoke("Send", name, message);  //call from hub to server
            //await HubConnection.Start(new WebSocketTransport());
            //await hubConnection.Start(new LongPollingTransport());
            //ServicePointManager.DefaultConnectionLimit = ServicePointManager.DefaultPersistentConnectionLimit;

            await HubContext.Clients.All.sendMessage(name, message);
            //hubProxy.On<Stock>("addMessage", stock => stock => Console.WriteLine("Stock update for {0} new price {1}", stock.Symbol, stock.Price));
            //hubProxy.On("addMessage", (msg) => hubProxy.Invoke("Send", new { name, message }));  //call from hub to client
            //HubProxy.Invoke("Send", name, message);  //call from hub to server
        }

        ////public void Connect(string userName)
        ////{
        ////    var id = Context.ConnectionId;

        ////    if (ConnectedUsers.Count(x => x.ConnectionId == id) == 0)
        ////    {
        ////        ConnectedUsers.Add(new ChatUserDetail { ConnectionId = id, UserName = userName });

        ////        // send to caller
        ////        Clients.Caller.onConnected(id, userName, ConnectedUsers, CurrentMessage);

        ////        // send to all except caller client
        ////        Clients.AllExcept(id).onNewUserConnected(id, userName);
        ////    }
        ////}
        public void Connect(string userName, string email)
        {
            EmailIdLoaded = email;
            var id = Context.ConnectionId;
            //var item = ChatService.GetChatUserDetailByEmail(email);
            //if (item != null)
            //{
            //    ChatService.RemoveChatUser(item);

            //    // Disconnect
            //    Clients.All.onUserDisconnectedExisting(item.ConnectionId, item.UserName);
            //}

            //var users = ChatService.GetChatUsers().ToList();
            //if (users.Where(x => x.Email == email).ToList().Count == 0)
            //{
            //    var chatUserEntry = new ChatUserEntry
            //    {
            //        ConnectionId = id,
            //        UserName = userName,
            //        EmailId = email
            //    };
            //    ChatService.AddChatUser(chatUserEntry);

            //    // send to caller
            //    var connectedUsers = users.ToList();
            //    var currentMessage = ChatService.GetChatMessages().ToList();
            //    Clients.Caller.onConnected(id, userName, connectedUsers, currentMessage);
            //}

            // send to all except caller client
            Clients.AllExcept(id).onNewUserConnected(id, userName, email);
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            //Remove user out of datababse
            var user = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (user != null)
            {
                ChatService.RemoveChatUser(user);
            }

            var id = Context.ConnectionId;
            //Remove user out of cache
            var item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                ConnectedUsers.Remove(item);
                Clients.All.onUserDisconnected(id, item.UserName);
            }
            return base.OnDisconnected(stopCalled);
        }
        public void SendMessageToAll(string userName, string message)
        {
            // store last 100 messages in cache
            AddMessageInCache(userName, message);

            // Broad cast message
            Clients.All.messageReceived(userName, message);
        }
        public void SendPrivateMessage(string toUserId, string message)
        {
            string fromUserId = Context.ConnectionId;

            var toUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == toUserId);
            var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == fromUserId);

            if (toUser != null && fromUser != null)
            {
                // send to 
                Clients.Client(toUserId).sendPrivateMessage(fromUserId, fromUser.UserName, message);

                // send to caller user
                Clients.Caller.sendPrivateMessage(toUserId, fromUser.UserName, message);
            }
        }
        public void SendPrivateMessage(string toUserId, string message, string status)
        {
            string fromUserId = Context.ConnectionId;
            var toUser = ChatService.GetChatUserDetailByConnection(toUserId);
            var fromUser = ChatService.GetChatUserDetailByConnection(fromUserId);
            if (toUser != null && fromUser != null)
            {
                if (status == "Click")
                {
                    var privateMessage = new PrivateMessageEntry
                    {
                        FromEmail = fromUser.Email,
                        ToEmail = toUser.Email,
                        UserName = fromUser.UserName,
                        Message = message
                    };
                    AddPrivateMessageInCache(fromUser.Email, toUser.Email, fromUser.UserName, message);
                    ChatService.AddPrivateMessage(privateMessage);
                }

                // send to 
                Clients.Client(toUserId).sendPrivateMessage(fromUserId, fromUser.UserName, message, fromUser.Email, toUser.Email, status, fromUserId);

                // send to caller user
                Clients.Caller.sendPrivateMessage(toUserId, fromUser.UserName, message, fromUser.Email, toUser.Email, status, fromUserId);
            }
        }

        //public IEnumerable<PrivateChatMessageDetail> GetPrivateMessage(string fromEmailId, string toEmailId, int take)
        //{
        //    return ChatService.GetPrivateMessage(fromEmailId, toEmailId, take);
        //}

        //public IEnumerable<PrivateChatMessageDetail> GetScrollingChatData(string fromEmailId, string toEmailId, int start = 10, int length = 1)
        //{
        //    return ChatService.GetScrollingChatData(fromEmailId, toEmailId, start, length);
        //}
        #endregion

        //public override Task OnConnected()
        //{
        //    // Add your own code here.
        //    // For example: in a chat application, record the association between
        //    // the current connection ID and user name, and mark the user as online.
        //    // After the code in this method completes, the client is informed that
        //    // the connection is established; for example, in a JavaScript client,
        //    // the start().done callback is executed.
        //    return base.OnConnected();
        //}

        //public override Task OnDisconnected()
        //{
        //    // Add your own code here.
        //    // For example: in a chat application, mark the user as offline, 
        //    // delete the association between the current connection id and user name.
        //    return base.OnDisconnected();
        //}

        //public override Task OnReconnected()
        //{
        //    // Add your own code here.
        //    // For example: in a chat application, you might have marked the
        //    // user as offline after a period of inactivity; in that case 
        //    // mark the user as online again.
        //    return base.OnReconnected();
        //}

        #region private Messages

        private void AddMessageInCache(string userName, string message)
        {
            CurrentMessage.Add(new ChatMessageDetail { UserName = userName, Message = message });

            if (CurrentMessage.Count > 100)
                CurrentMessage.RemoveAt(0);

            //Save message to message table
            var chatMessage = new ChatMessageEntry
            {
                UserName = userName,
                Message = message,
                Email = EmailIdLoaded
            };
            ChatService.AddMessage(chatMessage);
        }
        private void AddPrivateMessageInCache(string fromEmail, string chatToEmail, string userName, string message)
        {
            // Save master
            var chatPrivateMessageMasterEntry = new ChatPrivateMessageMasterEntry
            {
                UserName = userName,
                Email = EmailIdLoaded
            };
            ChatService.AddChatPrivateMessageMaster(chatPrivateMessageMasterEntry);

            // Save details
            var chatPrivateMessageEntry = new ChatPrivateMessageEntry
            {
                MasterEmailId = fromEmail,
                ChatToEmailId = chatToEmail,
                Message = message
            };
            ChatService.AddChatPrivateMessage(chatPrivateMessageEntry);
        }
        #endregion
    }
}
