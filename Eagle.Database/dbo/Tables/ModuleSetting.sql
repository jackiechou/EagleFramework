CREATE TABLE [dbo].[ModuleSetting] (
    [ModuleSettingId] INT             IDENTITY (1, 1) NOT NULL,
    [SettingName]     NVARCHAR (50)   NOT NULL,
    [SettingValue]    NVARCHAR (2000) NOT NULL,
    [ModuleId]        INT             NOT NULL,
    CONSTRAINT [PK__ModuleSe__60EBEC71CBBD2FE7] PRIMARY KEY CLUSTERED ([ModuleSettingId] ASC)
);

