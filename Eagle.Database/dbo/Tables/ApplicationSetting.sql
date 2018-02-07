CREATE TABLE [dbo].[ApplicationSetting] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [ApplicationId] INT             NOT NULL,
    [SettingName]   NVARCHAR (50)   NOT NULL,
    [SettingValue]  NVARCHAR (2000) NULL,
    [IsSecure]      BIT             NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

