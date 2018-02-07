CREATE TABLE [dbo].[ModulePermission] (
    [ModulePermissionId] INT              IDENTITY (1, 1) NOT NULL,
    [RoleId]             UNIQUEIDENTIFIER NULL,
    [ModuleId]           INT              NULL,
    [CapabilityId]       INT              NULL,
    [UserIds]            NVARCHAR (MAX)   NULL,
    [AllowAccess]        BIT              CONSTRAINT [DF_ModulePermissions_AllowAccess] DEFAULT ((1)) NULL,
    CONSTRAINT [PK__ModulePe__3615A201E5C88A0E] PRIMARY KEY CLUSTERED ([ModulePermissionId] ASC)
);

