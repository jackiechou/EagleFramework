using System;
using System.Web.Mvc;
using Eagle.Common.Extensions.EnumHelper;
using Eagle.Core.Extension;
using Eagle.Services.Service;
using Eagle.Services.Service.MessageBroadCaster;
using Eagle.WebApp.Hubs;
using Microsoft.AspNet.SignalR;

namespace Eagle.WebApp.Controllers
{
    public class MessageBroadCastController : BasicController
    {
        private IHubContext HubContext { get; set; }
        private IBroadCastService BroadCastService { get; set; }
        private IChatService ChatService { get; set; }

        public MessageBroadCastController(IBroadCastService broadCastService, IChatService chatService)
        {
           // HubContext=GlobalHost.ConnectionManager.GetHubContext<ChatHub>()
            BroadCastService = broadCastService;
            ChatService = chatService;
            HubContext = GlobalHost.ConnectionManager.GetHubContext<BroadCastHub>();

        }

        /// <summary>
        /// BroadCast message with default EventName(onMessageListened)
        /// </summary>
        /// <param name="message">Message value</param>
        /// <returns>string message</returns>
        [HttpPost]
        public string BroadCast(string message)
        {
            return ProcessMessageRequest(message, EventNameEnum.ON_MESSAGE_LISTENED.EnumDescription());
        }

        /// <summary>
        /// BroadCast message
        /// </summary>
        /// <param name="message">Message value</param>
        /// <param name="eventName">EventName value</param>
        /// <returns>string message</returns>
        [HttpPost]
        public string BroadCast(string message, string eventName)
        {
            return ProcessMessageRequest(message, eventName);
        }

        /// <summary>
        /// BroadCast message
        /// </summary>
        /// <param name="messageRequest">MessageRequested value</param>
        /// <returns>string message</returns>
        [HttpPost]
        public string BroadCast(MessageRequest messageRequest)
        {
            string response;
            try
            {
                BroadCastService.BroadCast(messageRequest);
                response = "Message successfully broadcasted !";
            }
            catch (Exception exception)
            {
                response = "Opps got error. ";
                response = string.Concat(response, "Excepion, Message : ", exception.Message);
            }
            return response;
        }

        //// GET: Chat
        //public ActionResult SendMessage(string name, string message)
        //{
        //    HubContext.Clients.All
        //    return View();
        //}

        #region Private Methods 

        /// <summary>
        /// Process message request
        /// </summary>
        /// <param name="message">Message value</param>
        /// <param name="eventName">EventName value</param>
        /// <returns>string message</returns>
        private string ProcessMessageRequest(string message, string eventName)
        {
            string response = string.Empty;
            try
            {
                MessageRequest messageRequest = new MessageRequest()
                {
                    Message = message,
                    EventName = eventName.FindEnumFromDescription<EventNameEnum>()
                };
                BroadCastService.BroadCast(messageRequest);
                response = "Message successfully broadcasted !";
            }
            catch (Exception exception)
            {
                response = "Opps got error. ";
                response = string.Concat(response, "Excepion, Message : ", exception.Message);
            }
            return response;
        }

        #endregion

        #region Dispose

        private bool _disposed;

        [NonAction]
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    HubContext = null;
                    BroadCastService = null;
                    ChatService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}