CREATE TABLE [dbo].[CreditCard] (
    [CreditCardId]   INT           IDENTITY (1, 1) NOT NULL,
    [CreditCardName] NVARCHAR (50) NULL,
    [CreditCardCode] NVARCHAR (50) NULL,
    [CardType]       NVARCHAR (50) NOT NULL,
    [CardNumber]     NVARCHAR (25) NOT NULL,
    [ExpiredMonth]   INT           NOT NULL,
    [ExpiredYear]    INT           NOT NULL,
    [ModifiedDate]   DATETIME      NOT NULL,
    CONSTRAINT [PK_CreditCard_CreditCardID] PRIMARY KEY CLUSTERED ([CreditCardId] ASC)
);

