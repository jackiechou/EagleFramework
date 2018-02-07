CREATE TABLE [dbo].[City] (
    [CityId]   INT            NOT NULL,
    [CityName] NVARCHAR (255) NOT NULL,
    [IsActive] BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([CityId] ASC)
);

