CREATE TABLE [dbo].[Log] (
    [LogId]         INT              IDENTITY (1, 1) NOT NULL,
    [LogLevel]      VARCHAR (50)     NOT NULL,
    [LogType]       VARCHAR (50)     NOT NULL,
    [ActionType]    VARCHAR (50)     NOT NULL,
    [OldData]       VARCHAR (MAX)    NOT NULL,
    [NewData]       VARCHAR (MAX)    NOT NULL,
    [Thread]        VARCHAR (255)    NOT NULL,
    [Message]       VARCHAR (4000)   NOT NULL,
    [Exception]     VARCHAR (2000)   NULL,
    [Logger]        UNIQUEIDENTIFIER NOT NULL,
    [LogDate]       DATETIME         NOT NULL,
    [ApplicationId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED ([LogId] ASC)
);

