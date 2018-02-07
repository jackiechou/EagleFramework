CREATE TABLE [dbo].[ApplicationFolder] (
    [FolderId]   INT              IDENTITY (1, 1) NOT NULL,
    [FolderCode] UNIQUEIDENTIFIER NOT NULL,
    [FolderKey]  VARCHAR (50)     NOT NULL,
    [FolderPath] VARCHAR (300)    NOT NULL,
    [IsActive]   BIT              NULL,
    CONSTRAINT [PK_ApplicationFolders] PRIMARY KEY CLUSTERED ([FolderId] ASC)
);

