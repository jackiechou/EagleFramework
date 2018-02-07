CREATE TABLE [dbo].[UsersOnline] (
    [UserID]         UNIQUEIDENTIFIER NOT NULL,
    [ApplicationId]  UNIQUEIDENTIFIER NOT NULL,
    [TabID]          INT              NOT NULL,
    [CreationDate]   DATETIME         NOT NULL,
    [LastActiveDate] DATETIME         NOT NULL,
    PRIMARY KEY CLUSTERED ([UserID] ASC)
);

