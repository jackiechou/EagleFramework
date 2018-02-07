CREATE TABLE [dbo].[RoleGroupClaim] (
    [Id]      INT              IDENTITY (1, 1) NOT NULL,
    [GroupId] UNIQUEIDENTIFIER NOT NULL,
    [ClaimId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_RoleGroupClaim] PRIMARY KEY CLUSTERED ([Id] ASC)
);

