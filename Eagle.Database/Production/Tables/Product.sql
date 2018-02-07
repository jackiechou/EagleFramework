﻿CREATE TABLE [Production].[Product] (
    [ProductId]              INT              IDENTITY (1, 1) NOT NULL,
    [ProductNo]              UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL,
    [ProductCode]            NVARCHAR (50)    NULL,
    [CategoryCode]           VARCHAR (100)    NOT NULL,
    [LanguageCode]           VARCHAR (5)      NULL,
    [ManufacturerId]         INT              NULL,
    [ProductTypeId]          INT              NULL,
    [ProductName]            NVARCHAR (500)   NULL,
    [ProductAlias]           NVARCHAR (500)   NOT NULL,
    [DiscountId]             INT              NULL,
    [VendorId]               INT              NULL,
    [CurrencyCode]           NVARCHAR (16)    NULL,
    [NetPrice]               MONEY            NULL,
    [GrossPrice]             MONEY            NULL,
    [TaxRateId]              INT              NULL,
    [UnitsInStock]           INT              CONSTRAINT [DF_Products_UnitsInStock] DEFAULT ((0)) NULL,
    [UnitsOnOrder]           INT              CONSTRAINT [DF_Products_UnitsOnOrder] DEFAULT ((0)) NULL,
    [UnitsInAPackage]        INT              CONSTRAINT [DF_Products_UnitsInAPackage] DEFAULT ((0)) NULL,
    [UnitsInBox]             INT              CONSTRAINT [DF_Products_UnitsInBox] DEFAULT ((0)) NULL,
    [Unit]                   NVARCHAR (20)    NULL,
    [Weight]                 DECIMAL (10, 4)  NULL,
    [UnitOfWeightMeasure]    NVARCHAR (20)    NULL,
    [Length]                 DECIMAL (10, 4)  NULL,
    [Width]                  DECIMAL (10, 4)  NULL,
    [Height]                 DECIMAL (10, 4)  NULL,
    [UnitOfDimensionMeasure] NVARCHAR (20)    NULL,
    [Url]                    VARCHAR (255)    NULL,
    [MinPurchaseQty]         INT              CONSTRAINT [DF_Products_MinPurchaseQty] DEFAULT ((0)) NULL,
    [MaxPurchaseQty]         INT              NULL,
    [ListOrder]              INT              NULL,
    [Views]                  INT              CONSTRAINT [DF_Products_Views] DEFAULT ((0)) NULL,
    [PhotoFileName]          NVARCHAR (255)   NULL,
    [ThumbnailPhotoFileName] NVARCHAR (255)   NULL,
    [ShortDescription]       NVARCHAR (600)   NULL,
    [Specification]          NVARCHAR (MAX)   NULL,
    [Availability]           NVARCHAR (50)    NULL,
    [StartDate]              DATETIME         NULL,
    [EndDate]                DATETIME         NULL,
    [PurchaseScope]          NVARCHAR (50)    NULL,
    [Warranty]               NVARCHAR (20)    NULL,
    [IsOnline]               BIT              NULL,
    [InfoNotification]       BIT              CONSTRAINT [DF_Products_InfoStatus] DEFAULT ((1)) NULL,
    [PriceNotification]      BIT              CONSTRAINT [DF_Products_PriceStatus] DEFAULT ((1)) NULL,
    [QtyNotification]        BIT              CONSTRAINT [DF_Products_QtyStatus] DEFAULT ((1)) NULL,
    [Status]                 INT              CONSTRAINT [DF_Products_Discontinued] DEFAULT ((1)) NULL,
    [Ip]                     VARCHAR (30)     NULL,
    [LastUpdatedIp]          VARCHAR (30)     NULL,
    [CreatedDate]            DATETIME         NULL,
    [LastModifiedDate]       DATETIME         NULL,
    [CreatedByUserId]        UNIQUEIDENTIFIER NULL,
    [LastModifiedByUserId]   UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([ProductId] ASC)
);

