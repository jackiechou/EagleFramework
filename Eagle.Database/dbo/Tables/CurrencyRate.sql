CREATE TABLE [dbo].[CurrencyRate] (
    [CurrencyRateId]   INT            IDENTITY (1, 1) NOT NULL,
    [CurrencyRateDate] DATETIME       NOT NULL,
    [FromCurrencyCode] NVARCHAR (250) NOT NULL,
    [ToCurrencyCode]   NVARCHAR (250) NOT NULL,
    [AverageRate]      DECIMAL (18)   NOT NULL,
    [EndOfDayRate]     DECIMAL (18)   NOT NULL,
    [ModifiedDate]     DATETIME       NOT NULL,
    CONSTRAINT [PK_CurrencyRate] PRIMARY KEY CLUSTERED ([CurrencyRateId] ASC)
);

