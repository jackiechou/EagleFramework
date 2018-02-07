CREATE TABLE [dbo].[UserProfile] (
    [ProfileId] UNIQUEIDENTIFIER NOT NULL,
    [UserId]    UNIQUEIDENTIFIER NOT NULL,
    [ContactId] INT              NULL,
    CONSTRAINT [PK__UserProf__1788CC4C5DB6C8B7] PRIMARY KEY CLUSTERED ([ProfileId] ASC)
);

