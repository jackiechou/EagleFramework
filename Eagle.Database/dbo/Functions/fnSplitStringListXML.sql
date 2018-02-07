-- Split a comma-delimited string list with XML method

-- Table-valued user-defined function - TVF - SQL Server 2005/2008

-- Method uses XML xquery to split string list

CREATE FUNCTION [dbo].[fnSplitStringListXML] (

      @StringList NVARCHAR(MAX),

      @Delimiter CHAR(1))

 

RETURNS @TableList TABLE(ID int identity(1,1), StringLiteral NVARCHAR(128))

BEGIN

      IF @StringList = '' RETURN

      IF @Delimiter = ''

      BEGIN

         WITH Split AS

        ( SELECT CharOne=LEFT(@StringList,1),R=RIGHT(@StringList,len(@StringList)-1)

          UNION ALL

          SELECT LEFT(R,1), R=RIGHT(R,len(R)-1)

          FROM Split

          WHERE LEN(R)>0  )

        INSERT @TableList

        SELECT CharOne FROM Split

        OPTION ( MAXRECURSION 0)

        RETURN

      END -- IF

      DECLARE @XML xml

      SET @XML = '<root><csv>'+replace(@StringList,@Delimiter,'</csv><csv>')+

                 '</csv></root>'

      INSERT @TableList

      SELECT rtrim(ltrim(replace(Word.value('.','NVARCHAR(128)'),char(10),'')))

             AS ListMember

      FROM @XML.nodes('/root/csv') AS WordList(Word)

RETURN

END -- FUNCTION



