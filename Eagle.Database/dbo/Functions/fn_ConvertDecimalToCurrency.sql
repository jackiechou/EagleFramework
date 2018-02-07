CREATE FUNCTION [dbo].[fn_ConvertDecimalToCurrency]

              (@Amount DECIMAL(38,2))

RETURNS VARCHAR(64)

AS

  BEGIN

    DECLARE  @Result VARCHAR(64),

             @Buffer VARCHAR(64),

             @Comma  CHAR(1)

    SET @Result = ''

    SET @Comma = ''

    SET @Buffer = convert(VARCHAR(64),@Amount)

    SET @Result = right(@Buffer,3)

    SET @Buffer = left(@Buffer,len(@Buffer) - 3)

    WHILE (len(@Buffer) > 0)

      BEGIN

        SET @Result = left(convert(VARCHAR,convert(MONEY,reverse(

                                  left(reverse(@Buffer),12))),

                                   1),len(convert(VARCHAR,convert(MONEY,

                                   reverse(left(reverse(@Buffer),12))),

                                            1)) - 3) + @Comma + @Result

        SET @Buffer = CASE

                        WHEN len(@Buffer) > 12

                          THEN left(@Buffer,len(@Buffer) - 12)

                        ELSE ''

                      END

        SET @Comma = ','

      END

    RETURN REPLACE(@Result,' ','')

  END



