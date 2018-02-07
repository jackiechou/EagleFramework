CREATE TABLE [dbo].[VisitorStatistics] (
    [VisitorStatisticId] INT      IDENTITY (1, 1) NOT NULL,
    [ApplicationId]      INT      NULL,
    [TotalVisitorsInDay] INT      NULL,
    [CreatedOnDate]      DATETIME NULL,
    CONSTRAINT [PK__VisitorS__49893A585CB537A0] PRIMARY KEY CLUSTERED ([VisitorStatisticId] ASC)
);

