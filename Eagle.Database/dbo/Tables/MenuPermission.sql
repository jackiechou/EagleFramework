CREATE TABLE [dbo].[MenuPermission] (
    [MenuPermissionId] INT              IDENTITY (1, 1) NOT NULL,
    [MenuId]           INT              NOT NULL,
    [RoleId]           UNIQUEIDENTIFIER NULL,
    [UserIds]          NVARCHAR (MAX)   NULL,
    [AllowAccess]      BIT              NOT NULL,
    CONSTRAINT [PK__MenuPerm__830F63C59EEC6F35] PRIMARY KEY CLUSTERED ([MenuPermissionId] ASC)
);

