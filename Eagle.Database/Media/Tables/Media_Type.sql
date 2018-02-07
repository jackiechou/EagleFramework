CREATE TABLE [Media].[Media_Type] (
    [TypeId]               INT              IDENTITY (1, 1) NOT NULL,
    [TypeName]             NVARCHAR (50)    NOT NULL,
    [TypeExt]              NVARCHAR (100)   NULL,
    [TypePath]             NVARCHAR (4000)  NULL,
    [ListOrder]            INT              NULL,
    [Description]          NVARCHAR (50)    NULL,
    [Status]               CHAR (1)         NULL,
    [CreatedOnDate]        DATETIME         NULL,
    [ModifiedDate]         DATETIME         NULL,
    [IPLog]                VARCHAR (30)     NULL,
    [IPModifiedLog]        VARCHAR (30)     NULL,
    [CreatedByUserId]      UNIQUEIDENTIFIER NULL,
    [LastModifiedByUserId] UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([TypeId] ASC)
);

