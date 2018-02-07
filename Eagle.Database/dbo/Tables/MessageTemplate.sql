CREATE TABLE [dbo].[MessageTemplate] (
    [MessageTypeId]   INT            NOT NULL,
    [TemplateId]      INT            IDENTITY (1, 1) NOT NULL,
    [TemplateName]    NVARCHAR (250) NOT NULL,
    [TemplateSubject] NVARCHAR (250) NULL,
    [TemplateContent] NVARCHAR (MAX) NOT NULL,
    [Status]          BIT            NULL,
    CONSTRAINT [PK__MailTemp__F87ADD27B31C7E38] PRIMARY KEY CLUSTERED ([TemplateId] ASC)
);

