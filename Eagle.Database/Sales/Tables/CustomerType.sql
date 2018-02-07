CREATE TABLE [Sales].[CustomerType] (
    [CustomerTypeId]       INT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerTypeName]     NVARCHAR (50)    NOT NULL,
    [PromotionalRate]      INT              NULL,
    [Note]                 NVARCHAR (256)   NULL,
    [IsActive]             BIT              NOT NULL,
    [CreatedDate]          DATETIME         NULL,
    [LastModifiedDate]     DATETIME         NULL,
    [Ip]                   VARCHAR (30)     NULL,
    [LastUpdatedIp]        VARCHAR (30)     NULL,
    [CreatedByUserId]      UNIQUEIDENTIFIER NULL,
    [LastModifiedByUserId] UNIQUEIDENTIFIER NULL,
    [VendorId]             INT              NULL,
    CONSTRAINT [PK__Customer__B814E36471689F14] PRIMARY KEY CLUSTERED ([CustomerTypeId] ASC)
);

