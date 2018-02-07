CREATE TABLE [dbo].[ModuleCapability] (
    [CapabilityId]   INT            NOT NULL,
    [ModuleId]       INT            NULL,
    [CapabilityName] NVARCHAR (150) NULL,
    [DispalyName]    NVARCHAR (150) NULL,
    [DisplayOrder]   INT            NOT NULL,
    [Description]    NVARCHAR (255) NULL,
    [IsActive]       BIT            NULL,
    CONSTRAINT [PK__ModuleCa__9A6A78D6B49D1381] PRIMARY KEY CLUSTERED ([CapabilityId] ASC)
);

