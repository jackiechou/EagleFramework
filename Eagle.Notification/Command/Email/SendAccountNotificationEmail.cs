using System;

namespace Eagle.Notification.Command.Email
{
    [Serializable]
    public class SendAccountNotificationEmail : SendNotificationEmail
    {
        public int ServiceRequestId { get; set; }
    }

    [Serializable]
    public class SendServiceRequestCreateNotificationEmail : SendAccountNotificationEmail
    {

    }
}
