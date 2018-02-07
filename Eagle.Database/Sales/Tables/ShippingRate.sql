CREATE TABLE [Sales].[ShippingRate] (
    [ShippingCarrier_Id]        INT             NOT NULL,
    [ShippingMethod_Id]         INT             NOT NULL,
    [ShippingRate_Id]           INT             IDENTITY (1, 1) NOT NULL,
    [ShippingRate_CountryId]    INT             NOT NULL,
    [ShippingRate_Name]         NVARCHAR (255)  NOT NULL,
    [ShippingRate_ZipStart]     NVARCHAR (50)   NOT NULL,
    [ShippingRate_ZipEnd]       NVARCHAR (50)   NOT NULL,
    [ShippingRate_WeightStart]  DECIMAL (10, 3) NOT NULL,
    [ShippingRate_WeightEnd]    DECIMAL (10, 3) NOT NULL,
    [ShippingRate_Value]        DECIMAL (10, 2) NOT NULL,
    [ShippingRate_PackageFee]   DECIMAL (10, 2) NOT NULL,
    [ShippingRate_VAT]          DECIMAL (10, 2) NOT NULL,
    [ShippingRate_CurrencyCode] NCHAR (3)       NOT NULL,
    [ShippingRate_ListOrder]    INT             NOT NULL,
    CONSTRAINT [PK_ShippingRates] PRIMARY KEY CLUSTERED ([ShippingRate_Id] ASC)
);

