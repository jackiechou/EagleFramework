using Eagle.Entities;
using Eagle.Entities.SystemManagement;

namespace Eagle.Services.Dtos.Services.Message
{
    public class NotificationFailedMember : EntityBase
    {
        public int Id { get; set; }
        public int NotificationMessageId { get; set; }
        public int MemberId { get; set; }
        public NotificationMessageDetail NotificationMessage { get; set; }
        public User Member { get; set; }
    }
}
