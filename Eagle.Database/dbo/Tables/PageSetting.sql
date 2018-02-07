CREATE TABLE [dbo].[PageSetting] (
    [PageSettingId] INT             IDENTITY (1, 1) NOT NULL,
    [PageId]        INT             NOT NULL,
    [SettingName]   NVARCHAR (50)   NOT NULL,
    [SettingValue]  NVARCHAR (2000) NOT NULL,
    PRIMARY KEY CLUSTERED ([PageSettingId] ASC)
);

