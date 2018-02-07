CREATE TABLE [Production].[Product_File] (
    [ProductNo]     UNIQUEIDENTIFIER NOT NULL,
    [VendorId]      INT              NULL,
    [FileId]        INT              IDENTITY (1, 1) NOT NULL,
    [FileName]      NVARCHAR (128)   NOT NULL,
    [FileTitle]     NVARCHAR (128)   NOT NULL,
    [FileExtension] VARCHAR (128)    NOT NULL,
    [FileUrl]       NVARCHAR (254)   NOT NULL,
    [Description]   NVARCHAR (4000)  NOT NULL,
    [Status]        INT              NULL,
    [IsImage]       BIT              NULL,
    [Height]        INT              NULL,
    [Width]         INT              NULL,
    [ThumbHeight]   INT              NULL,
    [ThumbWidth]    INT              NULL,
    CONSTRAINT [PK__Product___0FFFC996621D86B8] PRIMARY KEY CLUSTERED ([FileId] ASC)
);

