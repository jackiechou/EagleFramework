CREATE TABLE [Purchasing].[Vendor_Currency] (
    [VendorCurrencyId]  INT           CONSTRAINT [DF_Vendor_Currency_VendorCurrencyId] DEFAULT ((0)) NOT NULL,
    [VendorId]          INT           NOT NULL,
    [CurrencyCode]      NCHAR (3)     NOT NULL,
    [CurrencySymbol]    NVARCHAR (50) NULL,
    [Decimals]          INT           NULL,
    [DecimalSymbol]     CHAR (1)      NULL,
    [ThousandSeparator] CHAR (1)      NULL,
    [PositiveFormat]    CHAR (1)      NULL,
    [NegativeFormat]    CHAR (1)      NULL,
    CONSTRAINT [PK_Vendor_Currency] PRIMARY KEY CLUSTERED ([VendorCurrencyId] ASC)
);

