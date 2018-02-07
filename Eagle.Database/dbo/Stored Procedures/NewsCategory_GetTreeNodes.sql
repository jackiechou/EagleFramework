create PROCEDURE [dbo].[NewsCategory_GetTreeNodes](@LanguageCode nchar(10),@node int)
AS
BEGIN	
	 IF @node =0 
        BEGIN
			SELECT [CategoryId]	
			    ,[CategoryCode]	 
				,CASE  Depth 
					 WHEN 0 THEN CategoryName
					 WHEN 1 THEN '|__ '+ CategoryName
					 WHEN 2 THEN '|__ __ '+ CategoryName
					 WHEN 3 THEN '|__ __ __ ' + CategoryName
					 WHEN 4 THEN '|__ __ __ __ ' +CategoryName
					 WHEN 5 THEN '|__ __ __ __ __ __ ' +CategoryName
					 WHEN 6 THEN '|__ __ __ __ __ __ __ ' +CategoryName
					 WHEN 7 THEN '|__ __ __ __ __ __ __ __ ' +CategoryName
					 WHEN 8 THEN '|__ __ __ __ __ __ __ __ __ ' +CategoryName
					 WHEN 9 THEN '|__ __ __ __ __ __ __ __ __ __ ' +CategoryName
					 WHEN 10 THEN '|__ __ __ __ __ __ __ __ __ __ __ ' +CategoryName                         
					 END AS [CategoryName]  
				  ,[Alias]
				  ,ISNULL(ParentId, 0) ParentId
				  ,[Depth]
				  ,[Lineage]
				  ,[ListOrder]
				  ,[CategoryImage]
				  ,[Description]
				  ,[Status]
				  ,CONVERT(VARCHAR(10), CreatedDate, 103) AS CreatedDate
				  ,CONVERT(VARCHAR(10), LastModifiedDate, 103) AS LastModifiedDate
				  ,CreatedByUserId
				  ,LastModifiedDate
				  ,Ip
				  ,LastUpdatedIp
			FROM [dbo].[NewsCategory]
			WHERE LanguageCode = @LanguageCode
			ORDER BY Lineage ASC, ListOrder ASC 	  
         END
   ELSE
       BEGIN        
            SELECT [CategoryId]
                ,[CategoryCode]		 
				,CASE  Depth 
					 WHEN 0 THEN CategoryName
					 WHEN 1 THEN '|__ '+ CategoryName
					 WHEN 2 THEN '|__ __ '+ CategoryName
					 WHEN 3 THEN '|__ __ __ ' + CategoryName
					 WHEN 4 THEN '|__ __ __ __ ' +CategoryName
					 WHEN 5 THEN '|__ __ __ __ __ __ ' +CategoryName
					 WHEN 6 THEN '|__ __ __ __ __ __ __ ' +CategoryName
					 WHEN 7 THEN '|__ __ __ __ __ __ __ __ ' +CategoryName
					 WHEN 8 THEN '|__ __ __ __ __ __ __ __ __ ' +CategoryName
					 WHEN 9 THEN '|__ __ __ __ __ __ __ __ __ __ ' +CategoryName
					 WHEN 10 THEN '|__ __ __ __ __ __ __ __ __ __ __ ' +CategoryName                         
					 END AS [CategoryName]  
				  ,[Alias]
				  ,ISNULL(ParentId, 0) ParentId
				  ,[Depth]
				  ,[Lineage]
				  ,[ListOrder]
				  ,[CategoryImage]
				  ,[Description]
				  ,[Status]
				   ,CONVERT(VARCHAR(10), CreatedDate, 103) AS CreatedDate
				  ,CONVERT(VARCHAR(10), LastModifiedDate, 103) AS LastModifiedDate
				  ,CreatedByUserId
				  ,LastModifiedDate
				  ,Ip
				  ,LastUpdatedIp
			FROM [dbo].[NewsCategory]			 
            WHERE LanguageCode = @LanguageCode
             AND Lineage LIKE ((SELECT Lineage FROM [dbo].[NewsCategory] WHERE [CategoryId]=@node) + '%')
            ORDER BY Lineage ASC, ListOrder ASC  
     END	
END


