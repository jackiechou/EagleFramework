CREATE TABLE [dbo].[ContainerModule] (
    [ContainerModuleId] INT           IDENTITY (1, 1) NOT NULL,
    [ContainerId]       INT           NOT NULL,
    [ModuleId]          INT           NOT NULL,
    [PaneName]          NVARCHAR (50) NOT NULL,
    [ModuleOrder]       INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([ContainerModuleId] ASC)
);

