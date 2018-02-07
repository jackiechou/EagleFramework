using System;
using System.Web.Http;
using Eagle.Common.Extensions.EnumHelper;
using Eagle.Core.Extension;
using Eagle.Services.Service.MessageBroadCaster;

namespace Eagle.WebApi.Areas
{
    /// <summary>
    /// Message broadcaster ApiController class
    /// </summary>
    public class MessageBroadCastController : ApiController
    {
        #region Private Variable

        private IBroadCastService _broadCast;

        #endregion

        #region Constructors

        /// <summary>
        /// Message broadcaster ApiController class
        /// </summary>
        /// <param name="broadCast">IBroadCast value</param>
        public MessageBroadCastController(IBroadCastService broadCast)
        {
            _broadCast = broadCast;
        }

        #endregion

        #region Public API Methods

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
        /// <param name="messageRequest">MessageRequested value</param>
        /// <returns>string message</returns>
        [HttpPost]
        public string BroadCast(MessageRequest messageRequest)
        {
            string response = string.Empty;
            try
            {
                _broadCast.BroadCast(messageRequest);
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
                _broadCast.BroadCast(messageRequest);
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
    }
}
