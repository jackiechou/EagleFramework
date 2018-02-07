using AutoMapper;
using Eagle.Notification.Command.Email;
using Eagle.Services.Dtos.Services.Message;

namespace Eagle.Notification
{
    public class NotificationMapping
    {
        public static void ConfigureMapping()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<SendServiceRequestCreateNotification, SendServiceRequestCreateNotificationEmail>();
                cfg.CreateMap<SendNotificationEmail, NotificationLog<SendServiceRequestCreateNotificationEmail>>();

                cfg.CreateMap<NotificationTarget, NotificationTargetModel>();
            });
        }
    }
}
