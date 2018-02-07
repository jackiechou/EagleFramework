CREATE TABLE [dbo].[PermissionGroup] (
    [PermissionGroupId]   INT            IDENTITY (1, 1) NOT NULL,
    [PermissionGroupCode] VARCHAR (50)   NOT NULL,
    [PermissionGroupName] NVARCHAR (250) NULL,
    PRIMARY KEY CLUSTERED ([PermissionGroupId] ASC)
);

