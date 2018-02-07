CREATE PROCEDURE [dbo].[NewsCategories_GetAllParentNodesOfSelectedNodeStatus]
(@LanguageCode nvarchar(5), @CategoryCode uniqueidentifier, @Status int=null)
AS
BEGIN
	WITH Hierarchy(ApplicationId,LanguageCode,CategoryId,CategoryCode,CategoryName,Alias,ParentId,Depth,Lineage,
		ListOrder,CategoryImage,Description,NavigateUrl,Status,CreatedOnDate,LastModifiedOnDate
		,CreatedByUserId,LastModifiedByUserId,Ip,IPLastUpdated)
	AS
	(
		SELECT ApplicationId,LanguageCode,CategoryId,CategoryCode,CategoryName,Alias,ParentId,Depth,Lineage,
		ListOrder,CategoryImage,Description,NavigateUrl,Status,CreatedOnDate,LastModifiedOnDate
		,CreatedByUserId,LastModifiedByUserId,Ip,IPLastUpdated
		FROM dbo.NewsCategories WHERE LanguageCode = @LanguageCode AND (CategoryCode = @CategoryCode or @CategoryCode is null) AND (@Status=null or Status = @Status)  		
		
		UNION ALL

		SELECT c.ApplicationId,c.LanguageCode,c.CategoryId,c.CategoryCode,c.CategoryName,c.Alias,ISNULL(c.ParentId, 0) ParentId,c.Depth, c.Lineage,	 
		  c.ListOrder,c.CategoryImage,c.Description,c.NavigateUrl,c.Status,c.CreatedOnDate,c.LastModifiedOnDate,
		  c.CreatedByUserId,c.LastModifiedByUserId,c.Ip,c.IPLastUpdated	
		FROM dbo.NewsCategories AS c
		INNER JOIN Hierarchy ON c.CategoryId = Hierarchy.ParentId 
	)

	SELECT ApplicationId,LanguageCode,CategoryId,CategoryCode,CategoryName,Alias,ParentId,Depth,Lineage,
		ListOrder,CategoryImage,Description,NavigateUrl,Status,CONVERT(VARCHAR(10), CreatedOnDate, 103) AS CreatedOnDate,
		CONVERT(VARCHAR(10), LastModifiedOnDate, 103) AS LastModifiedOnDate,CreatedByUserId ,LastModifiedByUserId,Ip,IPLastUpdated
	FROM Hierarchy ORDER BY Depth ASC
END
