CREATE TABLE [dbo].[Country] (
    [CountryId]    INT           IDENTITY (1, 1) NOT NULL,
    [CountryName]  VARCHAR (100) NOT NULL,
    [Iso2Alpha]    VARCHAR (2)   NULL,
    [Iso3Alpha]    VARCHAR (3)   NULL,
    [IanaInternet] VARCHAR (4)   NULL,
    [UnVehicle]    VARCHAR (4)   NULL,
    [IocOlympic]   VARCHAR (4)   NULL,
    [UnIsoNumeric] VARCHAR (4)   NULL,
    [ItuCalling]   VARCHAR (6)   NULL,
    [IsPublished]  BIT           NULL,
    CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED ([CountryId] ASC)
);

