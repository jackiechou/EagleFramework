create FUNCTION [dbo].[CountSunday]
	(@fromdate datetime ,
	 @todate datetime )
RETURNS int
AS 
Begin
	DECLARE @range int 
	DECLARE @count int 
 
	SET @range = DATEDIFF(d, @fromdate, @todate) + 1 
	SELECT @count= @range / 7 * 1 + @range % 7 - 
		(SELECT COUNT(*) FROM ( SELECT 1 AS d UNION ALL 
								SELECT 2 UNION ALL 
								SELECT 3 UNION ALL 
								SELECT 4 UNION ALL 
								SELECT 5 UNION ALL 
								SELECT 6 UNION ALL 
								SELECT 7 ) weekdays 
		 WHERE d <= @range % 7 AND DATENAME(WEEKDAY, @todate - d + 1) IN ('Saturday', 'monday', 'tuesday', 'wednesday', 'thursday', 'friday')) 
	RETURN @count
END


