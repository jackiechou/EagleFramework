CREATE TABLE [dbo].[Event] (
    [VendorId]        INT            NULL,
    [EventId]         CHAR (32)      NOT NULL,
    [EventCode]       NVARCHAR (256) NOT NULL,
    [EventType]       NVARCHAR (256) NOT NULL,
    [EventTitle]      NVARCHAR (256) NULL,
    [EventMessage]    NVARCHAR (MAX) NULL,
    [StartDate]       DATETIME       NULL,
    [EndDate]         DATETIME       NULL,
    [CreatedDate]     DATETIME       NULL,
    [TimeZoneId]      NVARCHAR (256) NULL,
    [EventStatus]     BIT            NULL,
    [EventSequence]   DECIMAL (19)   NOT NULL,
    [EventOccurrence] DECIMAL (19)   NOT NULL,
    [Location]        NVARCHAR (MAX) NULL,
    [Latitude]        DECIMAL (18)   NULL,
    [Longitude]       DECIMAL (18)   NULL,
    [PhotoSmallId]    INT            NULL,
    [PhotoLargeId]    INT            NULL,
    CONSTRAINT [PK__Events__7944C810C0A41A3E] PRIMARY KEY CLUSTERED ([EventId] ASC)
);

