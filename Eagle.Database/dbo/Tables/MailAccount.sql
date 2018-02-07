CREATE TABLE [dbo].[MailAccount] (
    [MailAccountId]        INT            IDENTITY (1, 1) NOT NULL,
    [MailServerProviderId] INT            NOT NULL,
    [SenderName]           NVARCHAR (250) NOT NULL,
    [ContactName]          NVARCHAR (250) NULL,
    [MailAddress]          VARCHAR (200)  NOT NULL,
    [Password]             VARCHAR (20)   NOT NULL,
    [Signature]            NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([MailAccountId] ASC)
);

