CREATE TABLE [Mail].[Mail_Templates] (
    [Mail_Template_Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Mail_Template_Name]         NVARCHAR (250) NULL,
    [Mail_Template_Content]      NVARCHAR (MAX) NULL,
    [Mail_Template_Discontinued] BIT            NULL,
    [Mail_Type_Id]               INT            NULL,
    CONSTRAINT [PK__Email_Te__E7FB8F212B2BF7AC] PRIMARY KEY CLUSTERED ([Mail_Template_Id] ASC)
);

