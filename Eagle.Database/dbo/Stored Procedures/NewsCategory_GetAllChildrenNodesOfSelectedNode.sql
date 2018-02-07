CREATE PROCEDURE [dbo].[NewsCategory_GetAllChildrenNodesOfSelectedNode]
(@CategoryCode varchar(100))
AS
BEGIN	
	SELECT [CategoryId],[CategoryCode],[CategoryName],[Alias],ISNULL(ParentId, 0) ParentId
						  ,[Depth],[Lineage],[ListOrder],[CategoryImage],[Description],[Status]
						   ,CONVERT(VARCHAR(10), CreatedDate, 103) AS CreatedDate
						   ,CONVERT(VARCHAR(10), LastModifiedDate, 103) AS LastModifiedDate,CreatedByUserId,LastModifiedDate,Ip,LastUpdatedIp
						FROM [dbo].[NewsCategory]
						WHERE Lineage LIKE ((SELECT Lineage FROM [dbo].[NewsCategory] WHERE [CategoryCode] = @CategoryCode) + '%')
						ORDER BY Lineage ASC   	

	
END


