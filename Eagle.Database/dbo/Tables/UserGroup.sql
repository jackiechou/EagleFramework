CREATE TABLE [dbo].[UserGroup] (
    [UserGroupId]    INT              IDENTITY (1, 1) NOT NULL,
    [GroupId]        UNIQUEIDENTIFIER NOT NULL,
    [UserId]         UNIQUEIDENTIFIER NOT NULL,
    [EffectiveDate]  DATETIME         NULL,
    [ExpiryDate]     DATETIME         NULL,
    [IsDefaultGroup] BIT              NULL,
    CONSTRAINT [PK_UserGroup] PRIMARY KEY CLUSTERED ([UserGroupId] ASC)
);

