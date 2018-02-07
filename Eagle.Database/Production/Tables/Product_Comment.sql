CREATE TABLE [Production].[Product_Comment] (
    [CommentId]    INT              IDENTITY (1, 1) NOT NULL,
    [ProductNo]    UNIQUEIDENTIFIER NOT NULL,
    [CommentName]  NVARCHAR (250)   NULL,
    [CommentEmail] VARCHAR (150)    NOT NULL,
    [CommentText]  NVARCHAR (MAX)   NULL,
    [IsReply]      BIT              NOT NULL,
    [IsActive]     BIT              NULL,
    PRIMARY KEY CLUSTERED ([CommentId] ASC)
);

