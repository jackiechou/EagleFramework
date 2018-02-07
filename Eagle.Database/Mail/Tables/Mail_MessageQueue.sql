CREATE TABLE [Mail].[Mail_MessageQueue] (
    [QueueId]         INT            IDENTITY (1, 1) NOT NULL,
    [FromSender]      NVARCHAR (250) NOT NULL,
    [ToReceiver]      NVARCHAR (MAX) NOT NULL,
    [Subject]         NVARCHAR (250) NOT NULL,
    [Message]         NVARCHAR (MAX) NOT NULL,
    [Bcc]             NVARCHAR (MAX) NULL,
    [Cc]              NVARCHAR (MAX) NULL,
    [Status]          BIT            CONSTRAINT [DF__Mail_Mess__Statu__1A6AB4A7] DEFAULT ((1)) NOT NULL,
    [ResponseStatus]  INT            CONSTRAINT [DF__Mail_Mess__Respo__1B5ED8E0] DEFAULT ((0)) NULL,
    [ResponseMessage] NVARCHAR (MAX) NULL,
    [PredefinedDate]  DATETIME       NULL,
    [SentDate]        DATETIME       NULL,
    CONSTRAINT [PK__Mail_Mes__8324E71549CA1D5D] PRIMARY KEY CLUSTERED ([QueueId] ASC)
);

