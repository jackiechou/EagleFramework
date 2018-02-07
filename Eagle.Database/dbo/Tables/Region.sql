CREATE TABLE [dbo].[Region] (
    [RegionId]    INT            NOT NULL,
    [RegionCode]  VARCHAR (255)  NOT NULL,
    [RegionName]  NVARCHAR (255) NOT NULL,
    [ProvinceId]  INT            NOT NULL,
    [IsPublished] BIT            NOT NULL,
    CONSTRAINT [PK__AppCount__FD0A6F83BF5E367D] PRIMARY KEY CLUSTERED ([RegionId] ASC)
);

