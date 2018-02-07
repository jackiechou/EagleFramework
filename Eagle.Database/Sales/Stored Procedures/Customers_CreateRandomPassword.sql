CREATE PROC [Sales].[Customers_CreateRandomPassword]
(
	@len int = 8, --Length of the password to be generated
	@password_type char(7) = 'simple',
	@o_return varchar(30) output
--Default is to generate a simple password with lowecase letters. 
--Pass anything other than 'simple' to generate a complex password. 
--The complex password includes numbers, special characters, upper case and lower case letters
)
AS
BEGIN
	DECLARE @password varchar(25),@type tinyint, @bitmap char(12)	
	SET @password='' 
	SET @bitmap = 'UAEIOYuaeioy' 
	--@bitmap contains all the vowels, which are a, e, i, o, u and y. These vowels are used to generate slightly readable/rememberable simple passwords

	WHILE @len > 0
	BEGIN
		IF @password_type = 'simple' --Generating a simple password
		BEGIN
		IF (@len%2) = 0  --Appending a random vowel to @password
			
			SET @password = @password + SUBSTRING(@bitmap,CONVERT(int,ROUND(1 + (RAND() * (5)),0)),1)
		ELSE --Appending a random alphabet
			SET @password = @password + CHAR(ROUND(97 + (RAND() * (25)),0))
			
		END
		ELSE --Generating a complex password
		BEGIN
			SET @type = ROUND(1 + (RAND() * (3)),0)

			IF @type = 1 --Appending a random lower case alphabet to @password
				SET @password = @password + CHAR(ROUND(97 + (RAND() * (25)),0))
			ELSE IF @type = 2 --Appending a random upper case alphabet to @password
				SET @password = @password + CHAR(ROUND(65 + (RAND() * (25)),0))
			ELSE IF @type = 3 --Appending a random number between 0 and 9 to @password
				SET @password = @password + CHAR(ROUND(48 + (RAND() * (9)),0))
			ELSE IF @type = 4 --Appending a random special character to @password
				SET @password = @password + CHAR(ROUND(33 + (RAND() * (13)),0))
		END

		SET @len = @len - 1
	END		
	set @o_return = (select @password)
END

