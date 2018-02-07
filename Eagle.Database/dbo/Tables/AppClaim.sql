CREATE TABLE [dbo].[AppClaim] (
    [ClaimId]   UNIQUEIDENTIFIER NOT NULL,
    [Key]       NVARCHAR (500)   NULL,
    [Value]     NVARCHAR (500)   NULL,
    [GroupId]   UNIQUEIDENTIFIER NULL,
    [IsDeleted] BIT              NULL,
    CONSTRAINT [PK_dbo.AppClaim] PRIMARY KEY CLUSTERED ([ClaimId] ASC)
);

