CREATE TABLE [dbo].[Address] (
    [AddressId]     INT            IDENTITY (1, 1) NOT NULL,
    [AddressTypeId] INT            NOT NULL,
    [Street]        NVARCHAR (512) NULL,
    [City]          NVARCHAR (512) NULL,
    [PostalCode]    VARCHAR (10)   NULL,
    [Description]   NVARCHAR (MAX) NULL,
    [RegionId]      INT            NULL,
    [ProvinceId]    INT            NULL,
    [CountryId]     INT            NULL,
    [ModifiedDate]  DATETIME       NULL,
    CONSTRAINT [PK__Address__091C2AFB5351711C] PRIMARY KEY CLUSTERED ([AddressId] ASC)
);

