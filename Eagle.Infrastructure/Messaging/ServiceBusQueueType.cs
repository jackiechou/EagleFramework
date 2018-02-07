namespace Eagle.Infrastructure.Messaging
{
    public enum ServiceBusQueueType
    {
        ScheduleTaskPublisher = 1,
        ScheduleTaskSubscriber = 2,
        TaskSchedulerPublisher = 3,
        TaskSchedulerSubscriber = 4,
        NotificationPublisher = 5,
        NotificationSubscriber = 6,
        ApplicationPublisher = 11,
        ApplicationSubscriber = 12
    }
}
