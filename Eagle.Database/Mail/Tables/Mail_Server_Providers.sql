CREATE TABLE [Mail].[Mail_Server_Providers] (
    [Mail_Server_Provider_Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Mail_Server_Provider_Name] NVARCHAR (50) NULL,
    [Mail_Server_Protocol]      VARCHAR (10)  NULL,
    [Incoming_Mail_Server_Host] NVARCHAR (50) NULL,
    [Incoming_Mail_Server_Port] INT           NULL,
    [Outgoing_Mail_Server_Host] NVARCHAR (50) NULL,
    [Outgoing_Mail_Server_Port] INT           NULL,
    [SSL]                       CHAR (1)      CONSTRAINT [DF_Mail_Incoming_Mail_Servers_Outgoing_Mail_Server_SSL] DEFAULT ((0)) NULL,
    [TLS]                       CHAR (1)      CONSTRAINT [DF_Mail_Incoming_Mail_Servers_Outgoing_Mail_Server_TLS] DEFAULT ((0)) NULL,
    CONSTRAINT [PK__Incoming__027A1BA06EE2037B] PRIMARY KEY CLUSTERED ([Mail_Server_Provider_Id] ASC)
);

