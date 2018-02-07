CREATE TABLE [Mail].[Mail_Accounts] (
    [Mail_Account_Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Mail_Server_Provider_Id] INT            NOT NULL,
    [Mail_Sender_Name]        NVARCHAR (250) NOT NULL,
    [Mail_Contact_Name]       NVARCHAR (250) NULL,
    [Mail_Address]            VARCHAR (200)  NOT NULL,
    [Mail_Account]            VARCHAR (200)  NOT NULL,
    [Mail_Password]           VARCHAR (20)   NOT NULL,
    [Mail_Signature]          NVARCHAR (MAX) NULL,
    CONSTRAINT [PK__Mail_Acc__13882DFB637050CF] PRIMARY KEY CLUSTERED ([Mail_Account_Id] ASC)
);

