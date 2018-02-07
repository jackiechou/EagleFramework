CREATE TABLE [Sales].[PaymentMethod] (
    [PaymentMethodId]   INT           IDENTITY (1, 1) NOT NULL,
    [PaymentMethodName] VARCHAR (255) NOT NULL,
    [PaymentMethodCode] VARCHAR (8)   NOT NULL,
    [IsCreditCard]      BIT           NOT NULL,
    [IsActive]          INT           NULL,
    CONSTRAINT [PK__Payment___781302A66033F171] PRIMARY KEY CLUSTERED ([PaymentMethodId] ASC)
);

