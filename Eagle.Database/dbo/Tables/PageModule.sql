CREATE TABLE [dbo].[PageModule] (
    [PageModuleId]       INT           IDENTITY (1, 1) NOT NULL,
    [PageId]             INT           NOT NULL,
    [ModuleId]           INT           NOT NULL,
    [Pane]               NVARCHAR (50) NULL,
    [Alignment]          NVARCHAR (10) NULL,
    [Color]              NVARCHAR (20) NULL,
    [Border]             NVARCHAR (10) NULL,
    [InsertedPosition]   NVARCHAR (50) NULL,
    [ReferencedModuleId] INT           NULL,
    [IconClass]          NVARCHAR (50) NULL,
    [IconFile]           INT           NULL,
    [ModuleOrder]        INT           NULL,
    [IsVisible]          BIT           NULL,
    CONSTRAINT [PK__PageModu__2729F7ACF2B40BD1] PRIMARY KEY CLUSTERED ([PageModuleId] ASC)
);

