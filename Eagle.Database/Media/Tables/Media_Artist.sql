CREATE TABLE [Media].[Media_Artist] (
    [ArtistId]             INT              IDENTITY (1, 1) NOT NULL,
    [ArtistName]           NVARCHAR (255)   NOT NULL,
    [Alias]                NVARCHAR (255)   NULL,
    [FrontImage]           NVARCHAR (255)   NULL,
    [MainImage]            NVARCHAR (255)   NULL,
    [Description]          NVARCHAR (MAX)   NULL,
    [ListOrder]            INT              NULL,
    [Ip]                   VARCHAR (30)     NULL,
    [LastUpdatedIp]        VARCHAR (30)     NULL,
    [CreatedDate]          DATETIME         NULL,
    [LastModifiedDate]     DATETIME         NULL,
    [CreatedByUserId]      UNIQUEIDENTIFIER NULL,
    [LastModifiedByUserId] UNIQUEIDENTIFIER NULL,
    [Status]               INT              NULL,
    CONSTRAINT [PK__Media_Ar__25706B500DB0A35E] PRIMARY KEY CLUSTERED ([ArtistId] ASC)
);

