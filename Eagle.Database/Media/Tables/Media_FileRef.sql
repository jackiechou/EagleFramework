CREATE TABLE [Media].[Media_FileRef] (
    [FileRefId]  INT IDENTITY (1, 1) NOT NULL,
    [FileId]     INT NULL,
    [AlbumId]    INT NULL,
    [ArtistId]   INT NULL,
    [ComposerId] INT NULL,
    [PlayListId] INT NULL,
    [TopicId]    INT NULL,
    [TypeId]     INT NULL,
    [VendorId]   INT NULL,
    CONSTRAINT [PK__FileRefs__ABD9997210C21A33] PRIMARY KEY CLUSTERED ([FileRefId] ASC)
);

