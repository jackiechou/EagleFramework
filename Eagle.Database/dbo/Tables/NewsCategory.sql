CREATE TABLE [dbo].[NewsCategory] (
    [LanguageCode]         NVARCHAR (5)     NOT NULL,
    [CategoryId]           INT              IDENTITY (1, 1) NOT NULL,
    [CategoryCode]         NVARCHAR (150)   NULL,
    [ParentId]             INT              NULL,
    [CategoryName]         NVARCHAR (150)   NOT NULL,
    [Alias]                NVARCHAR (150)   NOT NULL,
    [Depth]                INT              NULL,
    [Lineage]              NVARCHAR (100)   NULL,
    [ListOrder]            INT              NULL,
    [CategoryImage]        INT              NULL,
    [Description]          NVARCHAR (MAX)   NULL,
    [NavigateUrl]          NVARCHAR (1000)  NULL,
    [Status]               INT              NULL,
    [CreatedDate]          DATETIME         NULL,
    [LastModifiedDate]     DATETIME         NULL,
    [CreatedByUserId]      UNIQUEIDENTIFIER NULL,
    [LastModifiedByUserId] UNIQUEIDENTIFIER NULL,
    [IP]                   NVARCHAR (20)    NULL,
    [LastUpdatedIp]        NVARCHAR (20)    NULL,
    CONSTRAINT [PK_NewsCategories] PRIMARY KEY CLUSTERED ([CategoryId] ASC)
);

