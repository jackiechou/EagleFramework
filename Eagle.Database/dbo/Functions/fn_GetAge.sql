create FUNCTION [dbo].[fn_GetAge](@BirthDate datetime)
RETURNS int
AS 
Begin
	declare @now datetime
	set @now = getdate()
	declare @age int 
	set @age = Year(@now) - Year(@BirthDate);
    
	if (Month(@now) < Month(@BirthDate) Or (Month(@now) = Month(@BirthDate) and Day(@now) < Day(@BirthDate)))
		set @age = @age-1
	
	RETURN @age
end


