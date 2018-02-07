CREATE TABLE [Production].[Product_Vote] (
    [ProductNo] UNIQUEIDENTIFIER NOT NULL,
    [VoteId]    INT              IDENTITY (1, 1) NOT NULL,
    [Votes]     NVARCHAR (MAX)   NOT NULL,
    [AllVotes]  INT              NOT NULL,
    [Rating]    INT              NOT NULL,
    [LastIp]    NVARCHAR (MAX)   NOT NULL,
    CONSTRAINT [PK__Product___52F015C260C9E849] PRIMARY KEY CLUSTERED ([VoteId] ASC)
);

