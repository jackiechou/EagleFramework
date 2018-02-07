create FUNCTION [dbo].[ConvertLongToIP](@Long bigint)
RETURNS varchar(15)
AS
BEGIN
	DECLARE @IP varchar(15)
	DECLARE @TempLong bigint
	DECLARE @Temp bigint

	SET @TempLong = @Long
	SET @Temp = @TempLong / (256 * 256 * 256)
	SET @TempLong = @TempLong - (@Temp * 256 * 256 * 256)
	SET @IP = CONVERT(varchar(3), @Temp) + '.'
	SET @Temp = @TempLong / (256 * 256)
	SET @TempLong = @TempLong - (@Temp * 256 * 256)
	SET @IP = @IP + CONVERT(varchar(3), @Temp) + '.'
	SET @Temp = @TempLong / 256
	SET @TempLong = @TempLong - (@Temp * 256)
	SET @IP = @IP + CONVERT(varchar(3), @Temp) + '.'
	SET @Temp = @TempLong
	SET @TempLong = @TempLong - @Temp
	SET @IP = @IP + CONVERT(varchar(3), @Temp)

	RETURN (@IP)
END 

