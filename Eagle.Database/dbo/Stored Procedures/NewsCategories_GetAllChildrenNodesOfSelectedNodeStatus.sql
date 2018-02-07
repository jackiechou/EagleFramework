CREATE PROCEDURE [dbo].[NewsCategories_GetAllChildrenNodesOfSelectedNodeStatus]
(@LanguageCode nvarchar(5), @CategoryCode uniqueidentifier, @Status int=null)
AS
BEGIN
	SELECT [ApplicationId]
      ,[LanguageCode]
      ,[CategoryId]
      ,[CategoryCode]
      ,ISNULL(ParentId, 0) ParentId
      ,[CategoryName]
      ,[Alias]
      ,[Depth]
      ,[Lineage]
      ,[ListOrder]
      ,[CategoryImage]
      ,[Description]
      ,[NavigateUrl]
      ,[Status]
      ,[CreatedByUserId]
      ,CONVERT(VARCHAR(10), CreatedDate, 103) AS CreatedDate
      ,[LastModifiedByUserId]
      ,CONVERT(VARCHAR(10), LastModifiedDate, 103) AS LastModifiedDate
      ,[IP]
      ,[IPLastUpdated]
  FROM [dbo].[NewsCategories]
	WHERE LanguageCode = @LanguageCode AND (Lineage LIKE ((SELECT Lineage FROM [dbo].[NewsCategories] 
	WHERE [CategoryCode] = @CategoryCode) + '%')) AND (@Status=null or Status = @Status)  	
	ORDER BY Lineage ASC 
END
