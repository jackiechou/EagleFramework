using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Infrastructure
{
    public interface IServiceBusPublisher : IDisposable
    {
        //#region EventsMemberGroup Methods

        //void PublishAddEventsMemberGroup(int groupId, int memberId);
        //void PublishRemoveEventsMemberGroup(int groupId, int memberId);

        //#endregion

        //#region ScheduleTask Methods

        //void PublishRetryMessage<T>(ServiceBusQueueType serviceBusQueueType, T message);

        //void PublishSendEventReminder(int eventId);

        //void PublishSendEventRsvpReminder(int eventId);

        //#endregion

        //#region TaskScheduler Methods

        //void PublishScheduleTask<T>(T scheduleTask)
        //    where T : EventMessage;

        //void PublishCreateUpdateEndedEventStatusScheduleTask(int eventId, DateTime endDate);

        //void PublishEditUpdateEndedEventStatusScheduleTask(int eventId, DateTime endDate);

        //void PublishDeleteUpdateEndedEventStatusScheduleTask(int eventId);

        ///// <summary>
        /////     Publish CreateEventReminder ScheduleTask
        ///// </summary>
        ///// <param name="cronExpression">
        /////     This is optional parameter.
        /////     format for quartz.net - "seconds minutes hours day-of-month month day-of-week year(optional)"
        ///// </param>
        //void PublishCreateEventReminderScheduleTask(string cronExpression = "0 0 14-16 * * ?");

        //void PublishDeleteEventReminderScheduleTask();

        //void PublishCreateEventRsvpReminderScheduleTask(int eventId, DateTime rsvpDate);

        //void PublishDeleteEventRsvpReminderScheduleTask(int eventId);

        //#endregion

        //#region Notification Event Methods

        //void PublishSendEventCancelNotification(int networkId, int memberId,
        //    int eventId, NotificationTarget notificationTarget);

        //void PublishSendEventInviteNotification(int networkId, int memberId,
        //    int eventId, NotificationTarget notificationTarget);

        //void PublishSendEventReminderNotification(int networkId, int memberId,
        //    int eventId, NotificationTarget notificationTarget);

        //void PublishSendEventReplyNotification(int networkId, int memberId,
        //    int eventId, NotificationTarget notificationTarget);

        //void PublishSendEventRsvpReminderNotification(int networkId, int memberId,
        //    int eventId, NotificationTarget notificationTarget);

        //void PublishSendEventUpdateNotification(int networkId, int memberId,
        //    int eventId, NotificationTarget notificationTarget);
        //void PublishSendEventConfirmNotification(int networkId, int memberId,
        //    int eventId, Sherpa.Messaging.NotificationTarget notificationTarget);
        //#endregion

        //#region NotificationEmail Methods For Event

        //void PublishSendEventCancelNotificationEmail(SendEventCancelNotificationEmail command);
        //void PublishSendEventConfirmNotificationEmail(SendEventConfirmNotificationEmail command);
        //void PublishSendEventInviteNotificationEmail(SendEventInviteNotificationEmail command);
        //void PublishSendEventReminderNotificationEmail(SendEventReminderNotificationEmail command);
        //void PublishSendEventReplyNotificationEmail(SendEventReplyNotificationEmail command);
        //void PublishSendEventRsvpReminderNotificationEmail(SendEventRsvpReminderNotificationEmail command);
        //void PublishSendEventUpdateNotificationEmail(SendEventUpdateNotificationEmail command);

        //#endregion

        //#region NotificationSms Methods For Event

        //void PublishSendEventCancelNotificationSms(SendEventCancelNotificationSms command);
        //void PublishSendEventConfirmNotificationSms(SendEventConfirmNotificationSms command);
        //void PublishSendEventInviteNotificationSms(SendEventInviteNotificationSms command);
        //void PublishSendEventReminderNotificationSms(SendEventReminderNotificationSms command);
        //void PublishSendEventReplyNotificationSms(SendEventReplyNotificationSms command);
        //void PublishSendEventRsvpReminderNotificationSms(SendEventRsvpReminderNotificationSms command);
        //void PublishSendEventUpdateNotificationSms(SendEventUpdateNotificationSms command);

        //#endregion

        //#region NotificationInbox Methods For Event

        //void PublishSendEventCancelNotificationInbox(SendEventCancelNotificationInbox command);
        //void PublishSendEventConfirmNotificationInbox(SendEventConfirmNotificationInbox command);
        //void PublishSendEventInviteNotificationInbox(SendEventInviteNotificationInbox command);
        //void PublishSendEventReminderNotificationInbox(SendEventReminderNotificationInbox command);
        //void PublishSendEventReplyNotificationInbox(SendEventReplyNotificationInbox command);
        //void PublishSendEventRsvpReminderNotificationInbox(SendEventRsvpReminderNotificationInbox command);
        //void PublishSendEventUpdateNotificationInbox(SendEventUpdateNotificationInbox command);

        //#endregion

        
    }
}
