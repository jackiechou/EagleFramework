CREATE TABLE [dbo].[ApplicationModule] (
    [ApplicationModuleId] INT              IDENTITY (1, 1) NOT NULL,
    [ModuleId]            INT              NOT NULL,
    [ApplicationId]       UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY CLUSTERED ([ApplicationModuleId] ASC)
);

