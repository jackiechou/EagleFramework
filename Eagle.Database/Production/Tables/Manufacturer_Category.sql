CREATE TABLE [Production].[Manufacturer_Category] (
    [ManufacturerCategoryId] INT            IDENTITY (1, 1) NOT NULL,
    [CategoryName]           NVARCHAR (64)  NOT NULL,
    [CategoryDesc]           NVARCHAR (MAX) NULL,
    [IsActive]               BIT            NULL,
    [VendorId]               INT            NOT NULL,
    CONSTRAINT [PK__Manufact__5D8C04B3A8826484] PRIMARY KEY CLUSTERED ([ManufacturerCategoryId] ASC)
);

