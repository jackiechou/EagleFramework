CREATE TABLE [dbo].[MessageQueue] (
    [QueueId]         INT            IDENTITY (1, 1) NOT NULL,
    [From]            NVARCHAR (250) NOT NULL,
    [To]              NVARCHAR (MAX) NOT NULL,
    [Subject]         NVARCHAR (250) NOT NULL,
    [Body]            NVARCHAR (MAX) NOT NULL,
    [Bcc]             NVARCHAR (MAX) NULL,
    [Cc]              NVARCHAR (MAX) NULL,
    [Status]          BIT            DEFAULT ((1)) NOT NULL,
    [ResponseStatus]  INT            DEFAULT ((0)) NULL,
    [ResponseMessage] NVARCHAR (MAX) NULL,
    [PredefinedDate]  DATETIME       NULL,
    [SentDate]        DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([QueueId] ASC)
);

