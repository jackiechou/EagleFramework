using System;

namespace Eagle.Services.Dtos.Services.Message
{
   public class NotificationMemberPreference:DtoBase
    {
        public Guid? MemberId { get; set; }
        public Guid? NotificationTypeId { get; set; }
        public Guid? ChannelPreference { get; set; }
    }
}
