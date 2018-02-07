CREATE TABLE [dbo].[ApplicationFile] (
    [ApplicationId]        INT              NULL,
    [FileId]               INT              IDENTITY (1, 1) NOT NULL,
    [FileCode]             UNIQUEIDENTIFIER NOT NULL,
    [FileTitle]            NVARCHAR (100)   NOT NULL,
    [FileName]             NVARCHAR (100)   NOT NULL,
    [Extension]            NVARCHAR (100)   NOT NULL,
    [ContentType]          NVARCHAR (200)   NOT NULL,
    [FolderId]             INT              NOT NULL,
    [FileContent]          VARBINARY (MAX)  NULL,
    [FileDescription]      NVARCHAR (MAX)   NULL,
    [Size]                 INT              NULL,
    [Width]                INT              NULL,
    [Height]               INT              NULL,
    [CreatedOnDate]        DATETIME         NULL,
    [LastModifiedOnDate]   DATETIME         NULL,
    [CreatedByUserId]      INT              NULL,
    [LastModifiedByUserId] INT              NULL,
    CONSTRAINT [PK_Files] PRIMARY KEY CLUSTERED ([FileId] ASC)
);

