CREATE FUNCTION [dbo].[ConcatenateCols](@Id INT)
RETURNS VARCHAR(MAX)
AS
BEGIN 
	DECLARE @RtnStr VARCHAR(MAX)
	SELECT @RtnStr = COALESCE(@RtnStr + ',','') + col
	FROM dbo.t
	WHERE id = @Id AND col > ''
	RETURN @RtnStr
END



