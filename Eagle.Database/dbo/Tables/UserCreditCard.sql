CREATE TABLE [dbo].[UserCreditCard] (
    [ProfileId]    INT      NOT NULL,
    [CreditCardId] INT      NOT NULL,
    [ModifiedDate] DATETIME NOT NULL,
    PRIMARY KEY CLUSTERED ([ProfileId] ASC, [CreditCardId] ASC)
);

