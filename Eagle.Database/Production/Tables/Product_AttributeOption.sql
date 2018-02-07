CREATE TABLE [Production].[Product_AttributeOption] (
    [Attribute_Id] INT             NOT NULL,
    [Option_Id]    INT             IDENTITY (1, 1) NOT NULL,
    [Option_Name]  NVARCHAR (255)  NOT NULL,
    [Option_Value] DECIMAL (12, 5) NULL,
    PRIMARY KEY CLUSTERED ([Option_Id] ASC)
);

