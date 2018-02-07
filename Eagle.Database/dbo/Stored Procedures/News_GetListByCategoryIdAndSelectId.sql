create PROCEDURE [dbo].[News_GetListByCategoryIdAndSelectId](@categoryId int, @idx int, @record int)
AS
BEGIN	
	DECLARE @SQL nvarchar(4000)  
    SET @SQL=''
	
	 -- Kiểm tra có bài viết mới hơn không =========================================  
    DECLARE @countNew int  
    SET  @countNew  = (SELECT count(NewsId) FROM [dbo].[News]
    WHERE NewsId>@idx AND CategoryId = @categoryId AND [Status]='2')   
    IF @countNew IS NULL  SET @countNew=0  

	-- Kiểm tra có bài viết cũ hơn không ===========================================
    DECLARE @countOld int
    SET  @countOld  = (SELECT COUNT(NewsId) FROM [dbo].[News]
    WHERE NewsId<@idx AND CategoryId=@categoryId AND [Status]='2')   
    IF @countOld IS NULL SET @countOld=0     

   
	--Nếu có bài viết mới hơn ======================================================
    IF @countNew>0
	BEGIN
		-- Nếu có bài viết cũ hơn thì truy vấn 10 bài mới và 10 bài viết gần nhất
		IF @countOld >0
		BEGIN
			SET @SQL=@SQL+ N'SELECT 999999999999 AS NewsId,
						   N''<b>[Các bài viết mới hơn]</b>'' AS Headline 
							UNION							
							SELECT TOP('+Cast(@record AS nvarchar(20))+') [NewsId],[Title],[Headline]
							FROM [dbo].[News]                         
							WHERE  [Status]=2 AND NewsId >'+Cast(@idx AS nvarchar(20))+' AND CategoryId='+@categoryId+''
			SET @SQL=@SQL+' UNION SELECT '+Cast(@idx AS nvarchar(20))+' AS NewsId, 
							 N''<b>[Các bài đã đăng]</b>'' AS Headline
							UNION
							SELECT [NewsId],[Title],[Headline] FROM [dbo].[News]      
							WHERE  [Status]=2 AND NewsId <'+Cast(@idx AS nvarchar(20)) +' AND CategoryId='+ @categoryId+
							' ORDER BY [NewsId] DESC'

		END

		-- Ngược lại chỉ truy vấn 10 bài viết mới hơn gần nhất
		ELSE
			BEGIN
				SET @SQL =@SQL+N'SELECT 999999999999 AS NewsId, 
						       N''<b>[Các bài mới hơn]</b>'' AS Headline
								UNION      
								SELECT [NewsId],[Title],[Headline] FROM [dbo].[News]    
								WHERE [Status]=2 AND NewsId >' + Cast(@idx AS nvarchar(20))+' AND CategoryId='+@categoryId+
								' ORDER BY NewsId DESC' 
			  END
		END
		
      -- Nếu không có bài viết mới hơn => truy vấn 10 bài viết cũ hơn gần nhất
	ELSE
        BEGIN
              SET @SQL =@SQL+N'SELECT 999999999999 AS NewsId,N''<b>[Các bài đã đăng]</b>'' AS Headline
							UNION                 
							SELECT [NewsId],[Title],[Headline] FROM [dbo].[News]  
							WHERE [Status]=2 AND NewsId <' + Cast(@idx AS nvarchar(20)) +' AND CategoryId='+@categoryId+ 
							' ORDER BY NewsId DESC' 
		END
   
	
	PRINT @SQL
	-- Thực thi trả về danh sách bài viết
	EXEC (@SQL)
      
END


