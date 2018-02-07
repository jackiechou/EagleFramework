CREATE TABLE [dbo].[SkinPackageBackground] (
    [SkinBackgroundId] INT            IDENTITY (1, 1) NOT NULL,
    [SkinPackageId]    INT            NULL,
    [BackgroundName]   NVARCHAR (256) NOT NULL,
    [BackgroundLink]   NVARCHAR (MAX) NOT NULL,
    [FileId]           INT            NULL,
    [ListOrder]        INT            NULL,
    [IsExternalLink]   BIT            NULL,
    [IsActive]         BIT            NULL,
    CONSTRAINT [PK__SkinPack__A39ECC1260D791EC] PRIMARY KEY CLUSTERED ([SkinBackgroundId] ASC)
);

