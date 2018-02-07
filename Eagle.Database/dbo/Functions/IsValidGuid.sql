create FUNCTION [dbo].[IsValidGuid](@input NVARCHAR(MAX))
RETURNS bit
AS
BEGIN
DECLARE @isValidGuid BIT;
SET @isValidGuid = 0;
SET @input = UPPER(LTRIM(RTRIM(REPLACE(@input, '-', ''))));

IF(@input IS NOT NULL AND LEN(@input) = 32)
BEGIN
DECLARE @indexChar NCHAR(1)
DECLARE @index INT;
SET @index = 1;
WHILE (@index <= 32)
BEGIN
SET @indexChar = SUBSTRING(@input, @index, 1);
IF (ISNUMERIC(@indexChar) = 1 OR @indexChar IN ('A', 'B', 'C', 'D', 'E', 'F'))
SET @index = @index + 1;
ELSE
BREAK;   
END

IF(@index = 33)
SET @isValidGuid = 1;
END

RETURN @isValidGuid;
END

