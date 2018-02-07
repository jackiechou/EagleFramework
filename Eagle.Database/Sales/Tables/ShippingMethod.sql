CREATE TABLE [Sales].[ShippingMethod] (
    [ShippingMethod_Id]   INT            IDENTITY (1, 1) NOT NULL,
    [ShippingMethod_Name] NVARCHAR (150) NOT NULL,
    [ModifiedDate]        DATETIME       NULL,
    [Discontinued]        BIT            NULL,
    CONSTRAINT [PK__ShipMeth__8B21A5BE75392FF8] PRIMARY KEY CLUSTERED ([ShippingMethod_Id] ASC)
);

