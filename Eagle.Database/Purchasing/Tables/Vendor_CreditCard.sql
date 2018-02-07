CREATE TABLE [Purchasing].[Vendor_CreditCard] (
    [VendorCreditCardId] INT IDENTITY (1, 1) NOT NULL,
    [VendorId]           INT NOT NULL,
    [CreditCardId]       INT NOT NULL,
    PRIMARY KEY CLUSTERED ([VendorCreditCardId] ASC)
);

