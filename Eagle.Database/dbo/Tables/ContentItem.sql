CREATE TABLE [dbo].[ContentItem] (
    [ContentItemId] INT            IDENTITY (1, 1) NOT NULL,
    [ContentTypeId] INT            NOT NULL,
    [ItemKey]       NVARCHAR (150) NULL,
    [ItemText]      NVARCHAR (MAX) NULL,
    [IsActive]      BIT            NOT NULL,
    CONSTRAINT [PK__ContentI__B851BCEC9935CB52] PRIMARY KEY CLUSTERED ([ContentItemId] ASC)
);

