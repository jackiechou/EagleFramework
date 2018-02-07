CREATE TABLE [dbo].[UserAuthentication] (
    [UserAuthenticationId] INT             IDENTITY (1, 1) NOT NULL,
    [UserId]               INT             NOT NULL,
    [AuthenticationType]   NVARCHAR (100)  NOT NULL,
    [AuthenticationToken]  NVARCHAR (1000) NOT NULL,
    CONSTRAINT [PK_UserAuthentication] PRIMARY KEY CLUSTERED ([UserAuthenticationId] ASC)
);

