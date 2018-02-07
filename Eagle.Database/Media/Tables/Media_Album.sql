CREATE TABLE [Media].[Media_Album] (
    [TypeId]               INT              CONSTRAINT [DF_Media_Albums_TypeId] DEFAULT ((3)) NOT NULL,
    [AlbumId]              INT              IDENTITY (1, 1) NOT NULL,
    [AlbumName]            NVARCHAR (250)   NOT NULL,
    [Alias]                NVARCHAR (250)   NOT NULL,
    [FrontImage]           NVARCHAR (250)   NULL,
    [MainImage]            NVARCHAR (250)   NULL,
    [Description]          NVARCHAR (MAX)   NULL,
    [TotalViews]           INT              NULL,
    [SortKey]              INT              NULL,
    [Ip]                   VARCHAR (30)     NOT NULL,
    [LastUpdatedIp]        VARCHAR (30)     NULL,
    [CreatedDate]          DATETIME         NULL,
    [LastModifiedDate]     DATETIME         NULL,
    [CreatedByUserId]      UNIQUEIDENTIFIER NULL,
    [LastModifiedByUserId] UNIQUEIDENTIFIER NULL,
    [Status]               INT              NULL,
    CONSTRAINT [PK_Media_Albums] PRIMARY KEY CLUSTERED ([AlbumId] ASC)
);

