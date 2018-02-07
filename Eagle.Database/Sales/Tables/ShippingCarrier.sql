CREATE TABLE [Sales].[ShippingCarrier] (
    [ShippingCarrierId]   INT            IDENTITY (1, 1) NOT NULL,
    [ShippingCarrierName] NVARCHAR (250) NOT NULL,
    [ListOrder]           INT            NULL,
    [IsActive]            BIT            NULL,
    CONSTRAINT [PK_Shippers] PRIMARY KEY CLUSTERED ([ShippingCarrierId] ASC)
);

