CREATE TABLE [dbo].[ApplicationLanguage] (
    [ApplicationLanguageId] INT              IDENTITY (1, 1) NOT NULL,
    [ApplicationId]         UNIQUEIDENTIFIER NOT NULL,
    [LanguageCode]          VARCHAR (50)     NOT NULL,
    [Status]                BIT              NULL,
    CONSTRAINT [PK__Applicat__3214EC07236119A5] PRIMARY KEY CLUSTERED ([ApplicationLanguageId] ASC)
);

