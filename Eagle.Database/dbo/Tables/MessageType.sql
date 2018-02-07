CREATE TABLE [dbo].[MessageType] (
    [MessageTypeId]   INT            NOT NULL,
    [MessageTypeName] NVARCHAR (250) NULL,
    [Description]     NVARCHAR (MAX) NULL,
    [Status]          BIT            NOT NULL,
    CONSTRAINT [PK_dbo.MessageType] PRIMARY KEY CLUSTERED ([MessageTypeId] ASC)
);

