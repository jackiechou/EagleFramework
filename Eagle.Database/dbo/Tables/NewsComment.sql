CREATE TABLE [dbo].[NewsComment] (
    [NewsId]               INT              NULL,
    [CommentId]            INT              IDENTITY (1, 1) NOT NULL,
    [CommentName]          NVARCHAR (150)   NULL,
    [CommentEmail]         VARCHAR (150)    NULL,
    [CommentText]          VARCHAR (500)    NULL,
    [IsReplied]            BIT              NULL,
    [IsPublished]          BIT              NULL,
    [CreatedDate]          DATETIME         NULL,
    [LastModifiedDate]     DATETIME         NULL,
    [CreatedByUserId]      UNIQUEIDENTIFIER NULL,
    [LastModifiedByUserId] UNIQUEIDENTIFIER NULL,
    [Ip]                   VARCHAR (30)     NULL,
    [LastUpdatedIp]        VARCHAR (30)     NULL,
    PRIMARY KEY CLUSTERED ([CommentId] ASC)
);

