using Autofac.Features.Indexed;
using Eagle.Infrastructure.Messaging;

namespace Eagle.Infrastructure
{
    public class ServiceBusPublisher : ServiceBusPublisherBase, IServiceBusPublisher
    {
        private IIndex<ServiceBusQueueType, IServiceBus> _serviceBus;
        //private INotificationLogService _notificationLogService;
        //private IScheduleTaskLogService _scheduleTaskLogService;
        //private ITaskSchedulerLogService _taskSchedulerLogService;
        //private ISendReminderLogService _sendReminderLogService;
        //private readonly IEventsMemberGroupLogService _eventsMemberGroupLogService;

        public ServiceBusPublisher(IIndex<ServiceBusQueueType, IServiceBus> serviceBus
            //,INotificationLogService notificationLogService,
            //IScheduleTaskLogService scheduleTaskLogService,
            //ITaskSchedulerLogService taskSchedulerLogService,
            //ISendReminderLogService sendReminderLogService,
            //IEventsMemberGroupLogService eventsMemberGroupLogService
            )

            : base(serviceBus)
        {
            _serviceBus = serviceBus;
            //_notificationLogService = notificationLogService;
            //_scheduleTaskLogService = scheduleTaskLogService;
            //_taskSchedulerLogService = taskSchedulerLogService;
            //_sendReminderLogService = sendReminderLogService;
            //_eventsMemberGroupLogService = eventsMemberGroupLogService;

            //Configure Automapper
            //NotificationMapping.ConfigureMapping();
            //ScheduleTaskLogMapping.ConfigureMapping();
            //TaskSchedulerLogMapping.ConfigureMapping();
            //SendReminderLogMapping.ConfigureMapping();
            //ApplicationMapping.ConfigureMapping();
        }

        //#region EventsMemberGroup Methods

        //public void PublishAddEventsMemberGroup(int groupId, int memberId)
        //{
        //    var command = new AddEventsForMemberGroup(groupId, memberId);
        //    var eventsMemberGroupLog =
        //        Mapper.Map<AddEventsForMemberGroup, EventsMemberGroupLog<AddEventsForMemberGroup>>(command);
        //    eventsMemberGroupLog.Message = command;
        //    _eventsMemberGroupLogService.InsertEventsMemberGroupLog(eventsMemberGroupLog);
        //    _serviceBus[ServiceBusQueueType.ApplicationPublisher].Publish(command);
        //}

        //public void PublishRemoveEventsMemberGroup(int groupId, int memberId)
        //{
        //    var command = new RemoveEventsForMemberGroup(groupId, memberId);
        //    var eventsMemberGroupLog =
        //        Mapper.Map<RemoveEventsForMemberGroup, EventsMemberGroupLog<RemoveEventsForMemberGroup>>(command);
        //    eventsMemberGroupLog.Message = command;
        //    _eventsMemberGroupLogService.InsertEventsMemberGroupLog(eventsMemberGroupLog);
        //    _serviceBus[ServiceBusQueueType.ApplicationPublisher].Publish(command);
        //}

        //#endregion

        //#region ScheduleTask Methods

        //public void PublishRetryMessage<T>(ServiceBusQueueType serviceBusQueueType, T message)
        //{
        //    _serviceBus[serviceBusQueueType].Publish(message, message.GetType());
        //}

        //public void PublishSendEventReminder(int eventId)
        //{
        //    PublishSendReminderLog(new SendEventReminder(eventId));
        //}

        //public void PublishSendEventRsvpReminder(int eventId)
        //{
        //    PublishSendReminderLog(new SendEventRsvpReminder(eventId));
        //}

        //#endregion

        //#region TaskScheduler Methods

        //public void PublishCreateUpdateEndedEventStatusScheduleTask(int eventId, DateTime endDate)
        //{
        //    PublishTaskSchedulerLog(new CreateUpdateEndedEventStatusScheduleTask(eventId, endDate));
        //}

        //public void PublishEditUpdateEndedEventStatusScheduleTask(int eventId, DateTime endDate)
        //{
        //    PublishTaskSchedulerLog(new EditUpdateEndedEventStatusScheduleTask(eventId, endDate));
        //}

        //public void PublishDeleteUpdateEndedEventStatusScheduleTask(int eventId)
        //{
        //    PublishTaskSchedulerLog(new DeleteUpdateEndedEventStatusScheduleTask(eventId));
        //}

        //public void PublishCreateEventReminderScheduleTask(string cronExpression = "0 0 14-16 * * ?")
        //{
        //    PublishTaskSchedulerLog(new CreateEventReminderScheduleTask(cronExpression));
        //}

        //public void PublishDeleteEventReminderScheduleTask()
        //{
        //    PublishTaskSchedulerLog(new DeleteEventReminderScheduleTask());
        //}

        //public void PublishCreateEventRsvpReminderScheduleTask(int eventId, DateTime rsvpDate)
        //{
        //    PublishTaskSchedulerLog(new CreateEventRsvpReminderScheduleTask(eventId, rsvpDate));
        //}

        //public void PublishDeleteEventRsvpReminderScheduleTask(int eventId)
        //{
        //    PublishTaskSchedulerLog(new DeleteEventRsvpReminderScheduleTask(eventId));
        //}

        //public void PublishScheduleTask<T>(T scheduleTask)
        //    where T : EventMessage
        //{
        //    PublishScheduleTaskLog(scheduleTask);
        //}

        //#endregion

        //#region Notification Event Methods

        //public void PublishSendEventCancelNotification(int networkId, int memberId,
        //    int eventId, Sherpa.Messaging.NotificationTarget notificationTarget)
        //{
        //    var notification = new SendEventCancelNotification
        //    {
        //        NetworkId = networkId,
        //        MemberId = memberId,
        //        EventId = eventId,
        //        MessageTemplateType = MessageType.System,
        //        NotificationTarget = notificationTarget,
        //        IgnoreMemberPreference = false
        //    };

        //    PublishTargetNotificationLog(notification,
        //        Mapper.Map<SendEventCancelNotification, TargetNotificationLog<SendEventCancelNotification>>(notification));
        //}

        //public void PublishSendEventInviteNotification(int networkId, int memberId,
        //    int eventId, Sherpa.Messaging.NotificationTarget notificationTarget)
        //{
        //    var notification = new SendEventInviteNotification
        //    {
        //        NetworkId = networkId,
        //        MemberId = memberId,
        //        EventId = eventId,
        //        MessageTemplateType = MessageType.System,
        //        NotificationTarget = notificationTarget,
        //        IgnoreMemberPreference = false
        //    };

        //    PublishTargetNotificationLog(notification,
        //        Mapper.Map<SendEventInviteNotification, TargetNotificationLog<SendEventInviteNotification>>(notification));
        //}

        //public void PublishSendEventUpdateNotification(int networkId, int memberId,
        //    int eventId, Sherpa.Messaging.NotificationTarget notificationTarget)
        //{
        //    var notification = new SendEventUpdateNotification
        //    {
        //        NetworkId = networkId,
        //        MemberId = memberId,
        //        EventId = eventId,
        //        MessageTemplateType = MessageType.System,
        //        NotificationTarget = notificationTarget,
        //        IgnoreMemberPreference = false
        //    };

        //    PublishTargetNotificationLog(notification,
        //        Mapper.Map<SendEventUpdateNotification, TargetNotificationLog<SendEventUpdateNotification>>(notification));
        //}

        //public void PublishSendEventReminderNotification(int networkId, int memberId,
        //    int eventId, Sherpa.Messaging.NotificationTarget notificationTarget)
        //{
        //    var notification = new SendEventReminderNotification
        //    {
        //        NetworkId = networkId,
        //        MemberId = memberId,
        //        EventId = eventId,
        //        MessageTemplateType = MessageType.System,
        //        NotificationTarget = notificationTarget,
        //        IgnoreMemberPreference = false
        //    };

        //    PublishTargetNotificationLog(notification,
        //        Mapper.Map<SendEventReminderNotification, TargetNotificationLog<SendEventReminderNotification>>(
        //            notification));
        //}

        //public void PublishSendEventReplyNotification(int networkId, int memberId,
        //    int eventId, Sherpa.Messaging.NotificationTarget notificationTarget)
        //{
        //    var notification = new SendEventReplyNotification
        //    {
        //        NetworkId = networkId,
        //        MemberId = memberId,
        //        EventId = eventId,
        //        MessageTemplateType = MessageType.System,
        //        NotificationTarget = notificationTarget,
        //        IgnoreMemberPreference = false
        //    };

        //    PublishTargetNotificationLog(notification,
        //        Mapper.Map<SendEventReplyNotification, TargetNotificationLog<SendEventReplyNotification>>(notification));
        //}

        //public void PublishSendEventRsvpReminderNotification(int networkId, int memberId,
        //    int eventId, Sherpa.Messaging.NotificationTarget notificationTarget)
        //{
        //    var notification = new SendEventRsvpReminderNotification
        //    {
        //        NetworkId = networkId,
        //        MemberId = memberId,
        //        EventId = eventId,
        //        MessageTemplateType = MessageType.System,
        //        NotificationTarget = notificationTarget,
        //        IgnoreMemberPreference = false
        //    };

        //    PublishTargetNotificationLog(notification,
        //        Mapper.Map<SendEventRsvpReminderNotification, TargetNotificationLog<SendEventRsvpReminderNotification>>(
        //            notification));
        //}

        //public void PublishSendEventConfirmNotification(int networkId, int memberId,
        //    int eventId, NotificationTarget notificationTarget)
        //{
        //    var notification = new SendEventConfirmNotification
        //    {
        //        NetworkId = networkId,
        //        MemberId = memberId,
        //        EventId = eventId,
        //        MessageTemplateType = MessageType.System,
        //        NotificationTarget = notificationTarget,
        //        IgnoreMemberPreference = false
        //    };

        //    PublishTargetNotificationLog(notification,
        //        Mapper.Map<SendEventConfirmNotification, TargetNotificationLog<SendEventConfirmNotification>>(
        //            notification));
        //}

       // #endregion
    }
}
