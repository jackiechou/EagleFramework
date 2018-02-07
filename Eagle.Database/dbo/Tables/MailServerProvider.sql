CREATE TABLE [dbo].[MailServerProvider] (
    [MailServerProviderId]   INT           IDENTITY (1, 1) NOT NULL,
    [MailServerProviderName] NVARCHAR (50) NOT NULL,
    [MailServerProtocol]     VARCHAR (10)  NOT NULL,
    [IncomingMailServerHost] NVARCHAR (50) NOT NULL,
    [IncomingMailServerPort] INT           NOT NULL,
    [OutgoingMailServerHost] NVARCHAR (50) NULL,
    [OutgoingMailServerPort] INT           NULL,
    [SSL]                    CHAR (1)      CONSTRAINT [DF__MailServerP__SSL__5FB337D6] DEFAULT ((0)) NULL,
    [TLS]                    CHAR (1)      CONSTRAINT [DF__MailServerP__TLS__60A75C0F] DEFAULT ((0)) NULL,
    CONSTRAINT [PK__MailServ__AF8965E7DFDD6E82] PRIMARY KEY CLUSTERED ([MailServerProviderId] ASC)
);

