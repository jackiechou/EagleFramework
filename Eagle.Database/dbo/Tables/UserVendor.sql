CREATE TABLE [dbo].[UserVendor] (
    [UserVendorId] INT              IDENTITY (1, 1) NOT NULL,
    [UserId]       UNIQUEIDENTIFIER NOT NULL,
    [VendorId]     INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([UserVendorId] ASC)
);

