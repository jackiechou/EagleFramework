CREATE TABLE [dbo].[SkinPackageTemplate] (
    [SkinPackageId] INT            NOT NULL,
    [TemplateId]    INT            IDENTITY (1, 1) NOT NULL,
    [TemplateName]  NVARCHAR (250) NOT NULL,
    [TemplateKey]   NVARCHAR (250) NOT NULL,
    [TemplateSrc]   NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_SkinPackageTemplates] PRIMARY KEY CLUSTERED ([TemplateId] ASC)
);

