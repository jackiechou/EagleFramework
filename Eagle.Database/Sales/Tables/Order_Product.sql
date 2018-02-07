CREATE TABLE [Sales].[Order_Product] (
    [OrderProductId]   INT              IDENTITY (1, 1) NOT NULL,
    [OrderNo]          UNIQUEIDENTIFIER NOT NULL,
    [ProductNo]        UNIQUEIDENTIFIER NOT NULL,
    [Quantity]         SMALLINT         NOT NULL,
    [UnitPrice]        MONEY            NOT NULL,
    [CreatedDate]      DATETIME         NULL,
    [LastModifiedDate] DATETIME         NULL,
    CONSTRAINT [PK_Order_Product] PRIMARY KEY CLUSTERED ([OrderProductId] ASC)
);

