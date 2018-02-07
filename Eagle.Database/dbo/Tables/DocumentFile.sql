CREATE TABLE [dbo].[DocumentFile] (
    [FileId]               INT              IDENTITY (1, 1) NOT NULL,
    [FolderId]             INT              NULL,
    [FileCode]             UNIQUEIDENTIFIER CONSTRAINT [DF__DocumentFile__FileCode] DEFAULT (newid()) NOT NULL,
    [FileTitle]            NVARCHAR (100)   NOT NULL,
    [FileName]             NVARCHAR (100)   NOT NULL,
    [FileExtension]        NVARCHAR (100)   NOT NULL,
    [FileVersion]          NVARCHAR (50)    NULL,
    [FileContent]          VARBINARY (MAX)  NULL,
    [FileType]             NVARCHAR (128)   NOT NULL,
    [FileDescription]      NVARCHAR (MAX)   NULL,
    [StorageType]          NVARCHAR (50)    NULL,
    [Size]                 INT              NULL,
    [Width]                INT              NULL,
    [Height]               INT              NULL,
    [IsActive]             BIT              NULL,
    [CreatedDate]          DATETIME         NULL,
    [LastModifiedDate]     DATETIME         NULL,
    [CreatedByUserId]      INT              NULL,
    [LastModifiedByUserId] INT              NULL,
    CONSTRAINT [PK__DocumentFile] PRIMARY KEY CLUSTERED ([FileId] ASC)
);

