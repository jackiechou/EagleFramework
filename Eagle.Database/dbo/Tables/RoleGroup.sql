CREATE TABLE [dbo].[RoleGroup] (
    [ApplicationId] UNIQUEIDENTIFIER NOT NULL,
    [GroupId]       INT              NOT NULL,
    [GroupName]     NVARCHAR (50)    NOT NULL,
    [Description]   NVARCHAR (200)   NULL,
    [IsActive]      BIT              NULL,
    CONSTRAINT [PK__RoleGrou__149AF36A1BFD0199] PRIMARY KEY CLUSTERED ([GroupId] ASC)
);

