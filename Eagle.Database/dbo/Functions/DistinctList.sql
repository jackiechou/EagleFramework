CREATE FUNCTION [dbo].[DistinctList]
(
@List NVARCHAR(MAX),
@Delim CHAR
)
RETURNS
NVARCHAR(MAX)
AS
BEGIN
DECLARE @ParsedList TABLE
(
Item NVARCHAR(MAX)
)
DECLARE @list1 NVARCHAR(MAX), @Pos INT, @rList NVARCHAR(MAX)
SET @list = LTRIM(RTRIM(@list)) + @Delim
SET @pos = CHARINDEX(@delim, @list, 1)
WHILE @pos > 0
BEGIN
SET @list1 = LTRIM(RTRIM(LEFT(@list, @pos - 1)))
IF @list1 <> ''
INSERT INTO @ParsedList VALUES (CAST(@list1 AS NVARCHAR(MAX)))
SET @list = SUBSTRING(@list, @pos+1, LEN(@list))
SET @pos = CHARINDEX(@delim, @list, 1)
END
SELECT @rlist = COALESCE(@rlist+',','') + item
FROM (SELECT DISTINCT Item FROM @ParsedList) t
RETURN @rlist
END


