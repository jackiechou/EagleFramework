CREATE TABLE [dbo].[Module] (
    [ApplicationId]          UNIQUEIDENTIFIER NULL,
    [ContentItemId]          INT              NULL,
    [ModuleId]               INT              IDENTITY (1, 1) NOT NULL,
    [ModuleCode]             VARCHAR (50)     CONSTRAINT [DF_Modules_ModuleCode] DEFAULT (newid()) NOT NULL,
    [ModuleName]             NVARCHAR (256)   NULL,
    [ModuleTitle]            NVARCHAR (256)   NOT NULL,
    [Description]            NVARCHAR (MAX)   NULL,
    [InheritViewPermissions] BIT              NULL,
    [Header]                 NVARCHAR (MAX)   NULL,
    [Footer]                 NVARCHAR (MAX)   NULL,
    [StartDate]              DATETIME         NULL,
    [EndDate]                DATETIME         NULL,
    [ViewOrder]              INT              NULL,
    [AllPages]               BIT              NULL,
    [IsSecured]              BIT              NULL,
    [IsActive]               BIT              NOT NULL,
    CONSTRAINT [PK__Modules__2B7477A73B025E1D] PRIMARY KEY CLUSTERED ([ModuleId] ASC)
);

