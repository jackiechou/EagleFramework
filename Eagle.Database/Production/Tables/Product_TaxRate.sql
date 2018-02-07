CREATE TABLE [Production].[Product_TaxRate] (
    [TaxRateId]        INT             IDENTITY (0, 1) NOT NULL,
    [TaxRate]          DECIMAL (10, 2) NULL,
    [IsPercent]        BIT             NULL,
    [Description]      NVARCHAR (50)   NULL,
    [IsActive]         BIT             NULL,
    [CreatedDate]      DATETIME        NULL,
    [LastModifiedDate] DATETIME        NULL,
    CONSTRAINT [PK__Product___4A96282E12F47B68] PRIMARY KEY CLUSTERED ([TaxRateId] ASC)
);

