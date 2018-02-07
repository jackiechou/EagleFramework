CREATE TABLE [Production].[Product_Album] (
    [ProductAlbumId] INT              IDENTITY (1, 1) NOT NULL,
    [AlbumId]        INT              NOT NULL,
    [ProductNo]      UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY CLUSTERED ([ProductAlbumId] ASC)
);

