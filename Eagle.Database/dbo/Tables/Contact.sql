CREATE TABLE [dbo].[Contact] (
    [ContactId]         INT             IDENTITY (1, 1) NOT NULL,
    [ContactTypeId]     INT             NULL,
    [Title]             VARCHAR (40)    NULL,
    [FirstName]         NVARCHAR (256)  NULL,
    [LastName]          NVARCHAR (256)  NULL,
    [DisplayName]       NVARCHAR (128)  NULL,
    [Sex]               INT             NULL,
    [JobTitle]          NVARCHAR (256)  NULL,
    [Dob]               DATETIME        NULL,
    [PhotoThumbUrl]     NVARCHAR (4000) NULL,
    [PhotoThumbMiniUrl] NVARCHAR (4000) NULL,
    [LinePhone1]        INT             NULL,
    [LinePhone2]        INT             NULL,
    [Mobile]            VARCHAR (50)    NULL,
    [Fax]               VARCHAR (256)   NULL,
    [Email]             VARCHAR (256)   NULL,
    [Website]           VARCHAR (256)   NULL,
    CONSTRAINT [PK__UserCont__5C66259B5766F398] PRIMARY KEY CLUSTERED ([ContactId] ASC)
);

