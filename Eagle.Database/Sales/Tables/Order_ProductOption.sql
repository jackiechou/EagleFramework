CREATE TABLE [Sales].[Order_ProductOption] (
    [OrderProductOptionId] INT              IDENTITY (1, 1) NOT NULL,
    [OrderNo]              UNIQUEIDENTIFIER NOT NULL,
    [ProductNo]            UNIQUEIDENTIFIER NOT NULL,
    [OptionId]             INT              NOT NULL,
    [OptionValue]          MONEY            NOT NULL,
    CONSTRAINT [PK_Order_Product_Options] PRIMARY KEY CLUSTERED ([OrderProductOptionId] ASC)
);

