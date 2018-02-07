CREATE TABLE [dbo].[ModuleHtmlText] (
    [ItemId]     INT              IDENTITY (1, 1) NOT NULL,
    [ModuleCode] UNIQUEIDENTIFIER NULL,
    [NewsCode]   UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([ItemId] ASC)
);

