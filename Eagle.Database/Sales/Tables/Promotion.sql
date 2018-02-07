CREATE TABLE [Sales].[Promotion] (
    [VendorId]         INT            NULL,
    [PromotionId]      INT            IDENTITY (0, 1) NOT NULL,
    [PromotionValue]   MONEY          NOT NULL,
    [IsPercent]        BIT            NULL,
    [StartDate]        DATETIME       NULL,
    [EndDate]          DATETIME       NULL,
    [Description]      NVARCHAR (MAX) NULL,
    [CreatedDate]      DATETIME       NULL,
    [LastModifiedDate] DATETIME       NULL,
    [IsActive]         BIT            NULL,
    CONSTRAINT [PK__Promotio__52C42FCF348B2979] PRIMARY KEY CLUSTERED ([PromotionId] ASC)
);

