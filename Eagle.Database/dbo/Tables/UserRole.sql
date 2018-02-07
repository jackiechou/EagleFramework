CREATE TABLE [dbo].[UserRole] (
    [UserRoleId]    INT              IDENTITY (1, 1) NOT NULL,
    [UserId]        UNIQUEIDENTIFIER NOT NULL,
    [RoleId]        UNIQUEIDENTIFIER NOT NULL,
    [ExpiryDate]    DATETIME         NULL,
    [EffectiveDate] DATETIME         NULL,
    [IsDefaultRole] BIT              NULL,
    [IsTrialUsed]   BIT              NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED ([UserRoleId] ASC)
);

