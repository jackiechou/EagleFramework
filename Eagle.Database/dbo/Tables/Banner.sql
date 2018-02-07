﻿CREATE TABLE [dbo].[Banner] (
    [BannerId]             INT              IDENTITY (1, 1) NOT NULL,
    [BannerPositionId]     INT              NOT NULL,
    [BannerScopeId]        INT              NOT NULL,
    [BannerTypeId]         INT              NOT NULL,
    [LanguageCode]         VARCHAR (5)      NULL,
    [VendorId]             INT              NULL,
    [BannerTitle]          NVARCHAR (300)   NOT NULL,
    [BannerContent]        NVARCHAR (MAX)   NULL,
    [Advertiser]           NVARCHAR (250)   NULL,
    [FileId]               INT              NULL,
    [AltText]              NVARCHAR (250)   NULL,
    [Link]                 NVARCHAR (4000)  NULL,
    [Target]               NVARCHAR (6)     NULL,
    [Description]          NTEXT            NULL,
    [Tags]                 NVARCHAR (4000)  NULL,
    [ListOrder]            INT              NULL,
    [ClickThroughs]        INT              NULL,
    [Width]                INT              NULL,
    [Height]               INT              NULL,
    [StartDate]            DATETIME         NULL,
    [EndDate]              DATETIME         NULL,
    [Status]               INT              NULL,
    [CreatedByUserId]      UNIQUEIDENTIFIER NULL,
    [CreatedDate]          DATETIME         NULL,
    [LastModifiedByUserId] UNIQUEIDENTIFIER NULL,
    [LastModifiedDate]     DATETIME         NULL,
    CONSTRAINT [PK_Banners] PRIMARY KEY CLUSTERED ([BannerId] ASC)
);

