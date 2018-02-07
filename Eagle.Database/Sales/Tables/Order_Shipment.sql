CREATE TABLE [Sales].[Order_Shipment] (
    [OrderShipmentId] INT              IDENTITY (1, 1) NOT NULL,
    [OrderNo]         UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL,
    [CustomerNo]      UNIQUEIDENTIFIER NULL,
    [ShipMethodId]    INT              NOT NULL,
    [ShipDate]        DATETIME         NULL,
    [Weight]          DECIMAL (10, 2)  NULL,
    [IsInternational] BIT              NULL,
    [ReceiverName]    NVARCHAR (200)   NULL,
    [ReceiverEmail]   VARCHAR (70)     NULL,
    [ReceiverPhone]   NVARCHAR (30)    NULL,
    [ReceiverAddress] NVARCHAR (4000)  NULL,
    [CountryId]       INT              NULL,
    [ProvinceId]      INT              NULL,
    [CityId]          INT              NULL,
    [RegionId]        INT              NULL,
    [PostalCode]      NVARCHAR (30)    NULL,
    CONSTRAINT [PK__Order_Sh__B3CBD755053D955D] PRIMARY KEY CLUSTERED ([OrderShipmentId] ASC)
);

