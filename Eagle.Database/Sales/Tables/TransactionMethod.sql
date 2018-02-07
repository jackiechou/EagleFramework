CREATE TABLE [Sales].[TransactionMethod] (
    [TransactionMethodId]   INT            IDENTITY (1, 1) NOT NULL,
    [TransactionMethodName] NVARCHAR (250) NOT NULL,
    [TransactionMethodFee]  MONEY          NULL,
    [IsActive]              BIT            NULL,
    CONSTRAINT [PK_TransactionMethod] PRIMARY KEY CLUSTERED ([TransactionMethodId] ASC)
);

