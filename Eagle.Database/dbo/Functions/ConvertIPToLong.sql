create FUNCTION [dbo].[ConvertIPToLong](@IP varchar(15))
RETURNS bigint
AS
BEGIN
	DECLARE @Long bigint
	SET @Long = CONVERT(bigint, PARSENAME(@IP, 4)) * 256 * 256 * 256 +
		CONVERT(bigint, PARSENAME(@IP, 3)) * 256 * 256 +
		CONVERT(bigint, PARSENAME(@IP, 2)) * 256 +
		CONVERT(bigint, PARSENAME(@IP, 1))

	RETURN (@Long)
END

