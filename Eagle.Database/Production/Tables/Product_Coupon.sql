CREATE TABLE [Production].[Product_Coupon] (
    [VendorId]         INT             NOT NULL,
    [CultureCode]      NCHAR (10)      NOT NULL,
    [CouponId]         INT             IDENTITY (1, 1) NOT NULL,
    [CouponCode]       VARCHAR (32)    NOT NULL,
    [CouponType]       TINYINT         NOT NULL,
    [CouponName]       NVARCHAR (500)  NOT NULL,
    [CouponAmount]     DECIMAL (12, 2) NOT NULL,
    [IsPercent]        BIT             NULL,
    [Description]      NVARCHAR (MAX)  NULL,
    [StartDate]        DATETIME        NULL,
    [EndDate]          DATETIME        NULL,
    [Status]           INT             NULL,
    [CreatedDate]      DATETIME        NULL,
    [LastModifiedDate] DATETIME        NULL,
    CONSTRAINT [PK__Product___2A776B9C54EC22B6] PRIMARY KEY CLUSTERED ([CouponId] ASC)
);

