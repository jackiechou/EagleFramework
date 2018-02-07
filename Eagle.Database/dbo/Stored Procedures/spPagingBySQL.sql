create PROCEDURE [dbo].[spPagingBySQL]
(@total int,
@current_page int ,
@page_size int,
@row_per_page int)
AS
BEGIN
	DECLARE  @page_number int SET @page_number=1
	DECLARE @i int
	SET @i=1
	DECLARE @total_page int
	IF @total%@row_per_page>0
	SET @total_page=(@total/@row_per_page)+1
	ELSE
	SET @total_page=@total/@row_per_page 
	DECLARE @start int SET @start=0
	DECLARE @sql nvarchar(4000)
	SET @sql=''
	IF @current_page<=@total_page
	BEGIN
		-- Xử lý trường hợp @current_page=1
		IF @current_page=1
		BEGIN
			SET @sql=@sql+ N'Trang '
			SET @page_number=@page_size
			IF @page_number>@total_page SET @page_number=@total_page
			SET @start=1
		END
		ELSE
		BEGIN
			SET @sql=@sql+ N' <a href="?p=1">Trang đầu</a>'
			SET @sql=@sql+ ' <a href="?p='+ 
				Cast((@current_page-1) AS nvarchar(4))+N'">Trang trước</a>'
			-- Xử lý trường hợp (@total_page-@current_page)<@page_size/2
			IF(@total_page-@current_page)<@page_size/2
			   BEGIN
				  SET @start=(@total_page-@page_size)+1
				  IF @start<=0 SET @start=1 
				  SET @page_number = @total_page
			   END
			ELSE
			BEGIN
				IF (@current_page-(@page_size/2))=0
				BEGIN
					SET @start=1
					SET @page_number=@current_page+(@page_size/2)+1
					IF @total_page<@page_number
						SET @page_number=@total_page
				END
				ELSE
				   BEGIN
					  SET @start=@current_page-(@page_size/2)
					  IF @start<=0 SET @start=1 
					  SET @page_number=@current_page+(@page_size/2)
					  IF @total_page<@page_number
						  SET @page_number=@total_page
					  ELSE
					  IF @page_number <@page_size 
						  SET @page_number=@page_size
				   END
			END
		 END	
		 
		SET @i=@start
		WHILE @i<=@page_number
		BEGIN
			IF @i=@current_page
				SET @sql=@sql+'
				 ['+Cast(Cast(@i AS int) AS nvarchar(4))+'] '
			ELSE
				SET @sql=@sql+'
				 <a href="?p='+Cast(@i AS nvarchar(4))+'">'
					+Cast(@i AS nvarchar(4))+'</a> '
			SET @i=@i+1 
		END
		IF @current_page<@total_page
		BEGIN
			SET @sql=@sql+ N'
			 <a href="?p='+Cast((@current_page+1) 
				AS nvarchar(4))+N'">Trang sau</a>'
			 SET @sql=@sql+ N' 
				 <a href="?p='+cast(@total_page AS nvarchar(6))+
				  N'">Trang cuối</a>'
		END
		SELECT @sql AS Paging	
		-- PRINT @sql
	END
END


