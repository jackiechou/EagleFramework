create FUNCTION [dbo].[fn_GetFirstChar]
(
	-- Add the parameters for the function here
	@string nvarchar(4000)
)
RETURNS nvarchar(4000)
AS
BEGIN
	declare @result nvarchar(4000)

	declare @TableOfWords table(id int , word nvarchar(1000))
	declare @LenString int 
	declare @id int
	set @id=1
	while len( @string ) > 0 
	begin 
		select @LenString = 
		(case charindex( ' ', @string ) 
		when 0 then len( @string ) 
		else ( charindex( ' ', @string ) -1 )
		end
		) 

		insert into 
		@TableOfWords
		select 
		@id,substring( @string, 1, @LenString )

		set @id=@id+1

		select @string = 
		(case ( len( @string ) - @LenString )
		when 0 then '' 
		else right( @string, len( @string ) - @LenString - 1 ) 
		end
		) 
	end

	set @result = ''

	select
	@result = @result + left(word,1)
	from
	@TableOfWords
	SET @result=UPPER(@result)
	SET @result=REPLACE(@result,N'Á','A')
	SET @result=REPLACE(@result,N'À','A')
	SET @result=REPLACE(@result,N'Ả','A')
	SET @result=REPLACE(@result,N'Ã','A')
	SET @result=REPLACE(@result,N'Ạ','A')
	SET @result=REPLACE(@result,N'Ă','A')
	SET @result=REPLACE(@result,N'Ắ','A')
	SET @result=REPLACE(@result,N'Ằ','A')
	SET @result=REPLACE(@result,N'Ẳ','A')
	SET @result=REPLACE(@result,N'Ẵ','A')
	SET @result=REPLACE(@result,N'Ặ','A')
	SET @result=REPLACE(@result,N'Â','A')
	SET @result=REPLACE(@result,N'Ấ','A')
	SET @result=REPLACE(@result,N'Ầ','A')
	SET @result=REPLACE(@result,N'Ẩ','A')
	SET @result=REPLACE(@result,N'Ẫ','A')
	SET @result=REPLACE(@result,N'Ậ','A')
	SET @result=REPLACE(@result,N'É','E')
	SET @result=REPLACE(@result,N'È','E')
	SET @result=REPLACE(@result,N'Ẻ','E')
	SET @result=REPLACE(@result,N'Ẽ','E')
	SET @result=REPLACE(@result,N'Ẹ','E')
	SET @result=REPLACE(@result,N'Ê','E')
	SET @result=REPLACE(@result,N'Ế','E')
	SET @result=REPLACE(@result,N'Ề','E')
	SET @result=REPLACE(@result,N'Ể','E')
	SET @result=REPLACE(@result,N'Ễ','E')
	SET @result=REPLACE(@result,N'Ệ','E')
	SET @result=REPLACE(@result,N'Í','I')
	SET @result=REPLACE(@result,N'Ì','I')
	SET @result=REPLACE(@result,N'Ỉ','I')
	SET @result=REPLACE(@result,N'Ĩ','I')
	SET @result=REPLACE(@result,N'Ị','I')
	SET @result=REPLACE(@result,N'Ó','O')
	SET @result=REPLACE(@result,N'Ò','O')
	SET @result=REPLACE(@result,N'Ỏ','O')
	SET @result=REPLACE(@result,N'Õ','O')
	SET @result=REPLACE(@result,N'Ọ','O')
	SET @result=REPLACE(@result,N'Ô','O')
	SET @result=REPLACE(@result,N'Ố','O')
	SET @result=REPLACE(@result,N'Ồ','O')
	SET @result=REPLACE(@result,N'Ổ','O')
	SET @result=REPLACE(@result,N'Ỗ','O')
	SET @result=REPLACE(@result,N'Ộ','O')
	SET @result=REPLACE(@result,N'Ơ','O')
	SET @result=REPLACE(@result,N'Ớ','O')
	SET @result=REPLACE(@result,N'Ờ','O')
	SET @result=REPLACE(@result,N'Ở','O')
	SET @result=REPLACE(@result,N'Ỡ','O')
	SET @result=REPLACE(@result,N'Ợ','O')
	SET @result=REPLACE(@result,N'Ú','U')
	SET @result=REPLACE(@result,N'Ù','U')
	SET @result=REPLACE(@result,N'Ủ','U')
	SET @result=REPLACE(@result,N'Ũ','U')
	SET @result=REPLACE(@result,N'Ụ','U')
	SET @result=REPLACE(@result,N'Ư','U')
	SET @result=REPLACE(@result,N'Ứ','U')
	SET @result=REPLACE(@result,N'Ừ','U')
	SET @result=REPLACE(@result,N'Ử','U')
	SET @result=REPLACE(@result,N'Ữ','U')
	SET @result=REPLACE(@result,N'Ự','U')
	SET @result=REPLACE(@result,N'Ý','Y')
	SET @result=REPLACE(@result,N'Ỳ','Y')
	SET @result=REPLACE(@result,N'Ỷ','Y')
	SET @result=REPLACE(@result,N'Ỹ','Y')
	SET @result=REPLACE(@result,N'Ỵ','Y')
	RETURN UPPER(@result)
END


