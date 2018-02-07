CREATE TABLE [dbo].[ModuleControls] (
    [ModuleControlId] INT            IDENTITY (1, 1) NOT NULL,
    [ModuleId]        INT            NOT NULL,
    [ControlTitle]    NVARCHAR (50)  NULL,
    [ControlKey]      NVARCHAR (50)  NULL,
    [ControlSrc]      NVARCHAR (256) NULL,
    [ControlType]     INT            NOT NULL,
    [IconFile]        NVARCHAR (100) NULL,
    [ViewOrder]       INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([ModuleControlId] ASC)
);

