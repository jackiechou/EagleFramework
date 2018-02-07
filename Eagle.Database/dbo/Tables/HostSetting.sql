CREATE TABLE [dbo].[HostSetting] (
    [SettingId]     INT            IDENTITY (1, 1) NOT NULL,
    [ApplicationId] INT            NOT NULL,
    [SettingName]   NVARCHAR (50)  NOT NULL,
    [SettingValue]  NVARCHAR (256) NOT NULL,
    [IsSecure]      BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([SettingId] ASC)
);

