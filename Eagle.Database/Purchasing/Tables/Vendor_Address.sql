CREATE TABLE [Purchasing].[Vendor_Address] (
    [VendorAddressId] INT      IDENTITY (1, 1) NOT NULL,
    [VendorId]        INT      NOT NULL,
    [AddressId]       INT      NOT NULL,
    [ModifiedDate]    DATETIME NOT NULL,
    PRIMARY KEY CLUSTERED ([VendorAddressId] ASC)
);

