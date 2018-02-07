create FUNCTION [dbo].[fn_MathMultiple] 
(
	-- Add the parameters for the function here
	@a int, @b int
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @tich int

	-- Add the T-SQL statements to compute the return value here
	SELECT @tich = @a * @b

	-- Return the result of the function
	RETURN @tich

END


