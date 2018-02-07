CREATE TABLE [dbo].[Department] (
    [BranchId]       INT            NOT NULL,
    [DepartmentId]   INT            IDENTITY (1, 1) NOT NULL,
    [DepartmentCode] NVARCHAR (3)   NULL,
    [DepartmentName] NVARCHAR (100) NOT NULL,
    [ParentId]       INT            CONSTRAINT [DF_CMS_Departments_Department_ParentID] DEFAULT ((0)) NULL,
    [AddressId]      INT            NULL,
    [Status]         BIT            NULL,
    CONSTRAINT [PK_IBMS_Department] PRIMARY KEY CLUSTERED ([DepartmentId] ASC)
);

