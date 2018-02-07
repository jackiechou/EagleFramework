CREATE TABLE [dbo].[Role] (
    [ApplicationId]   UNIQUEIDENTIFIER NOT NULL,
    [SeqNo]           INT              IDENTITY (1, 1) NOT NULL,
    [RoleId]          UNIQUEIDENTIFIER CONSTRAINT [DF_Roles_RoleCode] DEFAULT (newid()) NOT NULL,
    [RoleName]        NVARCHAR (256)   NOT NULL,
    [LoweredRoleName] NVARCHAR (256)   NOT NULL,
    [Description]     NVARCHAR (256)   NULL,
    CONSTRAINT [PK__Roles__8AFACE1B19C90F4F] PRIMARY KEY NONCLUSTERED ([RoleId] ASC)
);

