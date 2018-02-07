create PROCEDURE [dbo].[BannerPosition_GetTreeNodes](@node int)
AS
BEGIN	
	 IF @node =0 
             BEGIN
				SELECT [Id]
				  ,ISNULL(ParentId, 0) ParentId
				  ,[Depth]
				  ,[Lineage]
				  ,[SortKey]
				  ,CASE  Depth 
					 WHEN 0 THEN BannerPosition
					 WHEN 1 THEN '|__ '+ BannerPosition
					 WHEN 2 THEN '|__ __ '+ BannerPosition
					 WHEN 3 THEN '|__ __ __ ' + BannerPosition
					 WHEN 4 THEN '|__ __ __ __ ' +BannerPosition
					 WHEN 5 THEN '|__ __ __ __ __ __ ' +BannerPosition
					 WHEN 6 THEN '|__ __ __ __ __ __ __ ' +BannerPosition
					 WHEN 7 THEN '|__ __ __ __ __ __ __ __ ' +BannerPosition
					 WHEN 8 THEN '|__ __ __ __ __ __ __ __ __ ' +BannerPosition
					 WHEN 9 THEN '|__ __ __ __ __ __ __ __ __ __ ' +BannerPosition
					 WHEN 10 THEN '|__ __ __ __ __ __ __ __ __ __ __ ' +BannerPosition                         
					 END AS BannerPosition    							
				  ,[Description]
				 ,CONVERT(VARCHAR(10), PostedDate, 103) AS PostedDate
		  ,CONVERT(VARCHAR(10), LastPostedDate, 103) AS LastPostedDate
				  ,[Status]
			  FROM [dbo].[BannerPositions]
			  ORDER BY Lineage ASC                
            END
        ELSE
             BEGIN        
                 SELECT [Id]
				  ,ISNULL(ParentID, 0) ParentID
				  ,[Depth]
				  ,[Lineage]
				  ,[SortKey]
				  ,CASE  Depth 
					 WHEN 0 THEN BannerPosition
					 WHEN 1 THEN '|__ '+ BannerPosition
					 WHEN 2 THEN '|__ __ '+ BannerPosition
					 WHEN 3 THEN '|__ __ __ ' + BannerPosition
					 WHEN 4 THEN '|__ __ __ __ ' +BannerPosition
					 WHEN 5 THEN '|__ __ __ __ __ __ ' +BannerPosition
					 WHEN 6 THEN '|__ __ __ __ __ __ __ ' +BannerPosition
					 WHEN 7 THEN '|__ __ __ __ __ __ __ __ ' +BannerPosition
					 WHEN 8 THEN '|__ __ __ __ __ __ __ __ __ ' +BannerPosition
					 WHEN 9 THEN '|__ __ __ __ __ __ __ __ __ __ ' +BannerPosition
					 WHEN 10 THEN '|__ __ __ __ __ __ __ __ __ __ __ ' +BannerPosition                         
					 END AS [BannerPosition] 			
				  ,[Description]
				  ,CONVERT(VARCHAR(10), PostedDate, 103) AS PostedDate
		  ,CONVERT(VARCHAR(10), LastPostedDate, 103) AS LastPostedDate
				  ,[Status]
					FROM [dbo].[BannerPositions]
                    WHERE Lineage LIKE ((SELECT Lineage FROM [Cms.Articles_Categories] WHERE [Id]=@node) + '%')
                    ORDER BY Lineage ASC    
             END
	
	
END


