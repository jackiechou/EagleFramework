CREATE TABLE [dbo].[DocumentFolder] (
    [FolderId]             INT              IDENTITY (1, 1) NOT NULL,
    [FolderCode]           UNIQUEIDENTIFIER NOT NULL,
    [FolderKey]            NVARCHAR (50)    NULL,
    [FolderPath]           VARCHAR (300)    NOT NULL,
    [FolderIcon]           VARCHAR (300)    NULL,
    [Description]          NVARCHAR (MAX)   NULL,
    [IsActive]             BIT              CONSTRAINT [DF_DocumentFolder_IsActive] DEFAULT ((1)) NULL,
    [Ip]                   VARCHAR (30)     NULL,
    [LastUpdatedIp]        VARCHAR (30)     NULL,
    [CreatedDate]          DATETIME         NULL,
    [LastModifiedDate]     DATETIME         NULL,
    [CreatedByUserId]      UNIQUEIDENTIFIER NULL,
    [LastModifiedByUserId] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_DocumentFolder] PRIMARY KEY CLUSTERED ([FolderId] ASC)
);

