CREATE TABLE [dbo].[Application] (
    [ApplicationId]    UNIQUEIDENTIFIER NOT NULL,
    [SeqNo]            INT              IDENTITY (1, 1) NOT NULL,
    [ApplicationName]  NVARCHAR (256)   NOT NULL,
    [DefaultLanguage]  NVARCHAR (10)    NOT NULL,
    [HomeDirectory]    VARCHAR (100)    NOT NULL,
    [Currency]         CHAR (3)         NULL,
    [TimeZoneOffset]   NVARCHAR (50)    NULL,
    [Url]              NVARCHAR (MAX)   NULL,
    [LogoFile]         NVARCHAR (50)    NULL,
    [BackgroundFile]   NVARCHAR (50)    NULL,
    [KeyWords]         NVARCHAR (500)   NULL,
    [CopyRight]        NVARCHAR (250)   NULL,
    [FooterText]       NVARCHAR (100)   NULL,
    [Description]      NVARCHAR (500)   NULL,
    [HostSpace]        INT              NULL,
    [HostFee]          MONEY            NULL,
    [ExpiryDate]       DATETIME         NULL,
    [RegisteredUserId] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK__Applicat__C93A4C9875A4BA1E] PRIMARY KEY NONCLUSTERED ([ApplicationId] ASC)
);

