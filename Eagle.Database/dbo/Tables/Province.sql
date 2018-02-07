CREATE TABLE [dbo].[Province] (
    [ProvinceId]   INT            NOT NULL,
    [ProvinceCode] VARCHAR (10)   NOT NULL,
    [ProvinceName] NVARCHAR (255) NOT NULL,
    [ListOrder]    INT            NOT NULL,
    [IsPublished]  BIT            NOT NULL,
    CONSTRAINT [PK_IBMS_Province] PRIMARY KEY CLUSTERED ([ProvinceId] ASC)
);

