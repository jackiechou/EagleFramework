using System;

namespace Eagle.Notification.Command.Email
{
    public class SendAccountNotification : SendNotification
    {
        public int ServiceRequestId { get; set; }
        public int MembeId { get; set; }
    }

    [Serializable]
    public class SendServiceRequestCreateNotification : SendAccountNotification
    {
        public SendServiceRequestCreateNotification()
        {
            NotificationType = NotificationType.ACCOUNT_CREATED;
        }
    }
}
