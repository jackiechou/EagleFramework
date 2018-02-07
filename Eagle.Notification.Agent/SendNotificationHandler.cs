using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eagle.Infrastructure;
using Eagle.Infrastructure.Messaging;
using Eagle.Services.Dtos.Services.Message;
using Eagle.Services.Messaging;
using Eagle.Services.SystemManagement;

namespace Eagle.Notification.Agent
{
    public abstract class SendNotificationHandler<T> : CommandHandler<T>
        where T : SendNotification
    {
        private IEnumerable<NotificationMemberPreference> _noticationMemberPreferences;
        private readonly IUserService _userService;
        protected readonly INotificationService NotificationService;
        protected IServiceBusPublisher ServiceBusPublisher {
            get { return (IServiceBusPublisher)ServiceBusPublisherBase; }
        }

        protected SendNotificationHandler(IServiceBusPublisher serviceBusPublisher,
            INotificationService notificationService, IUserService userService)
            : base((IServiceBusPublisherBase)serviceBusPublisher)
        {
            NotificationService = notificationService;
            _userService = userService;
        }

        public sealed override void Handle(T command)
        {
            //try
            //{
                //Get list of member and member's preference
                var targetMemberIds = new List<Guid>();
                var targetMemberContactDetails = _userService.GetProfiles(targetMemberIds).ToList();

                if (command.IgnoreMemberPreference)
                {
                    //Send all notification (Email, Sms, Inbox)
                }
                //else
                //{
                //    //_noticationMemberPreferences = NotificationService.GetMemberPreferenceList(targetMemberIds,
                //    //    (int) command.NotificationType);
                //    //var notificationMemberPreferences =
                //    //    _noticationMemberPreferences as IList<NotificationMemberPreference> ??
                //    //    _noticationMemberPreferences.ToList();

                //    //foreach (var notificationMemberPreference in notificationMemberPreferences)
                //    //{
                //    //    var targetMemberContactDetail =
                //    //        targetMemberContactDetails.First(
                //    //            m => m.Id == notificationMemberPreference.MemberId);
                //    //    if (notificationMemberPreference.ChannelPreference.HasValue)
                //    //    {
                //    //        command.TargetMemberId = targetMemberContactDetail.UserId;
                //    //        ////Publish Email
                //    //        //if ((int) NotificationChannel.Email) > 0)
                //    //        //{
                //    //        //    PublishNotificationEmail(command);
                //    //        //}
                //    //        ////Publish Sms
                //    //        //if ((notificationMemberPreference.ChannelPreference.Value &
                //    //        //     (int) NotificationChannel.Sms) > 0)
                //    //        //{
                //    //        //    PublishNotificationSms(command);
                //    //        //}
                //    //        ////Publish Inbox
                //    //        //if ((notificationMemberPreference.ChannelPreference.Value &
                //    //        //     (int) NotificationChannel.Inbox) > 0)
                //    //        //{
                //    //        //    PublishNotificationInbox(command);
                //    //        //}
                //    //    }
                //    //}
                //}
            //}
            //catch (Exception e)
            //{
            //    Retry(ServiceBusQueueType.NotificationPublisher, command);
            //    Trace.TraceInformation("SendNotification Retry Sent!");

            //}
            Trace.TraceInformation("SendNotification has been handled");
        }

        protected abstract void PublishNotificationEmail(SendNotification command);

        protected abstract void PublishNotificationSms(SendNotification command);

        protected abstract void PublishNotificationInbox(SendNotification command);
    }
}
