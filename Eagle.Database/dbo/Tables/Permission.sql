CREATE TABLE [dbo].[Permission] (
    [PermissionId]   INT          IDENTITY (1, 1) NOT NULL,
    [PermissionCode] VARCHAR (50) NOT NULL,
    [PermissionKey]  VARCHAR (50) NOT NULL,
    [PermissionName] VARCHAR (50) NOT NULL,
    [DisplayOrder]   INT          NOT NULL,
    [IsActive]       BIT          NULL,
    CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED ([PermissionId] ASC)
);

