using System.Collections.Generic;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Services.Messaging;

namespace Eagle.Services.Dtos.Services.Message
{
   public  class MessageDetail : MessageInfoBase
    {
        //user or system message
        public MessageTypeSetting MessageTypeId { get; set; }

        //only for system message template
        public NotificationTypeSetting NotificationTypeId { get; set; }

        //only for user message template
        public int? TemplateId { get; set; }

        public string WebsiteUrl { get; set; }
        public string WebsiteUrlBase { get; set; }

        //message content
        public List<SerializableKeyValuePair<string, string>> ExtraInfo { get; set; }

        //version
        public string Version { get; set; }

        public bool? IgnorePrefernceCheck { get; set; }

        public string ExceptionMessage { get; set; }
    }
}
