CREATE TABLE [dbo].[UserAddress] (
    [UserAddressId]    INT              IDENTITY (1, 1) NOT NULL,
    [AddressId]        INT              NULL,
    [UserId]           UNIQUEIDENTIFIER NOT NULL,
    [IsDefaultAddress] BIT              NULL,
    [IsActive]         BIT              NULL,
    PRIMARY KEY CLUSTERED ([UserAddressId] ASC)
);

