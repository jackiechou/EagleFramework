using System;
using System.Collections.Generic;
using Eagle.Core.Common;

namespace Eagle.Entities.Services.Messaging
{
    [Serializable]
    public class MessageInfo : MessageInfoBase
    {
        //user or system message
        public int MessageTemplateType { get; set; }

        //only for user message template
        public int? TemplateId { get; set; }

        //only for system message template
        public int NotificationTypeId { get; set; }

        public Guid UserId { get; set; }

        public string NetworkDescription { get; set; }
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
