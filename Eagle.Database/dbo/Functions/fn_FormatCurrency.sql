CREATE FUNCTION [dbo].[fn_FormatCurrency]
(@MoneyAmount MONEY)
RETURNS VARCHAR(24)
AS
  BEGIN
    RETURN (convert(VARCHAR,@MoneyAmount,1))
  END



