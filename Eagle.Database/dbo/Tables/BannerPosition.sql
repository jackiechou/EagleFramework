CREATE TABLE [dbo].[BannerPosition] (
    [PositionId]   INT              IDENTITY (1, 1) NOT NULL,
    [PositionCode] UNIQUEIDENTIFIER NULL,
    [PositionName] NVARCHAR (100)   NOT NULL,
    [ListOrder]    INT              NULL,
    [Description]  NVARCHAR (250)   NULL,
    [Status]       BIT              NOT NULL,
    CONSTRAINT [PK_BannerPositions] PRIMARY KEY CLUSTERED ([PositionId] ASC)
);

