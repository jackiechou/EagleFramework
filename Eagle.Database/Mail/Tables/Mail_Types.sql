CREATE TABLE [Mail].[Mail_Types] (
    [TypeId]          INT             NOT NULL,
    [PortalId]        INT             NOT NULL,
    [ParentId]        INT             NOT NULL,
    [CultureCode]     NVARCHAR (50)   NOT NULL,
    [Name]            NVARCHAR (255)  NOT NULL,
    [Description]     NVARCHAR (4000) NULL,
    [SortKey]         INT             NOT NULL,
    [Depth]           INT             NOT NULL,
    [PostedDate]      DATETIME        NULL,
    [LastUpdatedDate] DATETIME        NULL,
    [Status]          BIT             NOT NULL,
    CONSTRAINT [PK__Mail_Typ__516F03B51249A49B] PRIMARY KEY CLUSTERED ([TypeId] ASC)
);

