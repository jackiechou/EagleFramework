CREATE TABLE [dbo].[PagePermission] (
    [PagePermissionId] INT            IDENTITY (1, 1) NOT NULL,
    [RoleId]           INT            NULL,
    [PageId]           INT            NULL,
    [UserIds]          NVARCHAR (MAX) NULL,
    [AllowAccess]      BIT            CONSTRAINT [DF_PagePermission_AllowAccess] DEFAULT ((1)) NULL,
    CONSTRAINT [PK__PagePerm__C298ABD182F89F0C] PRIMARY KEY CLUSTERED ([PagePermissionId] ASC)
);

