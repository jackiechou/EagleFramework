CREATE TABLE [Media].[Media_PlayList] (
    [TypeId]               INT              CONSTRAINT [DF_Media_PlayLists_TypeId] DEFAULT ((3)) NOT NULL,
    [PlayListId]           INT              IDENTITY (1, 1) NOT NULL,
    [PlayListName]         NVARCHAR (250)   NULL,
    [Alias]                NVARCHAR (250)   NULL,
    [FrontImage]           NVARCHAR (250)   NULL,
    [MainImage]            NVARCHAR (250)   NULL,
    [Description]          NVARCHAR (MAX)   NULL,
    [TotalViews]           INT              NULL,
    [SortOrder]            INT              NULL,
    [Status]               INT              NULL,
    [Ip]                   VARCHAR (30)     NULL,
    [LastUpdatedIp]        VARCHAR (30)     NULL,
    [CreatedDate]          DATETIME         NULL,
    [LastModifiedDate]     DATETIME         NULL,
    [CreatedByUserId]      UNIQUEIDENTIFIER NULL,
    [LastModifiedByUserId] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK__Media_Pl__B30167A009E0127A] PRIMARY KEY CLUSTERED ([PlayListId] ASC)
);

