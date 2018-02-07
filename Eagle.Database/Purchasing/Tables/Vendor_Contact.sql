CREATE TABLE [Purchasing].[Vendor_Contact] (
    [VendorContactId] INT IDENTITY (1, 1) NOT NULL,
    [VendorId]        INT NOT NULL,
    [ContactId]       INT NOT NULL,
    PRIMARY KEY CLUSTERED ([VendorContactId] ASC)
);

