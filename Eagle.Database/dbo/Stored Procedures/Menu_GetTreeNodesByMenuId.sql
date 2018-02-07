CREATE procedure [dbo].[Menu_GetTreeNodesByMenuId](@MenuId int)
as
begin
	SELECT [PageId]
      ,[TypeId]
      ,[MenuId]
      ,[MenuCode]
      ,[ParentId]
      ,[Depth]
      ,[Lineage]
      ,[ListOrder]
      ,[HasChild]
      ,[MenuName]
      ,[MenuTitle]
      ,[MenuAlias]
      ,[Description]
      ,[Target]
      ,[IconClass]
      ,[IconFile]
      ,[Color]
      ,[CssClass]
      ,[IsSecured]
      ,[Status]
      ,[CreatedDate]
      ,[LastModifiedDate]
      ,[CreatedByUserId]
      ,[LastModifiedByUserId]
  FROM [dbo].[Menu]
  where Lineage LIKE ((SELECT Lineage FROM [dbo].[Menu] WHERE [MenuId]=@MenuId) + '%')
end


