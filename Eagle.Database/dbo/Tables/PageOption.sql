CREATE TABLE [dbo].[PageOption] (
    [PageId]      INT              NOT NULL,
    [OptionId]    INT              IDENTITY (1, 1) NOT NULL,
    [OptionCode]  UNIQUEIDENTIFIER NOT NULL,
    [OptionName]  VARCHAR (64)     NOT NULL,
    [OptionValue] NVARCHAR (4000)  NULL,
    CONSTRAINT [PK__PageOpti__92C7A1FF9E34D648] PRIMARY KEY CLUSTERED ([OptionId] ASC)
);

