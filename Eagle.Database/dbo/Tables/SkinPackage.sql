CREATE TABLE [dbo].[SkinPackage] (
    [ApplicationId]    INT            NOT NULL,
    [SkinPackageId]    INT            IDENTITY (1, 1) NOT NULL,
    [SkinPackageName]  NVARCHAR (150) NOT NULL,
    [SkinPackageAlias] NVARCHAR (150) NULL,
    [SkinPackageSrc]   NVARCHAR (250) NOT NULL,
    PRIMARY KEY CLUSTERED ([SkinPackageId] ASC)
);

