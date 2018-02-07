CREATE TABLE [Sales].[Order_Payment] (
    [OrderPaymentId]  INT              IDENTITY (1, 1) NOT NULL,
    [OrderNo]         UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL,
    [PaymentMethodId] INT              NOT NULL,
    [PaymentCode]     VARCHAR (15)     NULL,
    [PaymentName]     NVARCHAR (150)   NULL,
    [PaymentNumber]   NVARCHAR (200)   NULL,
    [PaymentExpire]   NVARCHAR (200)   NULL,
    [PaymentStatus]   BIT              NULL,
    CONSTRAINT [PK_Order_Payment] PRIMARY KEY CLUSTERED ([OrderPaymentId] ASC)
);

