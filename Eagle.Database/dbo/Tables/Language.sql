CREATE TABLE [dbo].[Language] (
    [LanguageId]   INT           IDENTITY (1, 1) NOT NULL,
    [LanguageCode] VARCHAR (5)   NOT NULL,
    [LanguageName] NVARCHAR (50) NOT NULL,
    [Description]  NVARCHAR (50) NULL,
    [Status]       BIT           NOT NULL,
    [ModifiedDate] DATETIME      NULL,
    CONSTRAINT [PK_Languages] PRIMARY KEY CLUSTERED ([LanguageCode] ASC)
);

