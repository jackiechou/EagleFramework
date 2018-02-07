CREATE TABLE [Production].[Product_Discount] (
    [DiscountId]       INT             IDENTITY (0, 1) NOT NULL,
    [Quantity]         INT             NULL,
    [DiscountRate]     DECIMAL (10, 2) NOT NULL,
    [IsPercent]        BIT             NULL,
    [Description]      NVARCHAR (50)   NULL,
    [StartDate]        DATETIME        NULL,
    [EndDate]          DATETIME        NULL,
    [CreatedDate]      DATETIME        NULL,
    [LastModifiedDate] DATETIME        NULL,
    [IsActive]         BIT             NULL,
    [VendorId]         INT             NULL,
    CONSTRAINT [PK__Product___596C1C4845AA1867] PRIMARY KEY CLUSTERED ([DiscountId] ASC)
);

