create FUNCTION [dbo].[fnNullIfEmptyGuid](@guidValue nvarchar(100))
RETURNS uniqueidentifier
AS
BEGIN       
    declare @result uniqueidentifier
    select @result = case when @guidValue = '' then null else CAST(@guidValue as uniqueidentifier) end
    RETURN @result
END


