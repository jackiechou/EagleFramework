CREATE TABLE [dbo].[RoleGroupReference] (
    [Id]             INT              NOT NULL,
    [RoleId]         UNIQUEIDENTIFIER NOT NULL,
    [RoleGroupId]    UNIQUEIDENTIFIER NOT NULL,
    [IsDefaultGroup] BIT              NULL,
    CONSTRAINT [PK_RoleGroupReference] PRIMARY KEY CLUSTERED ([Id] ASC)
);

