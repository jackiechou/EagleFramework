CREATE TABLE [Production].[Product_Type] (
    [ProductTypeId]   INT            IDENTITY (1, 1) NOT NULL,
    [ProductTypeName] NVARCHAR (250) NULL,
    [Description]     NVARCHAR (MAX) NULL,
    [ListOrder]       INT            NULL,
    [IsActive]        BIT            NULL,
    [CategoryCode]    VARCHAR (100)  NULL,
    [VendorId]        INT            NOT NULL,
    CONSTRAINT [PK_Product_Types] PRIMARY KEY CLUSTERED ([ProductTypeId] ASC)
);

