CREATE TABLE [dbo].[Currency] (
    [CurrencyId]   INT            IDENTITY (1, 1) NOT NULL,
    [CurrencyCode] NVARCHAR (50)  NOT NULL,
    [CurrencyName] NVARCHAR (250) NOT NULL,
    [IsActive]     BIT            NULL,
    [ModifiedDate] DATETIME       NULL,
    CONSTRAINT [PK_Currency_1] PRIMARY KEY CLUSTERED ([CurrencyId] ASC)
);

