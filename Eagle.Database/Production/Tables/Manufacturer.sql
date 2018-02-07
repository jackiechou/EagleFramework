CREATE TABLE [Production].[Manufacturer] (
    [ManufacturerId]         INT            IDENTITY (1, 1) NOT NULL,
    [ManufacturerCategoryId] INT            NULL,
    [ManufacturerName]       NVARCHAR (40)  NOT NULL,
    [Address]                NVARCHAR (60)  NULL,
    [Email]                  NVARCHAR (100) NULL,
    [Phone]                  NVARCHAR (24)  NULL,
    [Fax]                    NVARCHAR (24)  NULL,
    [IsActive]               BIT            NULL,
    [VendorId]               INT            NOT NULL,
    CONSTRAINT [PK__Manufact__357E5CC1096B112E] PRIMARY KEY CLUSTERED ([ManufacturerId] ASC)
);

