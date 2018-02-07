create PROCEDURE [dbo].[NewsCategory_GetAllParentNodesOfSelectedNode]
(@CategoryCode varchar(100))
AS
BEGIN
	WITH Hierarchy(LanguageCode,CategoryId,CategoryCode,CategoryName,Alias,ParentId,Depth,Lineage,
		ListOrder,CategoryImage,Description,NavigateUrl,Status,CreatedDate,LastModifiedDate
		,CreatedByUserId,LastModifiedByUserId,Ip,LastUpdatedIp)
	AS
	(
		SELECT LanguageCode,CategoryId,CategoryCode,CategoryName,Alias,ParentId,Depth,Lineage,
		ListOrder,CategoryImage,Description,NavigateUrl,Status,CreatedDate,LastModifiedDate
		,CreatedByUserId,LastModifiedByUserId,Ip,LastUpdatedIp
		FROM dbo.NewsCategory WHERE CategoryCode = @CategoryCode		
		
		UNION ALL

		SELECT c.LanguageCode,c.CategoryId,c.CategoryCode,c.CategoryName,c.Alias,ISNULL(c.ParentId, 0) ParentId,c.Depth, c.Lineage,	 
		  c.ListOrder,c.CategoryImage,c.Description,c.NavigateUrl,c.Status,c.CreatedDate,c.LastModifiedDate,
		  c.CreatedByUserId,c.LastModifiedByUserId,c.Ip,c.LastUpdatedIp	
		FROM dbo.NewsCategory AS c
		INNER JOIN Hierarchy ON c.CategoryId = Hierarchy.ParentId 
	)

	SELECT LanguageCode,CategoryId,CategoryCode,CategoryName,Alias,ParentId,Depth,Lineage,
		ListOrder,CategoryImage,Description,NavigateUrl,Status,CreatedDate,LastModifiedDate
		,CreatedByUserId,LastModifiedByUserId,Ip,LastUpdatedIp
	FROM Hierarchy ORDER BY Depth ASC
END


