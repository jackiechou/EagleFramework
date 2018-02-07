CREATE procedure [dbo].[Menu_GetSubNodesByMenuId](@MenuId int)
as
begin
	WITH RecursiveTable (MenuId, ParentId,MenuName, Level)
	AS
	(    
		SELECT      MenuId, 
					ParentId, 
					MenuName,
					0 AS Level
		FROM [dbo].[Menu]
		WHERE MenuId = @MenuId
	    
		UNION ALL
	    
		SELECT  MTable.MenuId, 
					MTable.ParentId, 
					MTable.MenuName,
					LEVEL + 1
		FROM [dbo].Menu as MTable
			INNER JOIN RecursiveTable RTable ON
			MTable.ParentId = RTable.MenuId
	)
	SELECT * FROM RecursiveTable 
end


