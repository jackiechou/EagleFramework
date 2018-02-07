
CREATE FUNCTION [dbo].[fnNumberToEnglish]   (@Number  INT)

RETURNS VARCHAR(1024)

AS

  BEGIN

    DECLARE  @Below20  TABLE( ID   INT    IDENTITY ( 0 , 1 ),

                              Word VARCHAR(32)   )

    DECLARE  @Tens  TABLE( ID   INT    IDENTITY ( 2 , 1 ),

                              Word VARCHAR(32)   )

    INSERT @Below20 (Word)

    VALUES('Zero'),

          ('One'),

          ('Two'),

          ('Three'),

          ('Four'),

          ('Five'),

          ('Six'),

          ('Seven'),

          ('Eight'),

          ('Nine'),

          ('Ten'),

          ('Eleven'),

          ('Twelve'),

          ('Thirteen'),

          ('Fourteen'),

          ('Fifteen'),

          ('Sixteen'),

          ('Seventeen'),

          ('Eighteen'),

          ('Nineteen')

    

    INSERT @Tens     VALUES('Twenty'),

          ('Thirty'),

          ('Forty'),

          ('Fifty'),

          ('Sixty'),

          ('Seventy'),

          ('Eighty'),

          ('Ninety')

    

    DECLARE  @English VARCHAR(1024) = (SELECT CASE

                             WHEN @Number = 0 THEN ''

                             WHEN @Number BETWEEN 1 AND 19 THEN

                             (SELECT Word

                                     FROM   @Below20

                                    WHERE  ID = @Number)

                             WHEN @Number BETWEEN 20 AND 99 THEN

                             (SELECT Word

                                     FROM   @Tens

   WHERE  ID = @Number / 10) + '-' + dbo.fnNumberToEnglish(@Number%10)

   WHEN @Number BETWEEN 100 AND 999 THEN (dbo.fnNumberToEnglish(@Number / 100))

     + ' Hundred ' + dbo.fnNumberToEnglish(@Number%100)

   WHEN @Number BETWEEN 1000 AND 999999

     THEN (dbo.fnNumberToEnglish(@Number / 1000)) + ' Thousand '

     + dbo.fnNumberToEnglish(@Number%1000)

   ELSE ' INVALID INPUT'   END)

    SELECT @English = RTRIM(@English)

    SELECT @English = RTRIM(LEFT(@English,len(@English) - 1))

    WHERE  RIGHT(@English,1) = '-'

    RETURN (@English)

  END



