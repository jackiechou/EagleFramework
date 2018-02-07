CREATE TABLE [dbo].[ApplicationPage] (
    [ApplicationPageId] INT              IDENTITY (1, 1) NOT NULL,
    [PageId]            INT              NOT NULL,
    [ApplicationId]     UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY CLUSTERED ([ApplicationPageId] ASC)
);

