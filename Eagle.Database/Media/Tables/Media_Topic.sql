CREATE TABLE [Media].[Media_Topic] (
    [TopicId]              INT              IDENTITY (1, 1) NOT NULL,
    [TypeId]               INT              NULL,
    [ParentId]             INT              NULL,
    [ListOrder]            INT              NULL,
    [Name]                 NVARCHAR (100)   NOT NULL,
    [Alias]                NVARCHAR (100)   NULL,
    [Photo]                NVARCHAR (250)   NULL,
    [Description]          NVARCHAR (250)   NULL,
    [CreatedDate]          DATETIME         NULL,
    [LastModifiedDate]     DATETIME         NULL,
    [Ip]                   VARCHAR (30)     NULL,
    [LastUpdatedIp]        VARCHAR (30)     NULL,
    [CreatedByUserId]      UNIQUEIDENTIFIER NULL,
    [LastModifiedByUserId] UNIQUEIDENTIFIER NULL,
    [Status]               INT              NULL,
    CONSTRAINT [PK__Media_To__022E0F5D25BD3719] PRIMARY KEY CLUSTERED ([TopicId] ASC)
);

