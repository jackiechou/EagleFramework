CREATE TABLE [Production].[Product_Attribute] (
    [ProductNo]     UNIQUEIDENTIFIER NOT NULL,
    [AttributeId]   INT              IDENTITY (1, 1) NOT NULL,
    [AttributeName] NVARCHAR (255)   NOT NULL,
    [ListOrder]     INT              NOT NULL,
    [IsActive]      BIT              NULL,
    CONSTRAINT [PK_Product_Attributes] PRIMARY KEY CLUSTERED ([AttributeId] ASC)
);

