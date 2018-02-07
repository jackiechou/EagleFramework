-- Split a comma-delimited string list
-- Table-valued user-defined function - TVF

CREATE FUNCTION [dbo].[fnSplitStringList] (@StringList NVARCHAR(MAX))

RETURNS @TableList TABLE( StringLiteral NVARCHAR(128))

BEGIN

    DECLARE @StartPointer INT, @EndPointer INT

    SELECT @StartPointer = 1, @EndPointer = CHARINDEX(',', @StringList)

    WHILE (@StartPointer < LEN(@StringList) + 1)

    BEGIN

        IF @EndPointer = 0

            SET @EndPointer = LEN(@StringList) + 1

        INSERT INTO @TableList (StringLiteral)

        VALUES(LTRIM(RTRIM(SUBSTRING(@StringList, @StartPointer,

                                     @EndPointer - @StartPointer))))

        SET @StartPointer = @EndPointer + 1

        SET @EndPointer = CHARINDEX(',', @StringList, @StartPointer)

    END -- WHILE

    RETURN

END -- FUNCTION



