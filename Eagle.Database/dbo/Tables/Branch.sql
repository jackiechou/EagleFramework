CREATE TABLE [dbo].[Branch] (
    [BranchId]   INT            IDENTITY (1, 1) NOT NULL,
    [BranchCode] NVARCHAR (3)   NOT NULL,
    [BranchName] NVARCHAR (100) NOT NULL,
    [TaxCode]    NVARCHAR (50)  NULL,
    [Phone]      NVARCHAR (50)  NULL,
    [HotLine]    NVARCHAR (50)  NULL,
    [Fax]        NVARCHAR (50)  NULL,
    [Address]    NVARCHAR (50)  NULL,
    [Status]     BIT            NULL,
    CONSTRAINT [PK_CMS_Branchs] PRIMARY KEY CLUSTERED ([BranchId] ASC)
);

