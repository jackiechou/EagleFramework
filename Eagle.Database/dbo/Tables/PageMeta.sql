CREATE TABLE [dbo].[PageMeta] (
    [PageId]    INT            NOT NULL,
    [MetaId]    INT            IDENTITY (1, 1) NOT NULL,
    [MetaKey]   VARCHAR (225)  NOT NULL,
    [MetaValue] NVARCHAR (MAX) NOT NULL,
    [Title]     NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_PageMeta] PRIMARY KEY CLUSTERED ([MetaId] ASC)
);

