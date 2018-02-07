CREATE TABLE [dbo].[Skin] (
    [SkinId]         INT IDENTITY (1, 1) NOT NULL,
    [SkinTypeId]     INT NULL,
    [SkinPackageId]  INT NOT NULL,
    [IsSkinSelected] BIT CONSTRAINT [DF_Skins_IsSelected] DEFAULT ((0)) NULL,
    [SkinStatus]     BIT CONSTRAINT [DF_Skins_SkinStatus] DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([SkinId] ASC)
);

