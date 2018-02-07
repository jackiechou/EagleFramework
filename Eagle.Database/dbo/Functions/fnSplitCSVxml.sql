-- XML split solution for comma-limited number list

 

-- Table-valued user-defined function - TVF

CREATE FUNCTION [dbo].[fnSplitCSVxml]

               (@NumberList VARCHAR(4096))

RETURNS @SplitList TABLE( ListMember INT)

AS

  BEGIN

    DECLARE  @xml XML

    SET @NumberList = LTRIM(RTRIM(@NumberList))

    IF LEN(@NumberList) = 0

      RETURN

    SET @xml = '' + REPLACE(@NumberList,',','') + ''

    INSERT INTO @SplitList

    SELECT x.i.value('.','VARCHAR(MAX)') AS Member

    FROM   @xml.nodes('//n') x(i)

    RETURN

  END



