CREATE TABLE [Media].[Media_Composer] (
    [ComposerId]           INT              IDENTITY (1, 1) NOT NULL,
    [ComposerName]         NVARCHAR (250)   NULL,
    [Alias]                NVARCHAR (250)   NULL,
    [FrontImage]           NVARCHAR (250)   NULL,
    [MainImage]            NVARCHAR (250)   NULL,
    [Description]          NVARCHAR (250)   NULL,
    [ListOrder]            INT              NULL,
    [Status]               INT              NULL,
    [Ip]                   VARCHAR (30)     NOT NULL,
    [LastUpdatedIp]        VARCHAR (30)     NULL,
    [CreatedDate]          DATETIME         NULL,
    [LastModifiedDate]     DATETIME         NULL,
    [CreatedByUserId]      UNIQUEIDENTIFIER NULL,
    [LastModifiedByUserId] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK__Media_Co__16F22C1E1551C526] PRIMARY KEY CLUSTERED ([ComposerId] ASC)
);

