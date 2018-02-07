--USE [5eagles_Framework]
--GO

/****** Object:  Table [dbo].[Documentation]    Script Date: 12/15/2017 8:05:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Documentation](
	[DocumentationId] [int] IDENTITY(1,1) NOT NULL,
	[VendorId] [int] NULL,
	[FileId] [int] NULL,
	[Status] [int] NOT NULL,
	[Ip] [varchar](30) NULL,
	[LastUpdatedIp] [varchar](30) NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedDate] [datetime] NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[LastModifiedByUserId] [uniqueidentifier] NULL,
 CONSTRAINT [PK__Document] PRIMARY KEY CLUSTERED 
(
	[DocumentationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Documentation] ADD  CONSTRAINT [DF_Document_Status]  DEFAULT ((1)) FOR [Status]
GO
