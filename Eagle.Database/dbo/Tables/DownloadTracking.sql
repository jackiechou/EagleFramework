CREATE TABLE [dbo].[DownloadTracking] (
    [DownloadId]       INT          IDENTITY (1, 1) NOT NULL,
    [DocumentType]     INT          NULL,
    [Code]             VARCHAR (30) NULL,
    [Status]           BIT          NOT NULL,
    [ExpiredDate]      DATETIME     NOT NULL,
    [PercentCompleted] INT          NULL,
    [FileId]           INT          NULL,
    CONSTRAINT [PK_DownloadTracking] PRIMARY KEY CLUSTERED ([DownloadId] ASC)
);

