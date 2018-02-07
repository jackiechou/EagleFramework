CREATE TABLE [dbo].[PageModuleSetting] (
    [PageModuleSettingId] INT             IDENTITY (1, 1) NOT NULL,
    [PageModuleId]        INT             NOT NULL,
    [SettingName]         NVARCHAR (50)   NOT NULL,
    [SettingValue]        NVARCHAR (2000) NOT NULL,
    PRIMARY KEY CLUSTERED ([PageModuleSettingId] ASC)
);

