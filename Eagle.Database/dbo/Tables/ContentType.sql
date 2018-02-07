CREATE TABLE [dbo].[ContentType] (
    [ContentTypeId]   INT            IDENTITY (1, 1) NOT NULL,
    [ContentTypeName] NVARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([ContentTypeId] ASC)
);

