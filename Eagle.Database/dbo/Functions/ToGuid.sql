create FUNCTION [dbo].[ToGuid](@input NVARCHAR(MAX))
RETURNS UNIQUEIDENTIFIER
AS
BEGIN
DECLARE @guid UNIQUEIDENTIFIER;
SET @guid = NULL;

-- If this is a valid GUID, try to convert it
IF(dbo.[IsValidGuid](@input) = 1)
BEGIN
DECLARE @guidString AS NVARCHAR(MAX);
SET @guidString = UPPER(LTRIM(RTRIM(REPLACE(@input, '-', ''))));
SET @guidString = STUFF(@guidString, 9, 0, '-')
SET @guidString = STUFF(@guidString, 14, 0, '-')
SET @guidString = STUFF(@guidString, 19, 0, '-')
SET @guidString = STUFF(@guidString, 24, 0, '-')

IF(@guidString IS NOT NULL)
SET @guid = CONVERT(UNIQUEIDENTIFIER, @guidString);
END

RETURN @guid;
END

