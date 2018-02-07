CREATE TABLE [dbo].[BannerType] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [TypeName]    NVARCHAR (100) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Status]      BIT            NULL,
    CONSTRAINT [PK_BannerTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

