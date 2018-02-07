CREATE TABLE [dbo].[MessageCategory] (
    [CategoryId]       INT            IDENTITY (1, 1) NOT NULL,
    [ParentId]         INT            NOT NULL,
    [CategoryName]     NVARCHAR (MAX) NULL,
    [Description]      NVARCHAR (MAX) NULL,
    [SortKey]          INT            NOT NULL,
    [Depth]            INT            NOT NULL,
    [CreatedDate]      DATETIME       NOT NULL,
    [LastModifiedDate] DATETIME       NULL,
    [Status]           INT            NOT NULL,
    CONSTRAINT [PK_dbo.MessageCategory] PRIMARY KEY CLUSTERED ([CategoryId] ASC)
);

