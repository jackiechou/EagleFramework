CREATE TABLE [dbo].[NotificationTarget] (
    [MailGroupId]    UNIQUEIDENTIFIER NOT NULL,
    [NotificationId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_dbo.NotificationTarget] PRIMARY KEY CLUSTERED ([MailGroupId] ASC, [NotificationId] ASC),
    CONSTRAINT [FK_dbo.NotificationTarget_dbo.Notification_MailGroupId] FOREIGN KEY ([MailGroupId]) REFERENCES [dbo].[Notification] ([Id]) ON DELETE CASCADE
);

