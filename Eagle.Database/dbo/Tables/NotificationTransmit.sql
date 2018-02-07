CREATE TABLE [dbo].[NotificationTransmit] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [NotificationId] UNIQUEIDENTIFIER NOT NULL,
    [Status]         INT              NOT NULL,
    [CreateDate]     DATETIME         NOT NULL,
    [CompleteDate]   DATETIME         NULL,
    [IsDeleted]      BIT              NOT NULL,
    CONSTRAINT [PK_dbo.NotificationTransmit] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.NotificationTransmit_dbo.Notification_NotificationId] FOREIGN KEY ([NotificationId]) REFERENCES [dbo].[Notification] ([Id]) ON DELETE CASCADE
);

