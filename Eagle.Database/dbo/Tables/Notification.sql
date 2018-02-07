CREATE TABLE [dbo].[Notification] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [Type]              INT              NOT NULL,
    [ReferenceObjectId] UNIQUEIDENTIFIER NULL,
    [SendDate]          DATETIME         NOT NULL,
    [IsDeleted]         BIT              NOT NULL,
    [MessageQueueId]    UNIQUEIDENTIFIER NULL,
    [FromAccountId]     UNIQUEIDENTIFIER NULL,
    [ToAccountId]       UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_dbo.Notification] PRIMARY KEY CLUSTERED ([Id] ASC)
);

