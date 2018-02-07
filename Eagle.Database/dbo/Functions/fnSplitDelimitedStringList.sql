-- Split delimited string

CREATE FUNCTION [dbo].[fnSplitDelimitedStringList]

               (@StringList NVARCHAR(MAX),

                @Delimiter  NVARCHAR(5))

RETURNS @TableList TABLE(ID INT IDENTITY(1,1), StringLiteral NVARCHAR(1024))

  BEGIN

    DECLARE  @StartPointer INT,

             @EndPointer   INT

    SELECT @StartPointer = 1,

           @EndPointer = CHARINDEX(@Delimiter,@StringList)

    WHILE (@StartPointer < LEN(@StringList) + 1)

      BEGIN

        IF @EndPointer = 0

          SET @EndPointer = LEN(@StringList) + 1

        INSERT INTO @TableList

                   (StringLiteral)

        VALUES     (LTRIM(RTRIM(SUBSTRING(@StringList,@StartPointer,

                   @EndPointer - @StartPointer))))

        SET @StartPointer = @EndPointer + 1

        SET @EndPointer = CHARINDEX(@Delimiter,@StringList,@StartPointer)

      END -- WHILE

    RETURN

  END -- FUNCTION


