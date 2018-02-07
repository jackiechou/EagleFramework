CREATE FUNCTION [dbo].[fn_GiaiThua](@n int)
RETURNS bigint
AS
BEGIN
	DECLARE @k bigint SET @k=1
 	DECLARE @i int SET @i=1
 	WHILE @i<=@n
 	BEGIN
 		SET @k=@k*@i
 		SET @i=@i+1
 	END
 	RETURN @k
END

